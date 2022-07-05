using SFML.Graphics;
using SFML.System;
using System;

namespace TAC {
    class Label {

        public Text drawText {get; set;}
        public Vector2f Position {get; set;}
        public float Width {get; set;}
        private Sprite left;
        private Sprite center;
        private Sprite right;

        public Label() {

        }

        public Label(string text, Vector2f position, float width) {
            Width = width;
            Position = position;

            drawText = new Text(text, Assets.defaultFont);
            drawText.Position = new Vector2f(position.X + 8, position.Y + 6);
            drawText.FillColor = Color.White;
            drawText.LetterSpacing = 1;
            drawText.CharacterSize = 18;

            left = new Sprite(Assets.ui, new IntRect(120, 40, 32, 32));
            left.Position = position;
            center = new Sprite(Assets.ui, new IntRect(153, 40, 1, 32));
            center.Position = new Vector2f(position.X + 32, position.Y);
            center.Scale = new Vector2f(Width, 1.0f);
            right = new Sprite(Assets.ui, new IntRect(186, 40, 32, 32));
            right.Position = new Vector2f(position.X + 32 + Width, position.Y);
        }

        public void tick() {
            
        }

        public void render(RenderWindow window) {
            window.Draw(left);
            window.Draw(center);
            window.Draw(right);

            window.Draw(drawText);
        }
    }
}