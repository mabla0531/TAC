using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace TAC {
    class InventoryInterface {
        
        private Inventory inventory;

        private Vector2f Position;
        private Sprite inventoryBG;
        private Vector2f[] squarePositions;

        private ContextMenu rightClickMenu;
        private bool contextTriggered;

        public bool Active {get; set;}
        
        public InventoryInterface(Inventory i) {
            inventory = i;

            inventoryBG = new Sprite(Assets.ui, new IntRect(192, 292, 184, 184));
            inventoryBG.Scale = new Vector2f(1.0f, 1.0f);
            inventoryBG.Position = new Vector2f((Game.displayWidth / 2) - (inventoryBG.TextureRect.Width / 2), (Game.displayHeight / 2) - (inventoryBG.TextureRect.Height / 2));

            Active = false;

            squarePositions = new Vector2f[25];
            for (int y = 0; y < 5; y++) {
                for (int x = 0; x < 5; x++) {
                    squarePositions[(y * 5) + x] = new Vector2f(((x + 1) * 4) + (x * 32) + inventoryBG.Position.X, ((y + 1) * 4) + (y * 32) + inventoryBG.Position.Y);
                }
            }

            rightClickMenu = new ContextMenu(new List<ContextButton>(){
                new ContextButton(new Text("test1", Assets.defaultFont, 16), (sender, e) => {Console.WriteLine("test1");}),
                new ContextButton(new Text("test2", Assets.defaultFont, 16), (sender, e) => {Console.WriteLine("test2");}),
            });

            contextTriggered = false;
        }

        public void tick() {
            if (MouseHandler.RightClick && !contextTriggered) {
                rightClickMenu.X = (int)MouseHandler.MouseX;
                rightClickMenu.Y = (int)MouseHandler.MouseY;
                rightClickMenu.Active = !rightClickMenu.Active;
                contextTriggered = true;
            }

            if (!MouseHandler.RightClick)
                contextTriggered = false;

            rightClickMenu.tick();
        }

        public void render(RenderWindow window) {
            if (!Active) return;
            
            window.Draw(inventoryBG);
            Sprite currentSprite;
            for (int i = 0; i < inventory.Items.Count; i++) {
                currentSprite = new Sprite(inventory.Items[i].Icon);
                currentSprite.Scale = new Vector2f(2.0f, 2.0f);
                currentSprite.Position = squarePositions[i];
                window.Draw(currentSprite);
            }

            rightClickMenu.render(window);
        }
    }
}
