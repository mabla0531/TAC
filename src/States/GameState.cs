using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;
using System;

namespace TAC {

    class GameState : State {
        
        public Player player {get; set;}
        public List<Map> Maps {get; set;}
        public Map CurrentMap {get; set;}
        public List<DamageNumber> DamageNumbers {get; set;}
        public Vector2f gameCameraOffset;
        public bool Paused {get; set;}
        private bool pauseKeyPressed;
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


        public GameState(Game game) : base() {
            player = new Player(100.0f, 100.0f);

            Maps = new List<Map>();

            DamageNumbers = new List<DamageNumber>();

            gameCameraOffset = new Vector2f(0, 0);

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
            settings.onClick += (sender, e) => { game.showSettings(); };
            quit.onClick     += (sender, e) => {
                Paused = false;
                game.popState();
            };

            hud = new HUD(player);

            storageIcon = new Sprite(Assets.terrain, new IntRect(608, 640, 16, 16));
        }

        public void saveGame() {
            XMLHandler.writeSaveFile(this);
        }

        public void loadGame() {
            XMLHandler.readSaveFile(this);

            if (Maps.Count > 0)
                CurrentMap = Maps[0];
            CurrentMap.Entities.Add(player);
        }

        public override void tick() {
            //handle all transition tile
            foreach(Transition transition in CurrentMap.Transitions) {
                if (!player.getCollisionBounds().Intersects(new FloatRect(transition.X * 32.0f, transition.Y * 32.0f, 32.0f, 32.0f)))
                    continue;

                player.X = transition.TransitionX * 32.0f;
                player.Y = transition.TransitionY * 32.0f;
                foreach(Map map in Maps) {
                    if (map.MapName == transition.MapName) {
                        Console.WriteLine("Setting map to " + map.MapName);
                        CurrentMap.Entities.Remove(player);
                        CurrentMap = map;    
                        CurrentMap.Entities.Add(player);
                    }
                }
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape) && !pauseKeyPressed) {
                pauseKeyPressed = true;
                Paused = !Paused;
            }

            if (!Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                pauseKeyPressed = false;

            if (Paused) {
                //tick buttons
                resume.tick();
                save.tick();
                settings.tick();
                quit.tick();
                return;
            }

            if (player.Health <= 0)
                //end game

            CurrentMap.tick();

            gameCameraOffset.X = (int)(player.X + 16 - (Game.displayWidth / 2));
            gameCameraOffset.Y = (int)(player.Y + 16 - (Game.displayHeight / 2));
            if (gameCameraOffset.X > (CurrentMap.Width * 32) - Game.displayWidth) gameCameraOffset.X = (int)((CurrentMap.Width * 32) - Game.displayWidth);
            if (gameCameraOffset.Y > (CurrentMap.Height * 32) - Game.displayHeight) gameCameraOffset.Y = (int)((CurrentMap.Height * 32) - Game.displayHeight);
            if (gameCameraOffset.X < 0) gameCameraOffset.X = 0;
            if (gameCameraOffset.Y < 0) gameCameraOffset.Y = 0;
            if ((CurrentMap.Width * 32.0f) < Game.displayWidth) gameCameraOffset.X = ((CurrentMap.Width * 32.0f) / 2) - (Game.displayWidth / 2);
            if ((CurrentMap.Height * 32.0f) < Game.displayHeight) gameCameraOffset.Y = ((CurrentMap.Height * 32.0f) / 2) - (Game.displayHeight / 2);

            //Sort entities by Y value, to give 3D effect
            CurrentMap.Entities.Sort(Comparer<Entity>.Create((e1, e2) => e1.Y.CompareTo(e2.Y)));

            //Tick all entities, then spawn corpse if needed
            StorageHovering = false;

            for (int i = 0; i < CurrentMap.Entities.Count; i++) {

                if (CurrentMap.Entities[i].Health <= 0 && CurrentMap.Entities[i].IsKillable) {
                    Corpse c = new Corpse(CurrentMap.Entities[i].X, CurrentMap.Entities[i].Y);
                    c.inventory = CurrentMap.Entities[i].inventory;
                    CurrentMap.Entities[i] = c;
                }

                CurrentMap.Entities[i].tick();

                float currentEntityDeltaX = CurrentMap.Entities[i].X - player.X;
                float currentEntityDeltaY = CurrentMap.Entities[i].Y - player.Y;
                if (CurrentMap.Entities[i].Hovered && CurrentMap.Entities[i] is StorageEntity && CurrentMap.Entities[i].inventory.Items.Count > 0 && Math.Sqrt((currentEntityDeltaX * currentEntityDeltaX) + (currentEntityDeltaY * currentEntityDeltaY)) < player.InteractionRange)
                    StorageHovering = true;

                //Interaction
                if (CurrentMap.Entities[i] == player)
                    continue;

                if (StorageHovering && !StorageInventoryActive && MouseHandler.RightPressed && Math.Sqrt((currentEntityDeltaX * currentEntityDeltaX) + (currentEntityDeltaY * currentEntityDeltaY)) < player.InteractionRange) {
                    StorageInventoryActive = true;
                    storageInventory.Position = new Vector2f(MouseHandler.MouseX, MouseHandler.MouseY);
                    storageInventory.entity = (StorageEntity)CurrentMap.Entities[i];
                }
            }

            if (!new FloatRect(storageInventory.Position, storageInventory.Size).Contains(MouseHandler.MouseX, MouseHandler.MouseY) && StorageInventoryActive && (MouseHandler.RightButton || MouseHandler.LeftButton))
                StorageInventoryActive = false;

            //Tick ground based items
            GroundItem itemToPickUp = null;
            foreach(GroundItem g in CurrentMap.Items) {
                if (MouseHandler.RightPressed && g.Hovered) {
                    Handler.gameState.player.inventory.Items.Add(g.ItemReference);
                    itemToPickUp = g;
                    continue;
                }
                g.tick();
            }

            if (itemToPickUp != null)
                CurrentMap.Items.Remove(itemToPickUp); //Time travel would have been invented if we could remove a list member during iteration

            if (StorageInventoryActive)
                    storageInventory.tick();
        }

        public override void render(RenderWindow window) {
            CurrentMap.render(window, gameCameraOffset);
            
            //Render ground based items first, so they're under everything
            foreach(GroundItem g in CurrentMap.Items) {
                g.render(window);
            }

            foreach (Entity e in CurrentMap.Entities) {
                e.render(window);
            }

            foreach (GroundItem g in CurrentMap.Items) {
                g.renderTooltip(window); //Put tooltips above everything else so character doesn't walk over it
            }

            List<DamageNumber> damageNumbersToRemove = new List<DamageNumber>();
            foreach (DamageNumber damageNumber in DamageNumbers) {
                if (damageNumber.Finished) {
                    damageNumbersToRemove.Add(damageNumber);
                    continue;
                }

                damageNumber.render(window);
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
                //                                      Because of text rendering system, if X/Y are decimal point numbers it causes text to be gross and blurry
                storageInventory.Position = new Vector2f((int)(storageInventory.entity.X - gameCameraOffset.X), (int)(storageInventory.entity.Y - gameCameraOffset.Y));
                storageInventory.render(window);
            }
        }
    }
}