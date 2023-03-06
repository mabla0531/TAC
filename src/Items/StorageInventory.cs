using SFML.Graphics;
using SFML.System;

namespace TAC {
    class StorageInventory {
        
        public Vector2f Position;
        public Vector2f Size;

        private Player player;
        public StorageEntity entity {get; set;}
        private RectangleShape inventoryBG;
        private GaussianBlur gaussianBlur;
        private Sprite currentItem;
        private int index;

        private ScrollBar scrollBar;

        public bool Active {get; set;}

        private Text itemLabel;
        private Text itemDescription;
        private Text itemName;
        private RectangleShape highlight;
        private RectangleShape itemHighlight;

        private Button takeButton;

        public StorageInventory(Player p, StorageEntity e) {
            player = p;
            entity = e;

            Position = new Vector2f(0.0f, 0.0f);
            Size = new Vector2f(276.0f, 256.0f);

            inventoryBG = new RectangleShape(Size);
            inventoryBG.FillColor = new Color(36, 58, 71, 230);
            inventoryBG.Position = Position;
            gaussianBlur = new GaussianBlur((int)inventoryBG.Size.X, (int)inventoryBG.Size.Y);

            scrollBar = new ScrollBar(74, new Vector2f(inventoryBG.Position.X + (inventoryBG.Size.X) - 22, inventoryBG.Position.Y + 24));

            Active = false;

            itemLabel = new Text("", Assets.defaultFont);
            itemLabel.CharacterSize = 20;
            itemLabel.FillColor = new Color(200, 200, 200);
            itemLabel.OutlineColor = Color.Black;
            itemLabel.OutlineThickness = 1.0f;

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

            takeButton = new Button("Take", new Vector2f(inventoryBG.Position.X + (inventoryBG.Size.X /2) - 64.0f, inventoryBG.Position.Y + inventoryBG.Size.Y - 50.0f));
        
            takeButton.onClick += (sender, e) => {
                if (entity.inventory.Items.Count <= 0)
                    return;

                player.inventory.Items.Add(entity.inventory.Items[index]);
                entity.inventory.Items.RemoveAt(index);
                index -= 1;
            };
        }

        public void tick() {

            if (MouseHandler.WheelMove != 0) {
                index -= MouseHandler.WheelMove; //mouse wheel int direction is flipped from index, so -=
                MouseHandler.WheelMove = 0;
            }

            //maintain constraints of index
            if (index < 0) index = 0;
            if (index >= entity.inventory.Items.Count) index = entity.inventory.Items.Count - 1;

            //update scrollbar position
            scrollBar.ScrollPosition   = ((float)index / (float)(entity.inventory.Items.Count - 1)) * (float)(scrollBar.Height - 8);
            scrollBar.UpArrowShowing   = true;
            scrollBar.DownArrowShowing = true;
            if (index - 2 <= 0) scrollBar.UpArrowShowing = false;
            if (index + 2 >= entity.inventory.Items.Count - 1) scrollBar.DownArrowShowing = false;

            takeButton.tick();
        }

        public void render(RenderWindow window) {

            inventoryBG.Position = Position;
            gaussianBlur.blurArea((int)inventoryBG.Position.X, (int)inventoryBG.Position.Y, window);
            window.Draw(inventoryBG);
            
            highlight.Position = new Vector2f(inventoryBG.Position.X + 13, inventoryBG.Position.Y + 52);
            window.Draw(highlight);

            scrollBar.Position = new Vector2f(inventoryBG.Position.X + (inventoryBG.Size.X) - 22, inventoryBG.Position.Y + 24);
            scrollBar.render(window);

            if (entity.inventory.Items.Count <= 0)
                return;
            
            //maintain constraints of index
            if (index < 0) index = 0;
            if (index >= entity.inventory.Items.Count) index = entity.inventory.Items.Count - 1;


            int offset = -40;
            for (int i = index - 2; i <= index + 2; i++) {
                if (i >= 0 && i < entity.inventory.Items.Count) {    
                    itemLabel.DisplayedString = entity.inventory.Items[i].Name;
                    itemLabel.Position = new Vector2f(inventoryBG.Position.X + 16, inventoryBG.Position.Y + 50 + offset);
                    itemLabel.FillColor = new Color(200, 200, 200);
                    if (entity.inventory.Items[i].Equipped)
                        itemLabel.FillColor = new Color(212, 175, 55);

                    window.Draw(itemLabel);
                }

                offset += 20;
            }

            Item item = entity.inventory.Items[index];

            itemHighlight.Position = new Vector2f(inventoryBG.Position.X + (inventoryBG.Size.X / 4) - (itemHighlight.Size.X / 2), inventoryBG.Position.Y + 136);
            window.Draw(itemHighlight);

            currentItem = item.Icon;
            currentItem.Scale = new Vector2f(2.0f, 2.0f);
            currentItem.Position = new Vector2f(inventoryBG.Position.X + (inventoryBG.Size.X / 4) - (itemHighlight.Size.X / 2), inventoryBG.Position.Y + 136);
            window.Draw(currentItem);
            
            itemName.DisplayedString = item.Name;
            itemName.Position = new Vector2f(inventoryBG.Position.X + (inventoryBG.Size.X / 4) - (itemName.GetLocalBounds().Width / 2), inventoryBG.Position.Y + 172);
            itemName.OutlineColor = new Color(0, 0, 0);
            itemName.OutlineThickness = 1.0f;
            itemName.FillColor = item.ItemRarity switch {
                Item.Rarity.Unique    => new Color(181, 27, 140),
                Item.Rarity.Legendary => new Color(103, 47, 156),
                Item.Rarity.Epic      => new Color(26, 74, 196),
                Item.Rarity.Uncommon  => new Color(51, 196, 26),
                Item.Rarity.Common    => new Color(133, 133, 133),
                Item.Rarity.Useless   => new Color(97, 60, 23),
                                    _ => new Color(97, 60, 23)
            };
            window.Draw(itemName);
            
            itemDescription.Position = new Vector2f(inventoryBG.Position.X + inventoryBG.Size.X - 128, inventoryBG.Position.Y + inventoryBG.Size.Y - 122);
            itemDescription.DisplayedString = "Value: " + item.Value + "\nWeight:" + item.Weight;
            window.Draw(itemDescription);

            takeButton.Position = new Vector2f(inventoryBG.Position.X + (inventoryBG.Size.X /2) - 64.0f, inventoryBG.Position.Y + inventoryBG.Size.Y - 50.0f);
            takeButton.render(window);
        }
    }
}
