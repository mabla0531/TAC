using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using System;

namespace TAC {

    class GameState : State {
        public string SaveName {get; set;}
        public Player player {get; set;}
        public List<Map> Maps {get; set;}
        public Map CurrentMap {get; set;}
        public List<DamageNumber> DamageNumbers {get; set;}
        public Vector2f gameCameraOffset;
        public float TimeofDay {get; set;}
        private Clock timeClock;
        private Clock testClock;
        public RectangleShape LightLevel {get; set;}
        public bool Paused {get; set;}
        public bool StorageHovering {get; set;} = false;
        public StorageInventoryInterface StorageInventory {get; set;}
        public PlayerInventoryInterface PlayerInventory {get; set;}
        private DialogBox dialogBox;
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

            LightLevel = new RectangleShape(new Vector2f(Game.displayWidth, Game.displayHeight));
            LightLevel.Position = new Vector2f(0.0f, 0.0f);
            LightLevel.FillColor = new Color(0, 0, 0, 0);

            TimeofDay = 0.0f;
            timeClock = new Clock();
            testClock = new Clock();

            Paused = false;

            PlayerInventory = new PlayerInventoryInterface(player);
            StorageInventory = new StorageInventoryInterface(player, null);
            dialogBox = new DialogBox();

            pauseMenuBG = new RectangleShape(new Vector2f(276.0f, 256.0f));
            pauseMenuBG.Position = new Vector2f((Game.displayWidth / 2) - (pauseMenuBG.Size.X / 2), (Game.displayHeight / 2) - (pauseMenuBG.Size.Y));
            pauseMenuBG.FillColor = new Color(36, 58, 71, 230);
            gb = new GaussianBlur((int)pauseMenuBG.Size.X, (int)pauseMenuBG.Size.Y);

            resume   = new Button("Resume",    new Vector2f(pauseMenuBG.Position.X + (pauseMenuBG.Size.X / 2) - 64.0f, pauseMenuBG.Position.Y + 16.0f));
            save     = new Button("Save",     new Vector2f(pauseMenuBG.Position.X + (pauseMenuBG.Size.X / 2) - 64.0f, pauseMenuBG.Position.Y + 75.0f));
            settings = new Button("Settings", new Vector2f(pauseMenuBG.Position.X + (pauseMenuBG.Size.X / 2) - 64.0f, pauseMenuBG.Position.Y + 133.0f));
            quit     = new Button("Quit",     new Vector2f(pauseMenuBG.Position.X + (pauseMenuBG.Size.X / 2) - 64.0f, pauseMenuBG.Position.Y + 192.0f));

            resume.onClick   += (sender, e) => { Paused = false; };
            save.onClick     += (sender, e) =>{ saveGame(); };
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

            if (TextInputHandler.Characters.Contains(27))
                Paused = !Paused;

            if (Paused) {
                //tick buttons
                resume.tick();
                save.tick();
                settings.tick();
                quit.tick();
                return;
            }

            //if (player.Health <= 0)
                //end game
            
            dialogBox.tick();
            PlayerInventory.tick();
            StorageInventory.tick();

            if (dialogBox.Active || PlayerInventory.Active || StorageInventory.Active) return;

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

            CurrentMap.tick();

            gameCameraOffset.X = (int)(player.X + 16 - (Game.displayWidth / 2));
            gameCameraOffset.Y = (int)(player.Y + 16 - (Game.displayHeight / 2));
            if (gameCameraOffset.X > (CurrentMap.Width * 32) - Game.displayWidth) gameCameraOffset.X = (int)((CurrentMap.Width * 32) - Game.displayWidth);
            if (gameCameraOffset.Y > (CurrentMap.Height * 32) - Game.displayHeight) gameCameraOffset.Y = (int)((CurrentMap.Height * 32) - Game.displayHeight);
            if (gameCameraOffset.X < 0) gameCameraOffset.X = 0;
            if (gameCameraOffset.Y < 0) gameCameraOffset.Y = 0;
            if ((CurrentMap.Width * 32.0f) < Game.displayWidth) gameCameraOffset.X = ((CurrentMap.Width * 32.0f) / 2) - (Game.displayWidth / 2);
            if ((CurrentMap.Height * 32.0f) < Game.displayHeight) gameCameraOffset.Y = ((CurrentMap.Height * 32.0f) / 2) - (Game.displayHeight / 2);

            //Do sun cycle, days last ~20 minutes
            if (timeClock.ElapsedTime.AsMilliseconds() >= 1000) {
                TimeofDay += 0.00125f;
                timeClock.Restart();
            }

            if (TimeofDay >= 1.5f)
                TimeofDay = -1.5f;

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

                //Interaction
                float currentEntityDeltaX = CurrentMap.Entities[i].X - player.X;
                float currentEntityDeltaY = CurrentMap.Entities[i].Y - player.Y;
                if (CurrentMap.Entities[i].Hovered && CurrentMap.Entities[i] is StorageEntity && CurrentMap.Entities[i].inventory.Items.Count > 0 && Math.Sqrt((currentEntityDeltaX * currentEntityDeltaX) + (currentEntityDeltaY * currentEntityDeltaY)) < player.InteractionRange)
                    StorageHovering = true;

                if (CurrentMap.Entities[i] == player)
                    continue;

                if (StorageHovering && !StorageInventory.Active && MouseHandler.RightPressed && Math.Sqrt((currentEntityDeltaX * currentEntityDeltaX) + (currentEntityDeltaY * currentEntityDeltaY)) < player.InteractionRange) {
                    StorageInventory.Active = true;
                    Assets.inventory.Play();
                    StorageInventory.Position = new Vector2f(MouseHandler.MouseX, MouseHandler.MouseY);
                    StorageInventory.entity = (StorageEntity)CurrentMap.Entities[i];
                }

                if (CurrentMap.Entities[i].Hovered && CurrentMap.Entities[i] is FriendlyMob && !dialogBox.Active && MouseHandler.RightPressed && Math.Sqrt((currentEntityDeltaX * currentEntityDeltaX) + (currentEntityDeltaY * currentEntityDeltaY)) < player.InteractionRange) {
                    dialogBox.Active = true;
                }
            }

            if (TextInputHandler.Characters.Contains('e') || TextInputHandler.Characters.Contains('E')) {
                PlayerInventory.Active = !PlayerInventory.Active;
                Assets.inventory.Play();
            }

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

            byte lightLevel = (byte)Math.Abs(192.0f * TimeofDay);
            byte sunset = (byte)Math.Abs(255 * Math.Abs(Math.Abs(Math.Abs(TimeofDay) - 0.5f) - 0.5f));
            //0.0, 0
            //0.5, 255
            //1.0, 0
            if (Math.Abs(TimeofDay) > 1.0f) {
                lightLevel = 192;
                sunset = 0;
            }

            LightLevel.FillColor = new Color(sunset, 0, 0, lightLevel);
            window.Draw(LightLevel);

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

            hud.render(window);

            if (StorageHovering) {
                storageIcon.Position = new Vector2f(MouseHandler.MouseX + 16.0f, MouseHandler.MouseY + 16.0f);
                window.Draw(storageIcon);
            }

            PlayerInventory.render(window);
            StorageInventory.render(window);

            if (dialogBox.Active)
                dialogBox.render(window);

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
        }
    }
}