using System.Collections.Generic;

namespace TAC {
    class Inventory {
        public List<Item> Items {get; set;}
        public int money {get; set;}

        public Inventory() {
            Items = new List<Item>();
        }

        public double getTotalWeight() {
            double weight = 0;
            foreach (Item i in Items) {
                weight += i.Weight;
            }

            return weight;
        }
    }
}
