using SFML.Graphics;
using SFML.System;

namespace TAC {
    class CheckBox {
        
        public Vector2f Position {get; set;}
        public bool State {get; set;}
        private Sprite on;
        private Sprite off;
        private Sprite unpressed;
        private Sprite pressed;
        private Sprite frame;
        private bool clicked;

        public CheckBox(Vector2f position, bool state) {
            Position = position;
            State = state;

            clicked = false;

            on = new Sprite(Assets.ui, new IntRect(81, 137, 14, 14));
            off = new Sprite(Assets.ui, new IntRect(97, 137, 14, 14));
            unpressed = new Sprite(Assets.ui, new IntRect(48, 121, 14, 14));
            pressed = new Sprite(Assets.ui, new IntRect(48, 137, 14, 14));
            frame = new Sprite(Assets.ui, new IntRect(762, 86, 24, 24));

        }
        
        public void tick() {
            if (MouseHandler.LeftClick && new FloatRect(Position.X, Position.Y, 24, 24).Contains(MouseHandler.MouseX, MouseHandler.MouseY))
                clicked = true;
            
            if (clicked && !MouseHandler.LeftClick) {
                clicked = false;
                State = !State;
            }
        }

        public void render(RenderWindow window) {
            frame.Position = Position;
            window.Draw(frame);
            pressed.Position = Position + new Vector2f(5.0f, 5.0f);
            unpressed.Position = Position + new Vector2f(5.0f, 5.0f);
            window.Draw((clicked ? pressed : unpressed));
            on.Position = Position + new Vector2f(5.0f, 5.0f);
            off.Position = Position + new Vector2f(5.0f, 5.0f);
            window.Draw((State ? on : off));
        }
    }
}