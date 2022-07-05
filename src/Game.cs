using System;
using System.Collections.Generic;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace TAC {

    class Game {

        public static uint displayWidth = 1280;
        public static uint displayHeight = 720;

        private RenderWindow window;
        private Clock tickLimiter;

        private State gameState;
        private Stack<State> states;

        private void render() {
            foreach (State s in states)
            {
                s.render(window);
            }
        }

        private void tick() {
            states.Peek().tick();
        }

        private void run() {
            tickLimiter = new Clock();

            while (window.IsOpen) {

                if (tickLimiter.ElapsedTime.AsMilliseconds() >= 10) {
                    tickLimiter.Restart(); //limit execution to 100 ticks per second, so it runs uniformly on (most) systems, excluding the TI-84 Calculator
                    tick();
                }

                window.DispatchEvents();
                window.Clear();
                render();
                window.Display();
            }
        }

        private void init() {
            window = new RenderWindow(new VideoMode(displayWidth, displayHeight), "The Alpha Chronicles");
            window.Closed += (sender, e) => { ((Window)sender).Close(); }; //add closing event as lambda function 

            Assets.init();

            gameState = new GameState();
            states = new Stack<State>();
            states.Push(gameState);
            
        }

        static void Main(string[] args) {
            Game game = new Game();
            game.init();
            game.run();
        }
    }
}
