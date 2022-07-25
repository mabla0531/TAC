using SFML.System;
using SFML.Graphics;
using System;
using System.Collections.Generic;


namespace TAC {
    class ContextMenu {
        public List<ContextButton> buttons {get; set;}
        private RectangleShape background;
        public int X {get; set;}
        public int Y {get; set;}

        public bool Active {get; set;}

        public ContextMenu(List<ContextButton> b) {
            buttons = b;
            background = new RectangleShape(new Vector2f(64, b.Count * 16));
            background.FillColor = new Color(85, 85, 85);
            Active = false;
        }

        public ContextMenu() {
            buttons = new List<ContextButton>();
            background = new RectangleShape();
            Active = false;
        }

        public void tick() {
            if (!Active) return;

            for (int i = 0; i < buttons.Count; i++) {
                buttons[i].Hovered = false;
                if (new IntRect((int)X, (int)Y + (i * 16), 64, 16).Contains((int)MouseHandler.MouseX, (int)MouseHandler.MouseY)) {
                    buttons[i].Hovered = true;
                }
                buttons[i].tick();
            }
        }

        public void render(RenderWindow window) {
            if (!Active) return;
                
            background.Position = new Vector2f(X, Y);
            window.Draw(background);
            for (int i = 0; i < buttons.Count; i++) {
                buttons[i].DrawText.Position = new Vector2f(X - 1, Y + (i * 16) - 1);
                buttons[i].HoverRect.Position = new Vector2f(X, Y + (i * 16));
                buttons[i].render(window);
            }
        }
    }

    class ContextButton {
        public IntRect ClickBounds {get; set;}
        public event EventHandler onClick;
        public Text DrawText {get; set;}

        public RectangleShape HoverRect {get; set;}
        public bool Hovered {get; set;}

        public ContextButton(Text drawText, EventHandler action) {
            DrawText = drawText;

            HoverRect = new RectangleShape(new Vector2f(64.0f, 16.0f));
            HoverRect.FillColor = new Color(160, 160, 160);

            Hovered = false;

            onClick += action;
        }

        public void tick() {
            if (MouseHandler.LeftClick && ClickBounds.Contains((int)MouseHandler.MouseX, (int)MouseHandler.MouseY)) {
                onClick?.Invoke(this, EventArgs.Empty);
            }
        }

        public void render(RenderWindow window) {
            if (Hovered) {
                window.Draw(HoverRect);
            }

            window.Draw(DrawText);
        }
    }
}
