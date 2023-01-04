using System.IO;
using System.Collections.Generic;
using SFML.System;
using SFML.Graphics;

namespace TAC {

    class Map {

        public int Width { get; set; }
        public int Height { get; set; }

        public List<Sprite> tiles = new List<Sprite>();
        public List<int> SolidTiles { get; set; }

        public Sprite treeBorder1, treeBorder2, treeBorder3, treeBorder4;

        public int[,] tileMap = new int[1, 1];

        public void loadMap(string path) {
            List<string> lines = new List<string>(File.ReadAllLines(path));
            Width = lines[0].Split(' ').Length;
            Height = lines.Count;

            tileMap = new int[Height, Width];

            SolidTiles = new List<int>{99};

            treeBorder1 = new Sprite(Assets.terrain, new IntRect(608, 448, 32, 32));
            treeBorder2 = new Sprite(Assets.terrain, new IntRect(608, 480, 32, 32));
            treeBorder3 = new Sprite(Assets.terrain, new IntRect(640, 448, 32, 32));
            treeBorder4 = new Sprite(Assets.terrain, new IntRect(640, 480, 32, 32));

            for (int y = 0; y < Height; y++) {
                string[] tokens = lines[y].Split(' ');
                for (int x = 0; x < Width; x++) {
                    tileMap[y, x] = int.Parse(tokens[x]);
                }
            }
        }

        public void initTileSprites() {
            Sprite s = new Sprite(Assets.terrain);
            //0 - sand
            s.TextureRect = new IntRect(4 * 32, 6 * 32, 32, 32);
            tiles.Add(new Sprite(s));
            //grass
            s.TextureRect = new IntRect(1 * 32, 2 * 32, 32, 32);
            tiles.Add(new Sprite(s));
            //grass corner NW
            s.TextureRect = new IntRect(0 * 32, 1 * 32, 32, 32);
            tiles.Add(new Sprite(s));
            //grass corner NE
            s.TextureRect = new IntRect(2 * 32, 1 * 32, 32, 32);
            tiles.Add(new Sprite(s));
            //4 - grass corner SW
            s.TextureRect = new IntRect(0 * 32, 3 * 32, 32, 32);
            tiles.Add(new Sprite(s));
            //grass corner SE
            s.TextureRect = new IntRect(2 * 32, 3 * 32, 32, 32);
            tiles.Add(new Sprite(s));
            //grass edge N
            s.TextureRect = new IntRect(1 * 32, 1 * 32, 32, 32);
            tiles.Add(new Sprite(s));
            //grass edge W
            s.TextureRect = new IntRect(0 * 32, 2 * 32, 32, 32);
            tiles.Add(new Sprite(s));
            //grass edge E
            s.TextureRect = new IntRect(2 * 32, 2 * 32, 32, 32);
            tiles.Add(new Sprite(s));
            //9 - grass edge S
            s.TextureRect = new IntRect(1 * 32, 3 * 32, 32, 32);
            tiles.Add(new Sprite(s));
            //tree 1
            s.TextureRect = new IntRect(2 * 32, 12 * 32, 32, 32);
            tiles.Add(new Sprite(s));
            //tree 2
            s.TextureRect = new IntRect(3 * 32, 12 * 32, 32, 32);
            tiles.Add(new Sprite(s));
            //tree 3
            s.TextureRect = new IntRect(2 * 32, 13 * 32, 32, 32);
            tiles.Add(new Sprite(s));
            //tree 4
            s.TextureRect = new IntRect(3 * 32, 13 * 32, 32, 32);
            tiles.Add(new Sprite(s));
            //14 - tree 5
            s.TextureRect = new IntRect(0 * 32, 12 * 32, 32, 32);
            tiles.Add(new Sprite(s));
            //tree 6
            s.TextureRect = new IntRect(1 * 32, 12 * 32, 32, 32);
            tiles.Add(new Sprite(s));
            //tree 7
            s.TextureRect = new IntRect(0 * 32, 13 * 32, 32, 32);
            tiles.Add(new Sprite(s));
            //tree 8
            s.TextureRect = new IntRect(1 * 32, 13 * 32, 32, 32);
            tiles.Add(new Sprite(s));
        }

        public int getTileByCoords(int x, int y) {
            return tileMap[y / 32, x / 32];
        }

        public Map(string path) {
            initTileSprites();
            loadMap(path);
        }

        public void tick() {

        }

        public void render(RenderWindow window, Vector2i gameCameraOffset) {

            for (int y = 0; y < Height; y++) {
                if (y % 2 == 0) {
                    treeBorder3.Position = new Vector2f(0.0f - gameCameraOffset.X, (y * 32.0f) - gameCameraOffset.Y);
                    window.Draw(treeBorder3);
                    treeBorder3.Position = new Vector2f(((Width - 1) * 32.0f) - gameCameraOffset.X, (y * 32.0f) - gameCameraOffset.Y);
                    window.Draw(treeBorder3);
                } else {
                    treeBorder4.Position = new Vector2f(0.0f - gameCameraOffset.X, (y * 32.0f) - gameCameraOffset.Y);
                    window.Draw(treeBorder4);
                    treeBorder4.Position = new Vector2f(((Width - 1) * 32.0f) - gameCameraOffset.X, (y * 32.0f) - gameCameraOffset.Y);
                    window.Draw(treeBorder4);
                }
            }

            for (int x = 0; x < Width; x++) {
                treeBorder3.Position = new Vector2f((x * 32.0f) - gameCameraOffset.X, 0.0f - gameCameraOffset.Y);
                window.Draw(treeBorder3);
                treeBorder4.Position = new Vector2f((x * 32.0f) - gameCameraOffset.X, ((Height - 1) * 32.0f) - gameCameraOffset.Y);
                window.Draw(treeBorder4);
            }

            for (int x = 1; x < Width - 1; x++) {
                treeBorder2.Position = new Vector2f((x * 32.0f) - gameCameraOffset.X, 32.0f - gameCameraOffset.Y);
                window.Draw(treeBorder2);
                treeBorder1.Position = new Vector2f((x * 32.0f) - gameCameraOffset.X, ((Height - 2) * 32.0f) - gameCameraOffset.Y);
                window.Draw(treeBorder1);
            }

            for (int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    if (tileMap[y, x] != 99 && (x * 32) - gameCameraOffset.X > -32 && (x * 32) - gameCameraOffset.X < Game.displayWidth && (y * 32) - gameCameraOffset.Y > -32 && (y * 32) - gameCameraOffset.Y < Game.displayHeight) {
                        tiles[tileMap[y, x]].Position = new Vector2f((x * 32) - gameCameraOffset.X, (y * 32) - gameCameraOffset.Y);
                        window.Draw(tiles[tileMap[y, x]]);
                    }
                }
            }
        }
    }
}