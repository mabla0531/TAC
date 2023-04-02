/*
all maps that connect must have reversed transitions (in position, out position)
all maps must have a complete tree or transition border (99, 30-39)
one transition per cardinal direction
all maps must be at least 3 wide and 3 tall
*/

using System;

namespace TAC {
    class MapGenerator {
        private int width, height;

        public int[,] GenerateMap() {
            int[,] MapData = new int[width + 2, height + 4];

            //fill the map with 99's on the border and 1's in the middle
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    if (x == 0 || y < 2 || x == width - 1 || y > height - 3) {
                        MapData[x, y] = 99;
                        continue;
                    }
                    MapData[x, y] = 1;
                }
            }

            return MapData;
        }

        public MapGenerator(int w, int h) {
            width = w;
            height = h;
        }

        public MapGenerator() {
            Random random = new Random();
            width = random.Next(3, 128);
            height = random.Next(3, 128);
        }
    }
}