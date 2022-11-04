using SFML.System;
using SFML.Graphics;

namespace TAC {
    class ScrollBar {

        private Sprite upArrow;
        private Sprite downArrow;
        private Sprite scroll;

        public Vector2f Position {get; set;}

        public bool UpArrowShowing {get; set;}
        public bool DownArrowShowing {get; set;}

        public float ScrollPosition {get; set;}
        public int Height {get; set;}
    
        public ScrollBar(int h, Vector2f p) {
            upArrow = new Sprite(Assets.ui);
            upArrow.TextureRect = new IntRect(116, 140, 8, 8);
            downArrow = new Sprite(Assets.ui);
            downArrow.TextureRect = new IntRect(132, 140, 8, 8);
            scroll = new Sprite(Assets.ui);
            scroll.TextureRect = new IntRect(132, 108, 8, 8);

            Position = p;

            UpArrowShowing = true;
            DownArrowShowing = true;

            Height = h;
            ScrollPosition = 0.0f;
        }
        
        public void tick() {
            
        }

        public void render(RenderWindow window) {
            if (UpArrowShowing) {
                upArrow.Position = new Vector2f(Position.X, Position.Y - 8);
                window.Draw(upArrow);
            }

            if (DownArrowShowing) {
                downArrow.Position = new Vector2f(Position.X, Position.Y + Height);
                window.Draw(downArrow);
            }
            
            if (ScrollPosition > Height - 8)
                ScrollPosition = Height - 8;

            scroll.Position = new Vector2f(Position.X, Position.Y + ScrollPosition);
            window.Draw(scroll);
        }
    }
}