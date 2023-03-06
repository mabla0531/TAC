using SFML.Graphics;
using SFML.System;

namespace TAC {
    class CheckBox {
        
        public Vector2f Position {get; set;}
        public bool State {get; set;}

        public CheckBox(Vector2f position, bool state) {
            Position = position;
            State = state;

        }
        
        public void tick() {
            if (MouseHandler.LeftPressed && new FloatRect(Position.X, Position.Y, 24, 24).Contains(MouseHandler.MouseX, MouseHandler.MouseY))
                State = !State;
            
        }

        public void render(RenderWindow window) {
            
        }
    }
}