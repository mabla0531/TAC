using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace TAC {

    class Player {
        public float X { get; set; }
        public float Y { get; set; }
        private float moveSpeed = 1.5f;
        private Sprite sprite;

        public Player (float x, float y) {
            X = x;
            Y = y;

            sprite = new Sprite(Assets.player);
            sprite.TextureRect = new IntRect(32, 0, 32, 32);
        }

        public void tick() {
            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
                Y -= moveSpeed;
            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                X -= moveSpeed;
            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                Y += moveSpeed;
            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                X += moveSpeed;
        }

        public void render(RenderWindow window, Vector2f gameCameraOffset) {
            sprite.Position = new Vector2f(X - gameCameraOffset.X, Y - gameCameraOffset.Y);
            window.Draw(sprite);
        }
    }
}