using System;
using System.IO;
using System.Collections.Generic;
using SFML.System;
using SFML.Graphics;

namespace TAC {

    class Map {

        public int Width { get; set; }
        public int Height { get; set; }

        public List<Sprite> tiles = new List<Sprite>();

        public int[,] tileMap = new int[1, 1];

        public void loadMap(string path) {
            List<string> lines = new List<string>(File.ReadAllLines(path));
            Width = lines[0].Split(' ').Length;
            Height = lines.Count;

            tileMap = new int[Height, Width];

            for (int y = 0; y < Height; y++) {
                string[] tokens = lines[y].Split(' ');
                for (int x = 0; x < Width; x++) {
                    tileMap[y, x] = int.Parse(tokens[x]);
                }
            }
        }

        public Map(string path) {
            Sprite s = new Sprite(Assets.terrain);
            s.TextureRect = new IntRect(7 * 32, 1 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            s.TextureRect = new IntRect(1 * 32, 11 * 32, 32, 32);
            tiles.Add(s);

            loadMap(path);
        }

        public void tick() {

        }

        public void render(RenderWindow window, Vector2f gameCameraOffset) {
            for (int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    tiles[tileMap[y, x]].Position = new Vector2f((x * 32) - gameCameraOffset.X, (y * 32) - gameCameraOffset.Y);
                    window.Draw(tiles[tileMap[y, x]]);
                }
            }
        }
    }
}