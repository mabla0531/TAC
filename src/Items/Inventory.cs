using System.Collections.Generic;

namespace TAC {
    class Inventory {
        private List<Item> items;
        private int money;

        public void addItem (Item item) {
            items.Add(item);
        }

        public double getTotalWeight() {
            double weight = 0;
            foreach (Item i in items) {
                weight += i.Weight;
            }

            return weight;
        }
    }
}
