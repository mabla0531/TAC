using SFML.Graphics;
using SFML.System;
using System;

namespace TAC {
    class Button {

        public Text drawText {get; set;}
        public Vector2f Position {get; set;}
        public Vector2f Size {get; set;}
        private bool pressed, hovered;

        private Color buttonColor;
        private RectangleShape buttonRect;
        private bool translucent;
        private GaussianBlur gb;

        public event EventHandler onClick;

        public Button() {

        }

        public Button(string text, Vector2f position, bool t = false) {
            Position = position;
            Size = new Vector2f(128.0f, 42.0f);

            drawText = new Text(text, Assets.defaultFont);
            drawText.Position = new Vector2f(Position.X + 12, Position.Y + 8);
            drawText.FillColor = new Color(200, 200, 200);
            drawText.OutlineColor = Color.Black;
            drawText.OutlineThickness = 1.0f;
            drawText.LetterSpacing = 1;
            drawText.CharacterSize = 20;

            buttonRect = new RectangleShape(Size);
            buttonRect.Position = position;

            translucent = t;
            gb = new GaussianBlur((int)Size.X, (int)Size.Y);

            pressed = false;
            hovered = false;

            buttonColor = new Color(80, 80, 80);
            if (translucent) buttonColor = new Color(80, 80, 80, 200);
            buttonRect.FillColor = buttonColor;

        }

        public void tick() {
            buttonRect.FillColor = buttonColor;

            if (new IntRect((Vector2i)Position, (Vector2i)Size).Contains((int)MouseHandler.MouseX, (int)MouseHandler.MouseY)) {
                buttonRect.FillColor = new Color((byte)(buttonColor.R + 20), (byte)(buttonColor.G + 20), (byte)(buttonColor.B + 20), buttonColor.A);

                if (!hovered)
                    Assets.hover.Play();
                
                hovered = true;
                
                if (MouseHandler.LeftPressed) {
                    Assets.click.Play();
                    pressed = true;
                }

                if (pressed && !MouseHandler.LeftButton) {
                    onClick?.Invoke(this, EventArgs.Empty);
                }
            }

            if (!MouseHandler.LeftButton)
                pressed = false;

            if (!new IntRect((Vector2i)Position, (Vector2i)Size).Contains((int)MouseHandler.MouseX, (int)MouseHandler.MouseY))
                hovered = false;
        }

        public void render(RenderWindow window) {            
            
            buttonRect.Position = Position;
            drawText.Position = new Vector2f(Position.X + 12, Position.Y + 8);
            if (translucent)
                gb.blurArea((int)Position.X, (int)Position.Y, window);

            window.Draw(buttonRect);
            window.Draw(drawText);
        }
    }
}