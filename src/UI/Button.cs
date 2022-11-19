using SFML.Graphics;
using SFML.System;
using System;

namespace TAC {
    class Button {

        public Text drawText {get; set;}
        public Vector2f Position {get; set;}
        public Vector2f Size {get; set;}
        private bool pressed;

        private RectangleShape buttonRect;

        public event EventHandler onClick;

        public Button() {

        }

        public Button(string text, Vector2f position) {
            Position = position;
            Size = new Vector2f(128.0f, 42.0f);

            drawText = new Text(text, Assets.defaultFont);
            drawText.Position = new Vector2f(Position.X + 12, Position.Y + 8);
            drawText.FillColor = new Color(200, 200, 200);
            drawText.LetterSpacing = 1;
            drawText.CharacterSize = 20;

            buttonRect = new RectangleShape(Size);
            buttonRect.FillColor = new Color(80, 80, 80);
            buttonRect.Position = position;

            pressed = false;
        }

        public void tick() {
            if (pressed && !MouseHandler.LeftClick) {
                Assets.walk.Play();
                onClick?.Invoke(this, EventArgs.Empty);
            }

            pressed = false;

            buttonRect.FillColor = new Color(80, 80, 80);
            if (new IntRect((Vector2i)Position, (Vector2i)Size).Contains((int)MouseHandler.MouseX, (int)MouseHandler.MouseY)) {
                buttonRect.FillColor = new Color(100, 100, 100);
                if (MouseHandler.LeftClick)
                    pressed = true;
            }

            buttonRect.Position = Position;
            drawText.Position = new Vector2f(Position.X + 12, Position.Y + 8);
        }

        public void render(RenderWindow window) {
            window.Draw(buttonRect);
            window.Draw(drawText);
        }
    }
}