using SFML.Graphics;
using SFML.System;

namespace TAC {
    abstract class InterfaceWindow {
        protected RectangleShape background;
        protected GaussianBlur gaussianBlur;
        protected ScrollBar scrollBar;
        public bool Active {get; set;}

        public InterfaceWindow() {
            background = new RectangleShape(new Vector2f(276.0f, 256.0f));
            background.FillColor = new Color(36, 58, 71, 230);
            background.Position = new Vector2f((Game.displayWidth / 2) - (background.Size.X / 2), (Game.displayHeight / 2) - (background.Size.Y));
            gaussianBlur = new GaussianBlur((int)background.Size.X, (int)background.Size.Y);

            scrollBar = new ScrollBar(74, new Vector2f(background.Position.X + (background.Size.X) - 22, background.Position.Y + 24));

            Active = false;
        }

        public abstract void tick();
        public abstract void render(RenderWindow window);
    }
}