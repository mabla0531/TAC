using SFML.System;
using SFML.Graphics;

namespace TAC {
    class MainMenuState : State {
        private Button startButton;
        private Button loadButton;
        private Button settingsButton;
        private Button exitButton;

        private Sprite menuArt;
        private Sprite logo;

        private Game game;
        
        public MainMenuState(Game game) {
            this.game = game;

            startButton = new Button("Start", new Vector2f(64.0f, 192.0f), true);
            startButton.onClick += (sender, e) => {
                game.startGame();
            };

            loadButton = new Button("Load", new Vector2f(64.0f, 256.0f), true);
            loadButton.onClick += (sender, e) => {
                game.loadGame();
            };

            settingsButton = new Button("Settings", new Vector2f(64.0f, 320.0f), true);
            settingsButton.onClick += (sender, e) => {
                game.showSettings();
            };

            exitButton = new Button("Exit", new Vector2f(64.0f, 384.0f), true);
            exitButton.onClick += (sender, e) => {
                game.stop();
            };

            menuArt = new Sprite(Assets.menuArt, new IntRect(new Vector2i(0, 0), (Vector2i)Assets.menuArt.Size));
            menuArt.Position = new Vector2f(0.0f, 0.0f);
            menuArt.Scale = new Vector2f((float)Game.displayWidth / 1920.0f, (float)Game.displayHeight / 1080.0f); //scale from 1920x1080 to game height
            logo = new Sprite(Assets.logo, new IntRect(new Vector2i(0, 0), (Vector2i)Assets.logo.Size));
            logo.Position = new Vector2f((Game.displayWidth / 2) - (Assets.logo.Size.X / 2), 64.0f);

        }

        public override void tick() {
            startButton.tick();
            loadButton.tick();
            settingsButton.tick();
            exitButton.tick();
            menuArt.Scale = new Vector2f((float)Game.displayWidth / 1920.0f, (float)Game.displayHeight / 1080.0f); //scale from 1920x1080 to game height
            logo.Position = new Vector2f((Game.displayWidth / 2) - (Assets.logo.Size.X / 2), 64.0f);

        }

        public override void render(RenderWindow window) {
            window.Draw(menuArt);
            window.Draw(logo);
            startButton.render(window);
            loadButton.render(window);
            settingsButton.render(window);
            exitButton.render(window);
        }
    }
}
