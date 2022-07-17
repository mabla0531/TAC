using SFML.Graphics;

namespace TAC {
    class Item {
        public double Weight {get; set;}
        public int Value {get; set;}
        public string Name {get; set;}
        public Sprite Icon {get; set;}

        public static Item sword = new Item(1.0, 10, "Sword", new Sprite(Assets.items, new IntRect(32, 112, 16, 16)));
        public static Item pickaxe = new Item(2.0, 8, "Pickaxe", new Sprite(Assets.items, new IntRect(80, 96, 16, 16)));
        public static Item shovel = new Item(0.8, 4, "Shovel", new Sprite(Assets.items, new IntRect(64, 96, 16, 16)));
        public static Item axe = new Item(1.5, 5, "Axe", new Sprite(Assets.items, new IntRect(128, 96, 16, 16)));

        public Item(double weight = 0.0, int value = 0, string name = "", Sprite sprite = null) {
            Weight = weight;
            Value = value;
            Name = name;
            Icon = sprite;
        }

        public Item(Item i) {
            Weight = i.Weight;
            Value = i.Value;
            Name = i.Name;
            Icon = i.Icon;
        }
    }
}