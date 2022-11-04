using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace TAC {
    class InventoryInterface {
        
        private Inventory inventory;
        private Vector2f Position;
        private Sprite inventoryBG;
        private int index;

        private ScrollBar scrollBar;

        public bool Active {get; set;}

        private Text itemLabel;
        private RectangleShape highlight;

        public InventoryInterface(Inventory inv) {
            inventory = inv;

            inventoryBG = new Sprite(Assets.ui, new IntRect(434, 290, 128, 128));
            inventoryBG.Scale = new Vector2f(2.0f, 2.0f);
            inventoryBG.Position = new Vector2f((Game.displayWidth / 2) - (inventoryBG.TextureRect.Width), (Game.displayHeight / 2) - (inventoryBG.TextureRect.Height));

            scrollBar = new ScrollBar(74, new Vector2f(inventoryBG.Position.X + (inventoryBG.TextureRect.Width * inventoryBG.Scale.X) - 22, inventoryBG.Position.Y + 24));

            Active = false;

            itemLabel = new Text("", Assets.defaultFont);
            itemLabel.CharacterSize = 20;

            highlight = new RectangleShape(new Vector2f(231.0f, 20.0f));
            highlight.FillColor = new Color(56, 56, 56);
            highlight.OutlineColor = new Color(128, 128, 128);
            highlight.OutlineThickness = 1.0f;
        }

        public void tick() {
            
            if (!Active) //escape clause for non active inventory
                return;
            

            if (MouseHandler.WheelMove != 0) {
                index -= MouseHandler.WheelMove; //mouse wheel int direction is flipped from index, so -=
                MouseHandler.WheelMove = 0;
            }

            //maintain constraints of index
            if (index < 0) index = 0;
            if (index >= inventory.Items.Count) index = inventory.Items.Count - 1;

            //update scrollbar position
            scrollBar.ScrollPosition = ((float)index / (float)(inventory.Items.Count - 1)) * (float)(scrollBar.Height - 8);
            scrollBar.UpArrowShowing   = true;
            scrollBar.DownArrowShowing = true;
            if (index - 2 <= 0) scrollBar.UpArrowShowing = false;
            if (index + 2 >= inventory.Items.Count - 1) scrollBar.DownArrowShowing = false;
        }

            public void render(RenderWindow window) {
            if (!Active) return;
            
            inventoryBG.Position = new Vector2f((Game.displayWidth / 2) - (inventoryBG.TextureRect.Width), (Game.displayHeight / 2) - (inventoryBG.TextureRect.Height));
            window.Draw(inventoryBG);
            highlight.Position = new Vector2f(inventoryBG.Position.X + 13, inventoryBG.Position.Y + 52);
            window.Draw(highlight);

            scrollBar.Position = new Vector2f(inventoryBG.Position.X + (inventoryBG.TextureRect.Width * inventoryBG.Scale.X) - 22, inventoryBG.Position.Y + 24);
            scrollBar.render(window);

            int offset = -40;
            for (int i = index - 2; i <= index + 2; i++) {
                if (i >= 0 && i < inventory.Items.Count) {    
                    itemLabel.DisplayedString = inventory.Items[i].Name;
                    itemLabel.Position = new Vector2f(inventoryBG.Position.X + 16, inventoryBG.Position.Y + 50 + offset);
                    
                    window.Draw(itemLabel);
                }

                offset += 20;
            }
        }
    }
}
