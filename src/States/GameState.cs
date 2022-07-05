using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace TAC {

    class GameState : State {
        
        private Map map1;
        private Player player;
        private Vector2f gameCameraOffset;

        public GameState() : base() {
            map1 = new Map("res/test.map");
            player = new Player(10, 10);
            gameCameraOffset = new Vector2f(0.0f, 0.0f);
        }

        public override void tick() {
            map1.tick();
            player.tick();

            gameCameraOffset.X = player.X + 16 - (Game.displayWidth / 2);
            gameCameraOffset.Y = player.Y + 16 - (Game.displayHeight / 2);
            if (gameCameraOffset.X < 0) gameCameraOffset.X = 0;
            if (gameCameraOffset.Y < 0) gameCameraOffset.Y = 0;
            if (gameCameraOffset.X > (map1.Width * 32) - Game.displayWidth) gameCameraOffset.X = (map1.Width * 32) - Game.displayWidth;
            if (gameCameraOffset.Y > (map1.Height * 32) - Game.displayHeight) gameCameraOffset.Y = (map1.Height * 32) - Game.displayHeight;
        }

        public override void render(RenderWindow window) {
            map1.render(window, gameCameraOffset);
            player.render(window, gameCameraOffset);
        }
    }
}