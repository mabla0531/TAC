using SFML.Graphics;

namespace TAC {
    class Item {
        public float Weight {get; set;}
        public int Value {get; set;}
        public string Name {get; set;}

        public int Attack {get; set;}
        public int Defense {get; set;}
        public int Stamina {get; set;}
        public int Health {get; set;}

        public Sprite Icon {get; set;}
        public Rarity ItemRarity {get; set;}
        public Slot ItemSlot {get; set;}
        public bool Equipped {get; set;}
        

        public static Item sword = new Item(1.0f, 10, "Sword", 2, 0, 0, 0, new IntRect(32, 112, 16, 16), Rarity.Legendary, Slot.Hand);
        public static Item pickaxe = new Item(2.0f, 8, "Pickaxe", 1, 0, 0, 0, new IntRect(80, 96, 16, 16), Rarity.Common, Slot.Hand);
        public static Item shovel = new Item(0.8f, 4, "Shovel", 1, 0, 0, 0, new IntRect(64, 96, 16, 16), Rarity.Unique, Slot.Hand);
        public static Item axe = new Item(1.5f, 5, "Axe", 3, 0, 0, 0, new IntRect(128, 96, 16, 16), Rarity.Epic, Slot.Hand);

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

        public Item(float weight = 0.0f, int value = 0, string name = "", int attack = 0, int defense = 0, int health = 0, int stamina = 0, IntRect spriteRect = new IntRect(), Rarity itemRarity = Rarity.Common, Slot itemSlot = Slot.Hand) {
            Equipped = false;
            Weight = weight;
            Value = value;
            Name = name;
            Icon = new Sprite();
            Icon.Texture = Assets.items;
            Icon.TextureRect = spriteRect;
            ItemRarity = itemRarity;
            ItemSlot = itemSlot;
            Attack = attack;
            Defense = defense;
            Health = health;
            Stamina = stamina;
        }
    }
}