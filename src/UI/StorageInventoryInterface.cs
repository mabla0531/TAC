using SFML.Graphics;
using SFML.System;

namespace TAC {
    class StorageInventoryInterface : InterfaceWindow {
        
        public Vector2f Position;
        public Vector2f Size;

        private Player player;
        public StorageEntity entity {get; set;}
        private Sprite currentItem;
        private int index;

        private Text itemLabel;
        private Text itemDescription;
        private Text itemName;
        private RectangleShape highlight;
        private RectangleShape itemHighlight;

        private Button takeButton;

        public StorageInventoryInterface(Player p, StorageEntity e) : base() {
            player = p;
            entity = e;

            Position = new Vector2f(0.0f, 0.0f);
            Size = new Vector2f(276.0f, 256.0f);

            background = new RectangleShape(Size);
            background.FillColor = new Color(36, 58, 71, 230);
            background.Position = Position;
            gaussianBlur = new GaussianBlur((int)background.Size.X, (int)background.Size.Y);

            scrollBar = new ScrollBar(74, new Vector2f(background.Position.X + (background.Size.X) - 22, background.Position.Y + 24));

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

            takeButton = new Button("Take", new Vector2f(background.Position.X + (background.Size.X /2) - 64.0f, background.Position.Y + background.Size.Y - 50.0f));
        
            takeButton.onClick += (sender, e) => {
                if (entity.inventory.Items.Count <= 0)
                    return;

                player.inventory.Items.Add(entity.inventory.Items[index]);
                entity.inventory.Items.RemoveAt(index);
                index -= 1;
            };
        }

        public override void tick() {
            if (!Active) return;

            if ((!new FloatRect(Position, Size).Contains(MouseHandler.MouseX, MouseHandler.MouseY) && (MouseHandler.RightButton || MouseHandler.LeftButton)) || TextInputHandler.Characters.Contains(9)) { //Tab key to close
                Active = false;
                Assets.inventory.Play();
                return;
            }

            if (MouseHandler.WheelMove != 0)
                index -= MouseHandler.WheelMove; //mouse wheel int direction is flipped from index, so -=

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

        public override void render(RenderWindow window) {
            if (!Active) return;
            
            Position = new Vector2f((int)(entity.X - Handler.gameState.gameCameraOffset.X), (int)(entity.Y - Handler.gameState.gameCameraOffset.Y));

            background.Position = Position;
            gaussianBlur.blurArea((int)background.Position.X, (int)background.Position.Y, window);
            window.Draw(background);
            
            highlight.Position = new Vector2f(background.Position.X + 13, background.Position.Y + 52);
            window.Draw(highlight);

            scrollBar.Position = new Vector2f(background.Position.X + (background.Size.X) - 22, background.Position.Y + 24);
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
                    itemLabel.Position = new Vector2f(background.Position.X + 16, background.Position.Y + 50 + offset);
                    itemLabel.FillColor = new Color(200, 200, 200);
                    if (entity.inventory.Items[i].Equipped)
                        itemLabel.FillColor = new Color(212, 175, 55);

                    window.Draw(itemLabel);
                }

                offset += 20;
            }

            Item item = entity.inventory.Items[index];

            itemHighlight.Position = new Vector2f(background.Position.X + (background.Size.X / 4) - (itemHighlight.Size.X / 2), background.Position.Y + 136);
            window.Draw(itemHighlight);

            currentItem = item.Icon;
            currentItem.Scale = new Vector2f(2.0f, 2.0f);
            currentItem.Position = new Vector2f(background.Position.X + (background.Size.X / 4) - (itemHighlight.Size.X / 2), background.Position.Y + 136);
            window.Draw(currentItem);
            
            itemName.DisplayedString = item.Name;
            itemName.Position = new Vector2f(background.Position.X + (background.Size.X / 4) - (itemName.GetLocalBounds().Width / 2), background.Position.Y + 172);
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
            
            itemDescription.Position = new Vector2f(background.Position.X + background.Size.X - 128, background.Position.Y + background.Size.Y - 122);
            itemDescription.DisplayedString = "Value: " + item.Value + "\nWeight:" + item.Weight;
            window.Draw(itemDescription);

            takeButton.Position = new Vector2f(background.Position.X + (background.Size.X /2) - 64.0f, background.Position.Y + background.Size.Y - 50.0f);
            takeButton.render(window);
        }
    }
}
