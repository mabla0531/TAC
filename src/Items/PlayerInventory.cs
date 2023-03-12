using SFML.Graphics;
using SFML.System;

namespace TAC {
    class PlayerInventory {
        
        private Player player;
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

        private Button equipButton, dropButton;

        public PlayerInventory(Player p) {
            player = p;

            inventoryBG = new RectangleShape(new Vector2f(276.0f, 256.0f));
            inventoryBG.FillColor = new Color(36, 58, 71, 230);
            inventoryBG.Position = new Vector2f((Game.displayWidth / 2) - (inventoryBG.Size.X / 2), (Game.displayHeight / 2) - (inventoryBG.Size.Y));
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

            equipButton = new Button("Equip", new Vector2f(inventoryBG.Position.X + inventoryBG.Size.X - 136.0f, inventoryBG.Position.Y + inventoryBG.Size.Y - 50.0f));
            dropButton = new Button("Drop", new Vector2f(inventoryBG.Position.X + 8.0f, inventoryBG.Position.Y + inventoryBG.Size.Y - 50.0f));
            
            equipButton.onClick += (sender, e) => {
                if (player.inventory.Items.Count <= 0) //if inventory is empty, don't do anything
                
                    return;

                player.inventory.Items[index].Equipped = !player.inventory.Items[index].Equipped;
                Item i = player.inventory.Items[index];
                
                switch (i.ItemSlot) {
                    case Item.Slot.Hand:
                        player.Hand = null;
                        if (i.Equipped)
                            player.Hand = i;
                        break;
                    case Item.Slot.Offhand:
                        player.Offhand = null;
                        if (i.Equipped)
                            player.Offhand = i;
                        break;
                    case Item.Slot.Head:
                        player.Head = null;
                        if (i.Equipped)
                            player.Head = i;
                        break;
                    case Item.Slot.Chest:
                        player.Chest = null;
                        if (i.Equipped)
                            player.Chest = i;
                        break;
                    case Item.Slot.Legs:
                        player.Legs = null;
                        if (i.Equipped)
                            player.Legs = i;
                        break;
                    case Item.Slot.Feet:
                        player.Feet = null;
                        if (i.Equipped)
                            player.Feet = i;
                        break;
                    default:
                        player.Hand = null;
                        if (i.Equipped)
                            player.Hand = i;
                        break;
                }
            };

            dropButton.onClick += (sender, e) => {
                if (player.inventory.Items.Count >= 1) {
                    if (player.inventory.Items[index].Equipped) {
                        switch (player.inventory.Items[index].ItemSlot) {
                            case Item.Slot.Hand: 
                                player.Hand = null;
                                break;
                            case Item.Slot.Offhand: 
                                player.Offhand = null;
                                break;
                            case Item.Slot.Chest: 
                                player.Chest = null;
                                break;
                            case Item.Slot.Legs: 
                                player.Legs = null;
                                break;
                            case Item.Slot.Feet: 
                                player.Feet = null;
                                break;
                        }
                    }
                    
                    player.inventory.Items[index].Equipped = false;
                    Handler.gameState.Items.Add(new GroundItem(player.inventory.Items[index], player.X, player.Y));
                    player.inventory.Items.Remove(player.inventory.Items[index]);
                }
            };
        }

        public void tick() {
            if (!Active) //escape clause for non active inventory
                return;

            foreach (Item i in player.inventory.Items) {
                switch (i.ItemSlot) {
                    case Item.Slot.Hand: 
                        if (player.Hand != i) {
                            i.Equipped = false;
                        }
                        break;
                    case Item.Slot.Offhand: 
                        if (player.Offhand != i) {
                            i.Equipped = false;
                        }
                        break;
                    case Item.Slot.Chest: 
                        if (player.Chest != i) {
                            i.Equipped = false;
                        }
                        break;
                    case Item.Slot.Legs: 
                        if (player.Legs != i) {
                            i.Equipped = false;
                        }
                        break;
                    case Item.Slot.Feet: 
                        if (player.Feet != i) {
                            i.Equipped = false;
                        }
                        break;
                }
            }

            if (MouseHandler.WheelMove != 0) {
                index -= MouseHandler.WheelMove; //mouse wheel int direction is flipped from index, so -=
                MouseHandler.WheelMove = 0;
            }

            //maintain constraints of index
            if (index < 0) index = 0;
            if (index >= player.inventory.Items.Count) index = player.inventory.Items.Count - 1;

            //update scrollbar position
            scrollBar.ScrollPosition   = ((float)index / (float)(player.inventory.Items.Count - 1)) * (float)(scrollBar.Height - 8);
            scrollBar.UpArrowShowing   = true;
            scrollBar.DownArrowShowing = true;
            if (index - 2 <= 0) scrollBar.UpArrowShowing = false;
            if (index + 2 >= player.inventory.Items.Count - 1) scrollBar.DownArrowShowing = false;

            equipButton.tick();
            dropButton.tick();
        }

        public void render(RenderWindow window) {
            if (!Active) return;

            inventoryBG.Position = new Vector2f((Game.displayWidth / 2) - (inventoryBG.Size.X / 2), (Game.displayHeight / 2) - (inventoryBG.Size.Y));
            gaussianBlur.blurArea((int)inventoryBG.Position.X, (int)inventoryBG.Position.Y, window);
            window.Draw(inventoryBG);
            
            highlight.Position = new Vector2f(inventoryBG.Position.X + 13, inventoryBG.Position.Y + 52);
            window.Draw(highlight);

            scrollBar.Position = new Vector2f(inventoryBG.Position.X + (inventoryBG.Size.X) - 22, inventoryBG.Position.Y + 24);
            scrollBar.render(window);

            dropButton.Position = new Vector2f(inventoryBG.Position.X + 8, inventoryBG.Position.Y + inventoryBG.Size.Y - 50);
            dropButton.render(window);
            equipButton.Position = new Vector2f(inventoryBG.Position.X + inventoryBG.Size.X - 136, inventoryBG.Position.Y + inventoryBG.Size.Y - 50);
            equipButton.render(window);

            if (player.inventory.Items.Count <= 0)
                return;

            
            //maintain constraints of index
            if (index < 0) index = 0;
            if (index >= player.inventory.Items.Count) index = player.inventory.Items.Count - 1;


            int offset = -40;
            for (int i = index - 2; i <= index + 2; i++) {
                if (i >= 0 && i < player.inventory.Items.Count) {    
                    itemLabel.DisplayedString = player.inventory.Items[i].Name;
                    itemLabel.Position = new Vector2f(inventoryBG.Position.X + 16, inventoryBG.Position.Y + 50 + offset);
                    itemLabel.FillColor = new Color(200, 200, 200);
                    if (player.inventory.Items[i].Equipped)
                        itemLabel.FillColor = new Color(212, 175, 55);

                    window.Draw(itemLabel);
                }

                offset += 20;
            }

            Item item = player.inventory.Items[index];

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
        }
    }
}
