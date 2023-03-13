using System.IO;
using System.Collections.Generic;
using SFML.System;
using SFML.Graphics;
using System;

namespace TAC {

    class Transition {
        public int ID {get; set;}
        public string MapName {get; set;}
        public int X {get; set;}
        public int Y {get; set;}
        public int TransitionX {get; set;}
        public int TransitionY {get; set;}

        public Transition(int id, string mapName, int x, int y, int transitionX, int transitionY) {
            ID = id;
            MapName = mapName;
            X = x;
            Y = y;
            TransitionX = transitionX;
            TransitionY = transitionY;
        }
    }

    class Map {

        public string MapName {get; set;}
        public int Width {get; set;}
        public int Height {get; set;}

        public List<Sprite> tiles = new List<Sprite>();
        public List<int> SolidTiles {get; set;}
        public List<Transition> Transitions {get; set;} = new List<Transition>();
        public List<Entity> Entities {get; set;}
        public List<GroundItem> Items {get; set;}

        public Sprite treeBorder1, treeBorder2, treeBorder3, treeBorder4;
        public Sprite transitionBorder1, transitionBorder2, transitionBorder3, transitionBorder4;

        public int[,] tileMap;

        public void initTileSprites() {
            Sprite s = new Sprite(Assets.terrain);
            //0 - map transfer tile
            s.TextureRect = new IntRect(1 * 32, 2 * 32, 32, 32);
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
            return tileMap[(int)(x / 32), (int)(y / 32)];
        }

        public Map(string name) {
            initTileSprites();
            
            Entities = new List<Entity>();
            Items = new List<GroundItem>();
            
            MapName = name;
            List<string> lines = new List<string>(File.ReadAllLines("./res/maps/" + MapName + ".map"));
            int startToken = 0;
            do {
                string[] lineTokens = lines[0].Split(' ');
                startToken = int.Parse(lineTokens[0]);
                if (startToken >= 30 && startToken <= 39) {
                    Transitions.Add(new Transition(int.Parse(lineTokens[0]), lineTokens[1], int.Parse(lineTokens[2]), int.Parse(lineTokens[3]), int.Parse(lineTokens[4]), int.Parse(lineTokens[5])));
                    lines.RemoveAt(0);
                }
            } while (startToken >= 30 && startToken <= 39);

            Width = lines[0].Split(' ').Length;
            Height = lines.Count;

            tileMap = new int[Width, Height];

            SolidTiles = new List<int>{99};

            treeBorder1 = new Sprite(Assets.terrain, new IntRect(608, 448, 32, 32));
            treeBorder2 = new Sprite(Assets.terrain, new IntRect(608, 480, 32, 32));
            treeBorder3 = new Sprite(Assets.terrain, new IntRect(640, 448, 32, 32));
            treeBorder4 = new Sprite(Assets.terrain, new IntRect(640, 480, 32, 32));

            transitionBorder1 = new Sprite(Assets.terrain, new IntRect(672, 512, 32, 32));
            transitionBorder2 = new Sprite(Assets.terrain, new IntRect(704, 512, 32, 32));
            transitionBorder3 = new Sprite(Assets.terrain, new IntRect(672, 544, 32, 32));
            transitionBorder4 = new Sprite(Assets.terrain, new IntRect(704, 544, 32, 32));

            for (int x = 0; x < Width; x++) {
                for (int y = 0; y < Height; y++) {
                    string[] tokens = lines[y].Split(' ');
                    tileMap[x, y] = int.Parse(tokens[x]);
                }
            }
        }

        public void tick() {

        }

        public void render(RenderWindow window, Vector2f gameCameraOffset) {
            for (int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    if (tileMap[x, y] != 99)
                        continue;

                    if (y > 0 && tileMap[x, y - 1] != 99) {
                        treeBorder1.Position = new Vector2f((x * 32.0f) - gameCameraOffset.X, (y * 32.0f) - gameCameraOffset.Y);
                        window.Draw(treeBorder1);
                        continue;
                    }
                    
                    if (y < Height - 1 && tileMap[x, y + 1] != 99) {
                        treeBorder2.Position = new Vector2f((x * 32.0f) - gameCameraOffset.X, (y * 32.0f) - gameCameraOffset.Y);
                        window.Draw(treeBorder2);
                        continue;
                    }

                    if (y % 2 == 0) {
                        treeBorder3.Position = new Vector2f((x * 32.0f) - gameCameraOffset.X, (y * 32.0f) - gameCameraOffset.Y);
                        window.Draw(treeBorder3);
                        continue;
                    }
                    
                    treeBorder4.Position = new Vector2f((x * 32.0f) - gameCameraOffset.X, (y * 32.0f) - gameCameraOffset.Y);
                    window.Draw(treeBorder4);
                }
            }

            for (int x = 0; x < Width; x++) {
                for (int y = 0; y < Height; y++) {
                    if (tileMap[x, y] != 99 && ((tileMap[x, y] < 30 || tileMap[x, y] > 39)) && (x * 32) - gameCameraOffset.X > -32 && (x * 32) - gameCameraOffset.X < Game.displayWidth && (y * 32) - gameCameraOffset.Y > -32 && (y * 32) - gameCameraOffset.Y < Game.displayHeight) {
                        tiles[tileMap[x, y]].Position = new Vector2f((x * 32) - gameCameraOffset.X, (y * 32) - gameCameraOffset.Y);
                        window.Draw(tiles[tileMap[x, y]]);
                        continue;
                    }
                    if (tileMap[x, y] >= 30 && tileMap[x, y] <= 39) {
                        //if not in tilemap, draw exit
                        tiles[0].Position = new Vector2f((x * 32) - gameCameraOffset.X, (y * 32) - gameCameraOffset.Y);
                        window.Draw(tiles[0]);
                        if (y == 0) { //up exit
                            transitionBorder1.Position = new Vector2f((x * 32) - gameCameraOffset.X, (y * 32) - gameCameraOffset.Y);
                            window.Draw(transitionBorder1);
                        }
                        if (x == tileMap.GetLength(0) - 1) { //right exit
                            transitionBorder2.Position = new Vector2f((x * 32) - gameCameraOffset.X, (y * 32) - gameCameraOffset.Y);
                            window.Draw(transitionBorder2);
                        }
                        if (y == tileMap.GetLength(1) - 1) { //down exit
                            transitionBorder3.Position = new Vector2f((x * 32) - gameCameraOffset.X, (y * 32) - gameCameraOffset.Y);
                            window.Draw(transitionBorder3);
                        }
                        if (x == 0) { //left exit
                            transitionBorder4.Position = new Vector2f((x * 32) - gameCameraOffset.X, (y * 32) - gameCameraOffset.Y);
                            window.Draw(transitionBorder4);
                        }
                    }
                }
            }
        }
    }
}