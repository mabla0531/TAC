using SFML.Graphics;

namespace TAC {

    class Assets {

        public static Texture terrain;
        public static Texture player;

        public static void init() {
            terrain = new Texture("res/terrain.png");
            player = new Texture("res/player.png");
        }
    }
}