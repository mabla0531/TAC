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
        public List<Entity> Entities {get; set;}
        public List<GroundItem> Items {get; set;}
        public Vector2f gameCameraOffset;
        public bool Paused {get; set;}
        private bool pauseKeyPressed;
        private GroundItem itemToPickUp = null;
        public bool StorageHovering {get; set;} = false;
        private StorageInventory storageInventory;
        public bool StorageInventoryActive {get; set;} = false;

        private RectangleShape pauseMenuBG;
        private GaussianBlur gb;
        private Button resume;
        private Button save;
        private Button settings;
        private Button quit;

        private HUD hud;

        private Sprite storageIcon;


        public GameState() : base() {
            map1 = new Map("res/maps/test.map");
            currentMap = map1;
            player = new Player(100.0f, 100.0f);
            Entities = new List<Entity>();
            Entities.Add(player);
            HostileMob mob1 = new HostileMob(64.0f, 64.0f);
            mob1.inventory.Items.Add(Item.sword);
            mob1.inventory.Items.Add(Item.shovel);
            Entities.Add(mob1);

            Items = new List<GroundItem>();
            gameCameraOffset = new Vector2f(0, 0);

            player.X = 500.0f;
            player.Y = 500.0f;

            Paused = false;
            pauseKeyPressed = false;

            storageInventory = new StorageInventory(player, null);

            pauseMenuBG = new RectangleShape(new Vector2f(276.0f, 256.0f));
            pauseMenuBG.Position = new Vector2f((Game.displayWidth / 2) - (pauseMenuBG.Size.X / 2), (Game.displayHeight / 2) - (pauseMenuBG.Size.Y));
            pauseMenuBG.FillColor = new Color(36, 58, 71, 230);
            gb = new GaussianBlur((int)pauseMenuBG.Size.X, (int)pauseMenuBG.Size.Y);

            resume   = new Button("Resume",   new Vector2f(pauseMenuBG.Position.X + (pauseMenuBG.Size.X / 2) - 64.0f, pauseMenuBG.Position.Y + 16.0f));
            save     = new Button("Save",     new Vector2f(pauseMenuBG.Position.X + (pauseMenuBG.Size.X / 2) - 64.0f, pauseMenuBG.Position.Y + 75.0f));
            settings = new Button("Settings", new Vector2f(pauseMenuBG.Position.X + (pauseMenuBG.Size.X / 2) - 64.0f, pauseMenuBG.Position.Y + 133.0f));
            quit     = new Button("Quit",     new Vector2f(pauseMenuBG.Position.X + (pauseMenuBG.Size.X / 2) - 64.0f, pauseMenuBG.Position.Y + 192.0f));

            resume.onClick   += (sender, e) => { Paused = false; };
            save.onClick     += (sender, e) => { saveGame(); };
            settings.onClick += (sender, e) => { Handler.game.showSettings(); };
            quit.onClick     += (sender, e) => {
                Paused = false;
                Handler.game.popState();
            };

            hud = new HUD(player);

            storageIcon = new Sprite(Assets.terrain, new IntRect(608, 640, 16, 16));
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
            if (gameCameraOffset.Y > (map1.Height * 32) - Game.displayHeight) gameCameraOffset.Y = (int)((map1.Height * 32) - Game.displayHeight);

            //Sort entities by Y value, to give 3D effect
            Entities.Sort(Comparer<Entity>.Create((e1, e2) => e1.Y.CompareTo(e2.Y)));

            //Tick all entities, then spawn corpse if needed
            StorageHovering = false;
            for (int i = 0; i < Entities.Count; i++) {

                if (Entities[i].Health <= 0 && Entities[i].IsKillable) {
                    Corpse c = new Corpse(new Vector2f(Entities[i].X, Entities[i].Y));
                    c.inventory = Entities[i].inventory;
                    Entities[i] = c;
                }
                Entities[i].tick();

                if (Entities[i].Hovered && Entities[i] is StorageEntity)
                    StorageHovering = true;

                if (Entities[i] == player)
                    continue;

                //Interaction

                float currentEntityDeltaX = Entities[i].X - player.X;
                float currentEntityDeltaY = Entities[i].Y - player.Y;

                if (StorageHovering && !StorageInventoryActive && MouseHandler.RightPressed && Math.Sqrt((currentEntityDeltaX * currentEntityDeltaX) + (currentEntityDeltaY * currentEntityDeltaY)) < player.InteractionRange) {
                    StorageInventoryActive = true;
                    storageInventory.Position = new Vector2f(MouseHandler.MouseX, MouseHandler.MouseY);
                    storageInventory.entity = (StorageEntity)Entities[i];
                }
            }

            if (!new FloatRect(storageInventory.Position, storageInventory.Size).Contains(MouseHandler.MouseX, MouseHandler.MouseY) && StorageInventoryActive && (MouseHandler.RightButton || MouseHandler.LeftButton))
                StorageInventoryActive = false;

            //Tick ground based items
            itemToPickUp = null;
            foreach(GroundItem g in Items) {
                if (MouseHandler.RightPressed && g.Hovered) {
                    Handler.gameState.player.inventory.Items.Add(g.ItemReference);
                    itemToPickUp = g;
                    continue;
                }
                g.tick();
            }

            if (itemToPickUp != null)
                Items.Remove(itemToPickUp); //Time travel would have been invented if we could remove a list member during iteration

            if (StorageInventoryActive)
                    storageInventory.tick();
        }

        public override void render(RenderWindow window) {
            map1.render(window, gameCameraOffset);
            
            //Render ground based items first, so they're under everything
            foreach(GroundItem g in Items) {
                g.render(window);
            }

            foreach (Entity e in Entities) {
                e.render(window);
            }

            foreach (GroundItem g in Items) {
                g.renderTooltip(window); //Put tooltips above everything else so character doesn't walk over it
            }

            if (Paused) {
                pauseMenuBG.Position = new Vector2f((Game.displayWidth / 2) - (pauseMenuBG.Size.X / 2), (Game.displayHeight / 2) - (pauseMenuBG.Size.Y));
                gb.blurArea((int)pauseMenuBG.Position.X, (int)pauseMenuBG.Position.Y, window);
                window.Draw(pauseMenuBG);

                resume.Position     = new Vector2f(pauseMenuBG.Position.X + (pauseMenuBG.Size.X / 2) - 64.0f, pauseMenuBG.Position.Y + 16.0f);
                save.Position       = new Vector2f(pauseMenuBG.Position.X + (pauseMenuBG.Size.X / 2) - 64.0f, pauseMenuBG.Position.Y + 75.0f);
                settings.Position   = new Vector2f(pauseMenuBG.Position.X + (pauseMenuBG.Size.X / 2) - 64.0f, pauseMenuBG.Position.Y + 133.0f);
                quit.Position       = new Vector2f(pauseMenuBG.Position.X + (pauseMenuBG.Size.X / 2) - 64.0f, pauseMenuBG.Position.Y + 192.0f);
                resume.render(window);
                save.render(window);
                settings.render(window);
                quit.render(window);
            }

            hud.render(window);

            if (StorageHovering) {
                storageIcon.Position = new Vector2f(MouseHandler.MouseX + 16.0f, MouseHandler.MouseY + 16.0f);
                window.Draw(storageIcon);
            }

            if (StorageInventoryActive) {
                //                                      Because of text rendering system, if X/Y are decimal point numbers text is gross and blurry
                storageInventory.Position = new Vector2f((int)(storageInventory.entity.X - gameCameraOffset.X), (int)(storageInventory.entity.Y - gameCameraOffset.Y));
                storageInventory.render(window);
            }
        }
    }
}