using SFML.Graphics;
using SFML.System;

namespace TAC {
    class Slider {

        public Vector2f Position {get; set;}
        public float Length {get; set;}
        public float Fill {get; set;}

        private RectangleShape slider;
        private RectangleShape sliderFill;

        public Slider(Vector2f position, float length) {
            Position = position;
            Length = length;
            Fill = 2.0f;

            slider = new RectangleShape(new Vector2f(length, 12));
            sliderFill = new RectangleShape(new Vector2f(length, 12));

            slider.Position = Position;
            slider.FillColor = new Color(200, 200, 200);
            sliderFill.FillColor = Color.Blue;
            sliderFill.Position = Position;

        }

        public void tick() {
            if (new FloatRect(Position.X - 4, Position.Y - 4, Length + 8, 20).Contains(MouseHandler.MouseX, MouseHandler.MouseY) && MouseHandler.LeftClick) {
                Fill = MouseHandler.MouseX - (Position.X);
            }

            if (Fill > Length)
                Fill = Length;

            if (Fill < 0.0f)
                Fill = 0.0f;
        }

        public void render(RenderWindow window) {
            window.Draw(slider);
            sliderFill.Size = new Vector2f(Fill, 12.0f);
            window.Draw(sliderFill);
        }
    }
}