using System.IO;
using System.Linq;
using System.Collections.Generic;
using SFML.System;
using SFML.Graphics;

namespace TAC {

    class Map {

        public int Width { get; set; }
        public int Height { get; set; }

        public List<Sprite> tiles = new List<Sprite>();
        public List<int> solidTiles { get; set; }

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

        public void initTileSprites() {
            Sprite s = new Sprite(Assets.terrain);
            //0 - dirt
            s.TextureRect = new IntRect(7 * 32, 1 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //grass
            s.TextureRect = new IntRect(1 * 32, 11 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //grass corner NW
            s.TextureRect = new IntRect(6 * 32, 0 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //grass corner NE
            s.TextureRect = new IntRect(8 * 32, 0 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //4 - grass corner SW
            s.TextureRect = new IntRect(6 * 32, 2 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //grass corner SE
            s.TextureRect = new IntRect(8 * 32, 2 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //grass edge N
            s.TextureRect = new IntRect(7 * 32, 0 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //grass edge W
            s.TextureRect = new IntRect(6 * 32, 1 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //grass edge E
            s.TextureRect = new IntRect(8 * 32, 1 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //9 - grass edge S
            s.TextureRect = new IntRect(7 * 32, 2 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //grass inner corner NW
            s.TextureRect = new IntRect(6 * 32, 3 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //grass inner corner NE
            s.TextureRect = new IntRect(7 * 32, 3 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //grass inner corner SW
            s.TextureRect = new IntRect(6 * 32, 4 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //grass inner corner SE
            s.TextureRect = new IntRect(7 * 32, 4 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //14 - stone
            s.TextureRect = new IntRect(4 * 32, 1 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //stone corner NW
            s.TextureRect = new IntRect(3 * 32, 0 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //stone corner NE
            s.TextureRect = new IntRect(5 * 32, 0 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //stone corner SW
            s.TextureRect = new IntRect(3 * 32, 2 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //stone corner SE
            s.TextureRect = new IntRect(5 * 32, 2 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //19 - stone edge N
            s.TextureRect = new IntRect(4 * 32, 0 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //stone edge W
            s.TextureRect = new IntRect(3 * 32, 1 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //stone edge E
            s.TextureRect = new IntRect(5 * 32, 1 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //stone edge S
            s.TextureRect = new IntRect(4 * 32, 2 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //stone inner corner NW
            s.TextureRect = new IntRect(3 * 32, 3 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //24 - stone inner corner NE
            s.TextureRect = new IntRect(4 * 32, 3 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //stone inner corner SW
            s.TextureRect = new IntRect(3 * 32, 4 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //stone inner corner SE
            s.TextureRect = new IntRect(4 * 32, 9 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //water cliff corner NW
            s.TextureRect = new IntRect(0 * 32, 10 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //water cliff corner NE
            s.TextureRect = new IntRect(2 * 32, 10 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //29 - water cliff corner SW
            s.TextureRect = new IntRect(0 * 32, 12 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //water cliff corner SE
            s.TextureRect = new IntRect(2 * 32, 12 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //water cliff edge N
            s.TextureRect = new IntRect(1 * 32, 10 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //water cliff edge W
            s.TextureRect = new IntRect(0 * 32, 11 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //water cliff edge E
            s.TextureRect = new IntRect(2 * 32, 11 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //34 - water cliff edge S
            s.TextureRect = new IntRect(1 * 32, 12 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //water
            s.TextureRect = new IntRect(3 * 32, 10 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //water corner SW
            s.TextureRect = new IntRect(0 * 32, 13 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //water edge S
            s.TextureRect = new IntRect(1 * 32, 13 * 32, 32, 32);
            tiles.Add(s);
            s = new Sprite(Assets.terrain);
            //water corner SE
            s.TextureRect = new IntRect(2 * 32, 13 * 32, 32, 32);
            tiles.Add(s);
        }

        public int getTileByCoords(int x, int y) {
            return tileMap[y / 32, x / 32];
        }

        public Map(string path) {

            initTileSprites();

            solidTiles = new List<int>(Enumerable.Range(35, 38));

            loadMap(path);
        }

        public void tick() {

        }

        public void render(RenderWindow window, Vector2i gameCameraOffset) {
            for (int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    if ((x * 32) - gameCameraOffset.X > -32 && (x * 32) - gameCameraOffset.X < Game.displayWidth && (y * 32) - gameCameraOffset.Y > -32 && (y * 32) - gameCameraOffset.Y < Game.displayHeight) {
                        tiles[tileMap[y, x]].Position = new Vector2f((x * 32) - gameCameraOffset.X, (y * 32) - gameCameraOffset.Y);
                        window.Draw(tiles[tileMap[y, x]]);
                    }
                }
            }
        }
    }
}