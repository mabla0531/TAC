using SFML.Graphics;

namespace TAC {
    class Item {
        public double Weight {get; set;}
        public int Value {get; set;}
        public string Name {get; set;}
        public Sprite Icon {get; set;}
        public Rarity ItemRarity {get; set;}
        public Slot ItemSlot {get; set;}
        public bool Equipped {get; set;}
        

        public static Item sword = new Item(1.0, 10, "Sword", new Sprite(Assets.items, new IntRect(32, 112, 16, 16)), Rarity.Legendary, Slot.Hand);
        public static Item pickaxe = new Item(2.0, 8, "Pickaxe", new Sprite(Assets.items, new IntRect(80, 96, 16, 16)), Rarity.Common, Slot.Hand);
        public static Item shovel = new Item(0.8, 4, "Shovel", new Sprite(Assets.items, new IntRect(64, 96, 16, 16)), Rarity.Unique, Slot.Hand);
        public static Item axe = new Item(1.5, 5, "Axe", new Sprite(Assets.items, new IntRect(128, 96, 16, 16)), Rarity.Epic, Slot.Hand);

        public enum Rarity {
            Unique,
            Legendary,
            Epic,
            Uncommon,
            Common,
            Useless
        }

        public enum Slot {
            Head,
            Chest,
            Legs,
            Feet,
            Offhand,
            Hand
        }

        public Item(double weight = 0.0, int value = 0, string name = "", Sprite sprite = null, Rarity itemRarity = Rarity.Common, Slot itemSlot = Slot.Hand) {
            Equipped = false;
            Weight = weight;
            Value = value;
            Name = name;
            Icon = sprite;
            ItemRarity = itemRarity;
            ItemSlot = itemSlot;
        }

        public Item(Item i) {
            Equipped = false;
            Weight = i.Weight;
            Value = i.Value;
            Name = i.Name;
            Icon = i.Icon;
        }
    }
}