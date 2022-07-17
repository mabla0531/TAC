using SFML.Graphics;
using SFML.System;
using System;

namespace TAC {
    class InventoryInterface {
        
        private Vector2f Position;
        private Inventory inventory;
        private Sprite inventoryBG;
        public bool Active {get; set;}
        
        public InventoryInterface(Inventory i) {
            inventory = i;

            inventoryBG = new Sprite(Assets.ui, new IntRect(434, 290, 128, 128));
            inventoryBG.Scale = new Vector2f(2.0f, 2.0f);
            inventoryBG.Position = new Vector2f((Game.displayWidth / 2) - (inventoryBG.TextureRect.Width), (Game.displayHeight / 2) - (inventoryBG.TextureRect.Height));

            Active = false;
        }

        public void tick() {
            
        }

        public void render(RenderWindow window) {
            if (!Active) return;
            
            window.Draw(inventoryBG);
            foreach(Item i in inventory.Items) {
                Console.WriteLine(i.Name);
            }
        }
    }
}
