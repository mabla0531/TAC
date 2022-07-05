using SFML.Graphics;
using SFML.System;
using System;

namespace TAC {
    class Button {

        public Text drawText {get; set;}
        public Vector2f Position {get; set;}
        public float Scale {get; set;}
        public const int WIDTH = 72;
        private Sprite sprite;
        private Sprite pressedSprite;
        private bool pressed;

        public event EventHandler onClick;

        public Button() {

        }

        public Button(string text, Vector2f position, float scale) {
            Position = position;
            Scale = scale;

            drawText = new Text(text, Assets.defaultFont);
            drawText.Position = new Vector2f(position.X + (10 * Scale), position.Y + (5 * Scale) + 1);
            drawText.FillColor = Color.Black;
            drawText.LetterSpacing = 1;
            drawText.CharacterSize = (uint)(10 * Scale);

            sprite = new Sprite(Assets.ui, new IntRect(634, 118, 72, 24));
            sprite.Position = position;
            sprite.Scale = new Vector2f(Scale, Scale);
            pressedSprite = new Sprite(Assets.ui, new IntRect(634, 143, 72, 24));
            pressedSprite.Position = position;
            pressedSprite.Scale = new Vector2f(Scale, Scale);

            pressed = false;
        }

        public void tick() {
            if (pressed && !MouseHandler.LeftClick) {
                Assets.walk.Play();
                onClick?.Invoke(this, EventArgs.Empty);
            }

            pressed = false;
            if (MouseHandler.LeftClick && new IntRect((Vector2i)Position, new Vector2i(144, 48)).Contains((int)MouseHandler.MouseX, (int)MouseHandler.MouseY))
                pressed = true;

            sprite.Position = Position;
            pressedSprite.Position = Position;
            drawText.Position = (pressed ? new Vector2f(Position.X + (10 * Scale) + 1, Position.Y + (5 * Scale) + 2) : new Vector2f(Position.X + (10 * Scale), Position.Y + (5 * Scale) + 1));
        }

        public void render(RenderWindow window) {
            window.Draw((pressed ? pressedSprite : sprite));
            window.Draw(drawText);
        }
    }
}