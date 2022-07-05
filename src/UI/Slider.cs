using SFML.Graphics;
using SFML.System;

namespace TAC {
    class Slider {

        public Vector2f Position {get; set;}
        public float Length {get; set;}
        public float Fill {get; set;}
        public float MaxFill {get; set;}

        private Sprite sliderLeft, sliderMiddle, sliderRight;
        private Sprite sliderFill;

        public Slider(Vector2f position, float length) {
            Position = position;
            Length = length;
            MaxFill = length - 18.0f;
            Fill = 2.0f;

            sliderLeft = new Sprite(Assets.ui, new IntRect(258, 64, 24, 16));
            sliderMiddle = new Sprite(Assets.ui, new IntRect(283, 64, 24, 16));
            sliderRight = new Sprite(Assets.ui, new IntRect(308, 64, 24, 16));
            sliderFill = new Sprite(Assets.ui, new IntRect(334, 64, 1, 12));

            sliderLeft.Position = Position;
            sliderMiddle.Position = new Vector2f(Position.X + 24, Position.Y);
            sliderMiddle.Scale = new Vector2f(((length - 48.0f) / 24.0f), 1.0f);
            sliderRight.Position = new Vector2f(Position.X + (length - 24), Position.Y);
            sliderFill.Position = new Vector2f(Position.X + 9, Position.Y + 2);
            sliderFill.Scale = new Vector2f(Fill, 1.0f);
            
        }

        public void tick() {
            if (new FloatRect(Position.X, Position.Y, Length, 16).Contains(MouseHandler.MouseX, MouseHandler.MouseY) && MouseHandler.LeftClick) {
                Fill = MouseHandler.MouseX - (Position.X + 9);
            }

            if (Fill > MaxFill)
                Fill = MaxFill;

            if (Fill < 0.0f)
                Fill = 0.0f;
        }

        public void render(RenderWindow window) {
            window.Draw(sliderLeft);
            window.Draw(sliderMiddle);
            window.Draw(sliderRight);
            sliderFill.Scale = new Vector2f(Fill, 1.0f);
            window.Draw(sliderFill);
        }
    }
}