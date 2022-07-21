using SFML.Graphics;
using SFML.System;
using System;

namespace TAC {
    class InventoryInterface {
        
        private Inventory inventory;

        private Vector2f Position;
        private Sprite inventoryBG;
        private Vector2f[] squarePositions;

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
        }

        public void tick() {
            
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
        }
    }
}
