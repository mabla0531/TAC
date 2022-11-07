using SFML.Graphics;
using SFML.System;

namespace TAC {
    class InventoryInterface {
        
        private Player player;
        private Sprite inventoryBG;
        private Sprite currentItem;
        private int index;

        private ScrollBar scrollBar;

        public bool Active {get; set;}

        private Text itemLabel;
        private Text itemDescription;
        private Text itemName;
        private RectangleShape highlight;
        private RectangleShape itemHighlight;

        private Button equipButton;

        public InventoryInterface(Player p) {
            player = p;

            inventoryBG = new Sprite(Assets.ui, new IntRect(434, 290, 128, 128));
            inventoryBG.Scale = new Vector2f(2.0f, 2.0f);
            inventoryBG.Position = new Vector2f((Game.displayWidth / 2) - (inventoryBG.TextureRect.Width), (Game.displayHeight / 2) - (inventoryBG.TextureRect.Height));

            scrollBar = new ScrollBar(74, new Vector2f(inventoryBG.Position.X + (inventoryBG.TextureRect.Width * inventoryBG.Scale.X) - 22, inventoryBG.Position.Y + 24));

            Active = false;

            itemLabel = new Text("", Assets.defaultFont);
            itemLabel.CharacterSize = 20;

            itemDescription = new Text("", Assets.defaultFont);
            itemDescription.CharacterSize = 16;

            itemName = new Text("", Assets.defaultFont);
            itemName.CharacterSize = 20;   

            highlight = new RectangleShape(new Vector2f(231.0f, 20.0f));
            highlight.FillColor = new Color(56, 56, 56);
            highlight.OutlineColor = new Color(128, 128, 128);
            highlight.OutlineThickness = 1.0f;

            itemHighlight = new RectangleShape(new Vector2f(32.0f, 32.0f));
            itemHighlight.FillColor = new Color(56, 56, 56);
            itemHighlight.OutlineColor = new Color(128, 128, 128);
            itemHighlight.OutlineThickness = 1.0f;

            equipButton = new Button("Equip", new Vector2f(inventoryBG.Position.X + 106, inventoryBG.Position.Y + 202), 2.0f);
            
            equipButton.onClick += (sender, e) => {
                Item i = player.inventory.Items[index];
                
                if (i.ItemSlot == Item.Slot.Hand)
                    player.Hand = i;
                else if (i.ItemSlot == Item.Slot.Offhand)
                    player.Offhand = i;
                else if (i.ItemSlot == Item.Slot.Head)
                    player.Head = i;
                else if (i.ItemSlot == Item.Slot.Chest)
                    player.Chest = i;
                else if (i.ItemSlot == Item.Slot.Legs)
                    player.Legs = i;
                else if (i.ItemSlot == Item.Slot.Feet)
                    player.Feet = i;
                else
                    player.Hand = i;
            };
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
            if (index >= player.inventory.Items.Count) index = player.inventory.Items.Count - 1;

            //update scrollbar position
            scrollBar.ScrollPosition = ((float)index / (float)(player.inventory.Items.Count - 1)) * (float)(scrollBar.Height - 8);
            scrollBar.UpArrowShowing   = true;
            scrollBar.DownArrowShowing = true;
            if (index - 2 <= 0) scrollBar.UpArrowShowing = false;
            if (index + 2 >= player.inventory.Items.Count - 1) scrollBar.DownArrowShowing = false;

            equipButton.tick();
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
                if (i >= 0 && i < player.inventory.Items.Count) {    
                    itemLabel.DisplayedString = player.inventory.Items[i].Name;
                    itemLabel.Position = new Vector2f(inventoryBG.Position.X + 16, inventoryBG.Position.Y + 50 + offset);
                    
                    window.Draw(itemLabel);
                }

                offset += 20;
            }

            Item item = player.inventory.Items[index];

            itemHighlight.Position = new Vector2f(inventoryBG.Position.X + 16, inventoryBG.Position.Y + 128);
            window.Draw(itemHighlight);

            currentItem = item.Icon;
            currentItem.Scale = new Vector2f(2.0f, 2.0f);
            currentItem.Position = new Vector2f(inventoryBG.Position.X + 16, inventoryBG.Position.Y + 128);
            window.Draw(currentItem);
            
            itemDescription.Position = new Vector2f(inventoryBG.Position.X + 16, inventoryBG.Position.Y + 164);
            itemDescription.DisplayedString = "Value: " + item.Value + "\nWeight:" + item.Weight;
            window.Draw(itemDescription);

            itemName.Position = new Vector2f(inventoryBG.Position.X + 52, inventoryBG.Position.Y + 132);
            itemName.DisplayedString = item.Name;

            itemName.OutlineColor = new Color(0, 0, 0);
            itemName.OutlineThickness = 1.0f;

            itemName.FillColor = item.ItemRarity switch {
                Item.Rarity.Unique => new Color(181, 27, 140),
                Item.Rarity.Legendary => new Color(103, 47, 156),
                Item.Rarity.Epic => new Color(26, 74, 196),
                Item.Rarity.Uncommon => new Color(51, 196, 26),
                Item.Rarity.Common => new Color(133, 133, 133),
                Item.Rarity.Useless => new Color(97, 60, 23),
                _ => new Color(97, 60, 23)
            };

            window.Draw(itemName);

            equipButton.Position = new Vector2f(inventoryBG.Position.X + 106, inventoryBG.Position.Y + 202);
            equipButton.render(window);
        }
    }
}
