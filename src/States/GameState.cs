using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;
using System.IO;
using System;

namespace TAC {

    class GameState : State {
        
        private Map map1;
        public static Map currentMap;
        public Player player {get; set;}
        public List<Entity> Entities {get; set; }
        public Vector2i gameCameraOffset; 
        private bool paused;
        private bool pauseKeyPressed;

        private Sprite pauseMenuBG;
        private Button resume;
        private Button save;
        private Button settings;
        private Button quit;
        

        public GameState() : base() {
            map1 = new Map("res/maps/test.map");
            currentMap = map1;
            player = new Player(200.0f, 200.0f);
            Entities = new List<Entity>();
            Entities.Add(player);
            Entities.Add(new HostileMob(200.0f, 200.0f));
            gameCameraOffset = new Vector2i(0, 0);

            player.X = 500.0f;
            player.Y = 500.0f;

            paused = false;
            pauseKeyPressed = false;

            pauseMenuBG = new Sprite(Assets.ui, new IntRect(434, 290, 128, 128));
            pauseMenuBG.Scale = new Vector2f(2.0f, 2.0f);
            pauseMenuBG.Position = new Vector2f((Game.displayWidth / 2) - (pauseMenuBG.TextureRect.Width), (Game.displayHeight / 2) - (pauseMenuBG.TextureRect.Height));

            resume =    new Button("Resume",    new Vector2f(pauseMenuBG.Position.X + 56.0f, pauseMenuBG.Position.Y + 16.0f), 2.0f);
            save =      new Button("Save",      new Vector2f(pauseMenuBG.Position.X + 56.0f, pauseMenuBG.Position.Y + 75.0f), 2.0f);
            settings =  new Button("Settings",  new Vector2f(pauseMenuBG.Position.X + 56.0f, pauseMenuBG.Position.Y + 133.0f), 2.0f);
            quit =      new Button("Quit",      new Vector2f(pauseMenuBG.Position.X + 56.0f, pauseMenuBG.Position.Y + 192.0f), 2.0f);

            resume.onClick +=   (sender, e) => { paused = false; };
            save.onClick +=     (sender, e) => { saveGame(); };
            settings.onClick += (sender, e) => { Handler.game.showSettings(); };
            quit.onClick +=     (sender, e) => {
                paused = false;
                Handler.game.popState();
            };
        }

        public void saveGame() {
            StreamWriter sw = new StreamWriter("saves/test.tac");
            sw.WriteLine("[MAP]");
            sw.WriteLine("test");
            sw.WriteLine("[PLAYER]");
            sw.WriteLine((double)player.X);
            sw.WriteLine((double)player.Y);
            sw.WriteLine(player.Health);
            sw.WriteLine(player.MaxHealth);
            sw.WriteLine((double)player.Stamina);
            sw.WriteLine((double)player.MaxStamina);
            foreach (Entity e in Entities) {
                if (e != player) {
                    sw.WriteLine("[ENTITY]");
                    sw.WriteLine((double)e.X);
                    sw.WriteLine((double)e.Y);
                    sw.WriteLine(e.Health);
                    sw.WriteLine(e.MaxHealth);
                }
            }

            sw.Close();
        }

        public void loadGame() {
            List<string> lines = new List<string>();
            if (!File.Exists("saves/test.tac")) {
                Console.Error.WriteLine("Could not open save file.");
                return;
            }

            lines = new List<string>(File.ReadAllLines("saves/test.tac"));
            
            try {
                for (int i = 0; i < lines.Count; i++) {
                    if (lines[i] == "[MAP]") {
                        i++;
                        map1 = new Map("res/maps/" + lines[i] + ".map");
                    } else if (lines[i] == "[PLAYER]") {
                        player.X = float.Parse(lines[++i]);
                        player.Y = float.Parse(lines[++i]);
                        player.Health = int.Parse(lines[++i]);
                        player.MaxHealth = int.Parse(lines[++i]);
                        player.Stamina = float.Parse(lines[++i]);
                        player.MaxStamina = float.Parse(lines[++i]);
                    } else if (lines[i] == "[ENTITY]") {
                        HostileMob hm = new HostileMob(float.Parse(lines[++i]), float.Parse(lines[++i]));
                        hm.Health = int.Parse(lines[++i]);
                        hm.MaxHealth = int.Parse(lines[++i]);
                        Entities.Add(hm);
                    }
                }
            } catch (Exception e) {
                Console.WriteLine("Save file format is incorrect.");
                Console.WriteLine(e.Message);
            }
        }

        public override void tick() {
            
            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape) && !pauseKeyPressed) {
                pauseKeyPressed = true;
                paused = !paused;
            }

            if (!Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                pauseKeyPressed = false;

            if (paused) {
                resume.tick();
                save.tick();
                settings.tick();
                quit.tick();
                return;
            }

            if (player.Health <= 0)
                //end game

            map1.tick();

            gameCameraOffset.X = (int)(player.X + 16 - (Game.displayWidth / 2));
            gameCameraOffset.Y = (int)(player.Y + 16 - (Game.displayHeight / 2));
            if (gameCameraOffset.X < 0) gameCameraOffset.X = 0;
            if (gameCameraOffset.Y < 0) gameCameraOffset.Y = 0;
            if (gameCameraOffset.X > (map1.Width * 32) - Game.displayWidth) gameCameraOffset.X = (int)((map1.Width * 32) - Game.displayWidth);
            if (gameCameraOffset.Y > (map1.Height * 32) - Game.displayHeight + 128) gameCameraOffset.Y = (int)((map1.Height * 32) - Game.displayHeight + 128);

            Entities.Sort(Comparer<Entity>.Create((e1, e2) => e1.Y.CompareTo(e2.Y)));

            List<Entity> entitiesToRemove = new List<Entity>();

            foreach (Entity e in Entities) {
                if (e.Health <= 0) {
                    entitiesToRemove.Add(e);
                    continue;
                }
                e.tick();
            }

            foreach (Entity e in entitiesToRemove) {
                Entities.Remove(e);
            }
        }

        public override void render(RenderWindow window) {
            map1.render(window, gameCameraOffset);
            
            foreach (Entity e in Entities) {
                e.render(window);
            }

            if (paused) {
                pauseMenuBG.Position = new Vector2f((Game.displayWidth / 2) - (pauseMenuBG.TextureRect.Width), (Game.displayHeight / 2) - (pauseMenuBG.TextureRect.Height));
                window.Draw(pauseMenuBG);

                resume.Position     = new Vector2f(pauseMenuBG.Position.X + 56.0f, pauseMenuBG.Position.Y + 16.0f);
                save.Position       = new Vector2f(pauseMenuBG.Position.X + 56.0f, pauseMenuBG.Position.Y + 75.0f);
                settings.Position   = new Vector2f(pauseMenuBG.Position.X + 56.0f, pauseMenuBG.Position.Y + 133.0f);
                quit.Position       = new Vector2f(pauseMenuBG.Position.X + 56.0f, pauseMenuBG.Position.Y + 192.0f);
                resume.render(window);
                save.render(window);
                settings.render(window);
                quit.render(window);
            }
        }
    }
}