using System;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace TAC {

    class Player : ActiveEntity {
        private float walkSpeed = 1.0f, runSpeed = 2.0f;

        //running stuff
        public float Stamina {get; set;} = 200.0f;
        public float MaxStamina {get; set;} = 200.0f;
        private float staminaDecaySpeed = 0.5f, staminaRegenSpeed = 1.0f;
        private bool cooldown = false;

        //player addons
        private Sprite sword;

        private RectangleShape cooldownBarBackground, cooldownBar;

        //Inventory
        private InventoryInterface inventoryInterface;
        private bool inventoryKeyPressed = false;

        public Item Head {get; set;}
        public Item Chest {get; set;}
        public Item Legs {get; set;}
        public Item Feet {get; set;}
        public Item Offhand {get; set;}
        public Item Hand {get; set;}

        public Player (float x, float y) : base() {
            X = x;
            Y = y;

            collisionBounds = new FloatRect(6.0f, 12.0f, 20.0f, 18.0f);

            sprite = new Sprite(Assets.player);
            sprite.TextureRect = new IntRect(32, 0, 32, 32);

            sword = new Sprite(Assets.items);
            sword.TextureRect = new IntRect(16, 112, 16, 16);

            animationFrames = new IntRect[16] {new IntRect(0,  0,  32, 32),
                                               new IntRect(32, 0,  32, 32),
                                               new IntRect(64, 0,  32, 32),
                                               new IntRect(32, 0,  32, 32),
                                               new IntRect(0,  96, 32, 32),
                                               new IntRect(32, 96, 32, 32),
                                               new IntRect(64, 96, 32, 32),
                                               new IntRect(32, 96, 32, 32),
                                               new IntRect(0,  32, 32, 32),
                                               new IntRect(32, 32, 32, 32),
                                               new IntRect(64, 32, 32, 32),
                                               new IntRect(32, 32, 32, 32),
                                               new IntRect(0,  64, 32, 32),
                                               new IntRect(32, 64, 32, 32),
                                               new IntRect(64, 64, 32, 32),
                                               new IntRect(32, 64, 32, 32)};
            
            animFrameDelay = new Clock();

            MaxHealth = 10;
            Health = 10;
            DisplayHealth = (float)Health;

            attackAlpha = 1;

            cooldownBarBackground = new RectangleShape(new Vector2f(18, 4));
            cooldownBarBackground.FillColor = new Color(200, 200, 200);
            cooldownBar = new RectangleShape(new Vector2f(16, 2));
            cooldownBar.FillColor = new Color(32, 32, 32);

            inventoryInterface = new InventoryInterface(this);
            inventory.addItem(Item.sword);
            inventory.addItem(Item.axe);
            inventory.addItem(Item.pickaxe);
            inventory.addItem(Item.shovel);
        }

        public override void hurt(int damage) {
            Health -= damage;
        }

        public override void specificTick() {
            float mouseAngle;

            if (!inventoryKeyPressed && Keyboard.IsKeyPressed(Keyboard.Key.E)) {
                inventoryInterface.Active = !inventoryInterface.Active;
                inventoryKeyPressed = true;
            }

            if (!Keyboard.IsKeyPressed(Keyboard.Key.E))
                inventoryKeyPressed = false;

            float mouseDeltaX = MouseHandler.MouseX - (X - Handler.gameState.gameCameraOffset.X + 16);
            float mouseDeltaY = MouseHandler.MouseY - (Y - Handler.gameState.gameCameraOffset.Y + 16); //add 16 to get center

            mouseAngle = (float)Math.Atan2(mouseDeltaY, mouseDeltaX);
            sword.Rotation = (float)((mouseAngle * (180 / Math.PI)) - 45.0f);
            
            doKnockback();

            if (Keyboard.IsKeyPressed(Keyboard.Key.A)) {
                moveX -= curSpeed;
                direction = 2;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.D)) {
                moveX += curSpeed;
                direction = 3;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.W)) {
                moveY -= curSpeed;
                direction = 1;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.S)) {
                moveY += curSpeed;
                direction = 0;
            }

            if (moveX != 0.0f && moveY != 0.0f) { //diagonal movement
                moveX *= ((float)Math.Sqrt(2) / 2);
                moveY *= ((float)Math.Sqrt(2) / 2);
            }

            checkCollision();

            X += moveX;
            Y += moveY;

            curSpeed = walkSpeed;
            animFrameInterTime = 250;
            if (Stamina >= MaxStamina)
                cooldown = false;
            if (Stamina <= 0.0f)
                cooldown = true;
            if (Keyboard.IsKeyPressed(Keyboard.Key.LShift) && (moveX != 0.0f || moveY != 0.0f) && !cooldown) {
                curSpeed = runSpeed;
                animFrameInterTime = 150;
                Stamina -= staminaDecaySpeed;
            } else if (Stamina < MaxStamina) {
                Stamina += staminaRegenSpeed;
            }

            tickAnimation();

            float currentEntityDeltaX, currentEntityDeltaY;
            float currentEntityAngle;


            if (MouseHandler.LeftClick && attackCooldown.ElapsedTime.AsMilliseconds() >= attackInterval && !inventoryInterface.Active) {
                Assets.swish.Play();

                foreach (ActiveEntity e in Handler.gameState.Entities) {
                    currentEntityDeltaX = e.X - X;
                    currentEntityDeltaY = e.Y - Y;
                    currentEntityAngle = (float)Math.Atan2(currentEntityDeltaY, currentEntityDeltaX);

                    attackCooldown.Restart();
                    if (e != this && Math.Abs(mouseAngle - currentEntityAngle) < 0.2f && Math.Sqrt((currentEntityDeltaX * currentEntityDeltaX) + (currentEntityDeltaY * currentEntityDeltaY)) <= 64.0f) {
                        attack(e);
                        knockback(e, currentEntityAngle, 32.0f);
                        Assets.slice.Play();
                    }
                }
            }

            int elapsed = attackCooldown.ElapsedTime.AsMilliseconds();
            cooldownBar.Size = new Vector2f((elapsed >= attackInterval ? 16.0f : (16.0f * ((float)elapsed / (float)attackInterval))), 2.0f);
            
            cooldownBar.FillColor = new Color(32, 32, 32);
            if (elapsed >= attackInterval)
                cooldownBar.FillColor = new Color(200, 200, 200);

            inventoryInterface.tick();
        }

        public override void specificRender(RenderWindow window) {
            sword.Position = new Vector2f(X - Handler.gameState.gameCameraOffset.X + 16.0f, Y - Handler.gameState.gameCameraOffset.Y + 20.0f);
            window.Draw(sword);

            cooldownBarBackground.Position = new Vector2f(X - Handler.gameState.gameCameraOffset.X + 8, Y - Handler.gameState.gameCameraOffset.Y + 34);
            cooldownBar.Position = new Vector2f(X - Handler.gameState.gameCameraOffset.X + 9, Y - Handler.gameState.gameCameraOffset.Y + 35);
            window.Draw(cooldownBarBackground);
            window.Draw(cooldownBar);

            inventoryInterface.render(window);
        }
    }
}
