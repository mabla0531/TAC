using System.Collections.Generic;

namespace TAC {

    class WorldGenerator {
        private int mapsToGenerate;
        private List<int[,]> maps;

        public WorldGenerator(int mapCount) {
            mapsToGenerate = mapCount;
        }

        public void GenerateWorld() {
            //generate maps preliminarily
            for (int mapIndex = 0; mapIndex < mapsToGenerate; mapIndex++) {
                MapGenerator mapGenerator = new MapGenerator();
                maps.Add(mapGenerator.GenerateMap());
            }
        }

        //center map should have four maps surrounding
        //each next map should share a map with the one diagonal to it
    }
}