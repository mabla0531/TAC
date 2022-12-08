using SFML.Graphics;
using SFML.System;

namespace TAC {
    class CheckBox {
        
        public Vector2f Position {get; set;}
        public bool State {get; set;}

        private bool clicked;

        public CheckBox(Vector2f position, bool state) {
            Position = position;
            State = state;

            clicked = false;



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
            
        }
    }
}