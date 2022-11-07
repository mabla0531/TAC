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
        public bool Paused {get; set;}
        private bool pauseKeyPressed;

        private Sprite pauseMenuBG;
        private Button resume;
        private Button save;
        private Button settings;
        private Button quit;
        
        //HUD objects
        private Sprite hudFrameCornerNW, hudFrameCornerNE, hudFrameCornerSW, hudFrameCornerSE, 
                       hudFrameEdgeN, hudFrameEdgeS, hudFrameEdgeW, hudFrameEdgeE, hudFrameMiddle;
        
        //equipment objects
        private Sprite handSlot;

        //healthbar objects
        private Sprite statBarL, statBar, statBarR;
        private Sprite healthFill;
        private Sprite staminaFill;
        

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

            Paused = false;
            pauseKeyPressed = false;

            pauseMenuBG = new Sprite(Assets.ui, new IntRect(434, 290, 128, 128));
            pauseMenuBG.Scale = new Vector2f(2.0f, 2.0f);
            pauseMenuBG.Position = new Vector2f((Game.displayWidth / 2) - (pauseMenuBG.TextureRect.Width), (Game.displayHeight / 2) - (pauseMenuBG.TextureRect.Height));

            resume =    new Button("Resume",    new Vector2f(pauseMenuBG.Position.X + 56.0f, pauseMenuBG.Position.Y + 16.0f), 2.0f);
            save =      new Button("Save",      new Vector2f(pauseMenuBG.Position.X + 56.0f, pauseMenuBG.Position.Y + 75.0f), 2.0f);
            settings =  new Button("Settings",  new Vector2f(pauseMenuBG.Position.X + 56.0f, pauseMenuBG.Position.Y + 133.0f), 2.0f);
            quit =      new Button("Quit",      new Vector2f(pauseMenuBG.Position.X + 56.0f, pauseMenuBG.Position.Y + 192.0f), 2.0f);

            resume.onClick +=   (sender, e) => { Paused = false; };
            save.onClick +=     (sender, e) => { saveGame(); };
            settings.onClick += (sender, e) => { Handler.game.showSettings(); };
            quit.onClick +=     (sender, e) => {
                Paused = false;
                Handler.game.popState();
            };

            initHUD();
        }

        public void initHUD() {
            hudFrameCornerNW = new Sprite(Assets.ui);
            hudFrameCornerNE = new Sprite(Assets.ui);
            hudFrameCornerSW = new Sprite(Assets.ui);
            hudFrameCornerSE = new Sprite(Assets.ui);
            hudFrameEdgeN    = new Sprite(Assets.ui);
            hudFrameEdgeS    = new Sprite(Assets.ui);
            hudFrameEdgeW    = new Sprite(Assets.ui);
            hudFrameEdgeE    = new Sprite(Assets.ui);
            hudFrameMiddle   = new Sprite(Assets.ui);

            statBarL     = new Sprite(Assets.ui);
            statBar      = new Sprite(Assets.ui);
            statBarR     = new Sprite(Assets.ui);
            healthFill   = new Sprite(Assets.ui);
            staminaFill  = new Sprite(Assets.ui);

            hudFrameCornerNW.TextureRect = new IntRect(16,  40, 32, 32);
            hudFrameCornerNE.TextureRect = new IntRect(82,  40, 32, 32);
            hudFrameCornerSW.TextureRect = new IntRect(478, 80, 32, 32);
            hudFrameCornerSE.TextureRect = new IntRect(544, 80, 32, 32);
            hudFrameEdgeN.TextureRect    = new IntRect(49,  40, 32, 32);
            hudFrameEdgeS.TextureRect    = new IntRect(511, 80, 32, 32);
            hudFrameEdgeW.TextureRect    = new IntRect(478, 24, 32, 32);
            hudFrameEdgeE.TextureRect    = new IntRect(544, 24, 32, 32);
            hudFrameMiddle.TextureRect   = new IntRect(511, 24, 32, 32);
            statBarL.TextureRect         = new IntRect(258, 38, 24, 24);
            statBar.TextureRect          = new IntRect(283, 38, 24, 24);
            statBarR.TextureRect         = new IntRect(308, 38, 24, 24);
            staminaFill.TextureRect      = new IntRect(349, 71, 8, 16);
            healthFill.TextureRect       = new IntRect(349, 39, 8, 16);

            hudFrameCornerNW.Position    = new Vector2f(0, Game.displayHeight - 128);
            hudFrameCornerNE.Position    = new Vector2f(Game.displayWidth - 32, Game.displayHeight - 128);
            hudFrameCornerSW.Position    = new Vector2f(0, Game.displayHeight - 32);
            hudFrameCornerSE.Position    = new Vector2f(Game.displayWidth - 32, Game.displayHeight - 32);
            hudFrameEdgeN.Position       = new Vector2f(32, Game.displayHeight - 128);
            hudFrameEdgeN.Scale          = new Vector2f((Game.displayWidth / 32) - 2, 1.0f);
            hudFrameEdgeS.Position       = new Vector2f(32, Game.displayHeight - 32);
            hudFrameEdgeS.Scale          = new Vector2f((Game.displayWidth / 32) - 2, 1.0f);
            hudFrameEdgeE.Position       = new Vector2f(Game.displayWidth - 32, Game.displayHeight - 96);
            hudFrameEdgeE.Scale          = new Vector2f(1.0f, 2.0f);
            hudFrameEdgeW.Position       = new Vector2f(0, Game.displayHeight - 96);
            hudFrameEdgeW.Scale          = new Vector2f(1.0f, 2.0f);
            hudFrameMiddle.Position      = new Vector2f(32, Game.displayHeight - 96);
            hudFrameMiddle.Scale         = new Vector2f((Game.displayWidth / 32) - 2, 2.0f);
            statBarL.Scale               = new Vector2f(1.5f, 1.5f);
            statBarR.Scale               = new Vector2f(1.5f, 1.5f);
            statBar.Scale                = new Vector2f(4.0f, 1.5f);
            healthFill.Scale             = new Vector2f(15.0f * (player.DisplayHealth / (float)player.MaxHealth), 1.5f);
            staminaFill.Scale            = new Vector2f(15.0f * (player.Stamina / (float)player.MaxStamina), 1.5f);
        }

        private void renderHUD(RenderWindow window) {
            window.Draw(hudFrameMiddle);
            window.Draw(hudFrameEdgeN);
            window.Draw(hudFrameEdgeS);
            window.Draw(hudFrameEdgeW);
            window.Draw(hudFrameEdgeE);
            window.Draw(hudFrameCornerNW);
            window.Draw(hudFrameCornerNE);
            window.Draw(hudFrameCornerSW);
            window.Draw(hudFrameCornerSE);

            //draw in position of health bar
            statBarL.Position   = new Vector2f(8,  Game.displayHeight - 120);
            statBar.Position    = new Vector2f(44, Game.displayHeight - 120);
            statBarR.Position   = new Vector2f(118, Game.displayHeight - 120);
            window.Draw(statBarL);
            window.Draw(statBar);
            window.Draw(statBarR);

            //draw in position of stamina bar
            statBarL.Position   = new Vector2f(8,  Game.displayHeight - 84);
            statBar.Position    = new Vector2f(44, Game.displayHeight - 84);
            statBarR.Position   = new Vector2f(118, Game.displayHeight - 84);
            window.Draw(statBarL);
            window.Draw(statBar);
            window.Draw(statBarR);

            if (player.DisplayHealth > player.Health) {
                player.DisplayHealth -= 0.01f;
                Console.WriteLine(player.DisplayHealth);
            }
            
            healthFill.Position = new Vector2f(21,  Game.displayHeight - 114);
            healthFill.Scale = new Vector2f(15.0f * (player.DisplayHealth / (float)player.MaxHealth), 1.5f);
            window.Draw(healthFill);
            staminaFill.Position = new Vector2f(21,  Game.displayHeight - 78);
            staminaFill.Scale = new Vector2f(15.0f * (player.Stamina / (float)player.MaxStamina), 1.5f);
            window.Draw(staminaFill);

            if (player.Hand != null) {
                handSlot = player.Hand.Icon;
                handSlot.Scale = new Vector2f(3.0f, 3.0f);
                handSlot.Position = new Vector2f(Game.displayWidth - 70,  Game.displayHeight - 84);
                window.Draw(handSlot);
            }
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
                Paused = !Paused;
            }

            if (!Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                pauseKeyPressed = false;

            if (Paused) {
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

            if (Paused) {
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

            renderHUD(window);
        }
    }
}