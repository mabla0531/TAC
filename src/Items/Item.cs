namespace TAC {
    class Item {
        public double Weight {get; set;}
        public int Value {get; set;}
        public string Name {get; set;}

        public Item(double weight = 0.0, int value = 0, string name = "") {
            this.Weight = weight;
            this.Value = value;
            this.Name = name;
        }
    }
}