using System.Collections.Generic;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace TAC {

    class Game {

        private bool running = false;

        public static uint displayWidth = 1280;
        public static uint displayHeight = 720;

        private uint windowedWidth = 1280;
        private uint windowedHeight = 720;
        private bool enterPressed = false;

        private RenderWindow window;
        private Cursor defaultCursor;
        private Clock tickLimiter;

        private GameState gameState;
        private MainMenuState mainMenuState;
        private Stack<State> states;

        private void render() {
            states.Peek().render(window); //render the top state
        }

        private void tick() {
            if (new IntRect(0, 0, (int)displayWidth, (int)displayHeight).Contains((int)MouseHandler.MouseX, (int)MouseHandler.MouseY) && window.HasFocus())
                window.SetMouseCursor(defaultCursor); //for some reason when the cursor leaves the window it doesn't change back always
            states.Peek().tick(); //tick the top state
        }

        public void startGame() {
            states.Push(gameState);
        }

        public void loadGame() {
            gameState.loadGame();
            states.Push(gameState);
        }

        public void showSettings() {
            states.Push(new SettingsState());
        }

        public void popState() {
           states.Pop();
        }

        public void stop() {
            running = false;
        }

        private void run() {
            running = true;
            tickLimiter = new Clock();

            while (running) {
                if (tickLimiter.ElapsedTime.AsMilliseconds() >= 10) {
                    tickLimiter.Restart(); //limit execution to 100 ticks per second, so it runs 
                                           //uniformly on (most) systems, excluding the TI-84 Calculator
                    if ((Keyboard.IsKeyPressed(Keyboard.Key.RAlt) || Keyboard.IsKeyPressed(Keyboard.Key.LAlt)) && Keyboard.IsKeyPressed(Keyboard.Key.Enter) && !enterPressed) {
                        //if Alt+Enter is pressed
                        enterPressed = true;
                        SettingsState.Fullscreen = !SettingsState.Fullscreen; //flip fullscreen flag
                        window.Dispose();
                        initDisplay();
                    }

                    if (!Keyboard.IsKeyPressed(Keyboard.Key.Enter))
                        enterPressed = false;

                    MouseHandler.MouseX = Mouse.GetPosition(window).X;
                    MouseHandler.MouseY = Mouse.GetPosition(window).Y;

                    if (window.HasFocus())
                        tick();
                }

                window.DispatchEvents();
                window.Clear();
                render();
                window.Display();
            }
        }

        private void initDisplay() {

            window = new RenderWindow((SettingsState.Fullscreen ? VideoMode.DesktopMode : new VideoMode(windowedWidth, windowedHeight)), "The Alpha Chronicles", (SettingsState.Fullscreen ? Styles.Fullscreen : Styles.Default));
            displayWidth = SettingsState.Fullscreen ? VideoMode.DesktopMode.Width : windowedWidth;
            displayHeight = SettingsState.Fullscreen ? VideoMode.DesktopMode.Height : windowedHeight;

            window.Closed += (sender, e) => {
                running = false;
                ((Window)sender).Close();
                }; //add closing event as lambda function
            window.MouseButtonPressed += (sender, e) => {
                if (e.Button == Mouse.Button.Left) MouseHandler.LeftClick = true;
                if (e.Button == Mouse.Button.Right) MouseHandler.RightClick = true;
            };
            window.MouseButtonReleased += (sender, e) => {
                if (e.Button == Mouse.Button.Left) MouseHandler.LeftClick = false;
                if (e.Button == Mouse.Button.Right) MouseHandler.RightClick = false;
            };
            window.MouseWheelScrolled += (sender, e) => {
                if (e.Delta < 0.0) MouseHandler.WheelMove = -1;
                if (e.Delta > 0.0) MouseHandler.WheelMove =  1;
            };

            window.SetMouseCursor(defaultCursor);
        }

        private void init() {
            SettingsState.load();
            Assets.init();
            defaultCursor = new Cursor(Assets.cursorData, new Vector2u(12, 17), new Vector2u(0, 0));
            initDisplay();
            gameState = new GameState();
            Handler.gameState = gameState;
            mainMenuState = new MainMenuState(this);
            states = new Stack<State>();
            states.Push(mainMenuState);
        }

        static void Main(string[] args) {
            Game game = new Game();
            Handler.game = game;

            game.init();
            game.run();

            Assets.cleanup();
        }
    }
}
