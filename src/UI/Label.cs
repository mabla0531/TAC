using SFML.Graphics;
using SFML.System;
using System;

namespace TAC {
    class Label {

        public Text drawText {get; set;}
        public Vector2f Position {get; set;}
        public float Width {get; set;}

        private RectangleShape background;

        public Label() {

        }

        public Label(string text, Vector2f position, float width) {
            Width = width;
            Position = position;

            drawText = new Text(text, Assets.defaultFont);
            drawText.Position = new Vector2f(position.X + 8, position.Y + 6);
            drawText.FillColor = new Color(200, 200, 200);
            drawText.OutlineColor = Color.Black;
            drawText.OutlineThickness = 1.0f;
            drawText.LetterSpacing = 1;
            drawText.CharacterSize = 18;

            background = new RectangleShape(new Vector2f(width, 32));
            background.FillColor = new Color(80, 80, 80);
            background.Position = position;
        }

        public void tick() {
            
        }

        public void render(RenderWindow window) {
            background.Position = Position;
            window.Draw(background);

            window.Draw(drawText);
        }
    }
}