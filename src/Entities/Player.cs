using System;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace TAC {

    class Player : ActiveEntity {
        private float walkSpeed = 1.5f, runSpeed = 2.5f;

        //running stuff
        public float Stamina {get; set;} = 100.0f;
        public float MaxStamina {get; set;} = 100.0f;
        private float staminaDecaySpeed = 0.5f;
        private bool cooldown = false;

        //player addons
        private Sprite aimAngle;

        private RectangleShape cooldownBarBackground, cooldownBar;

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

            EntitySprite = new Sprite(Assets.player);
            EntitySprite.TextureRect = new IntRect(32, 0, 32, 32);

            aimAngle = new Sprite(Assets.items);
            aimAngle.TextureRect = new IntRect(16, 112, 16, 16);

            DefaultTextureRect = new IntRect(32, 0,  32, 32);
            animationFrames = new IntRect[16] {new IntRect(0,  0,  32, 32), //factors in direction (0-4, set of four frames)
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

            tick += () => {
                float mouseAngle;

                float mouseDeltaX = MouseHandler.MouseX - (X - Handler.gameState.gameCameraOffset.X + 16);
                float mouseDeltaY = MouseHandler.MouseY - (Y - Handler.gameState.gameCameraOffset.Y + 16); //add 16 to get center

                mouseAngle = (float)Math.Atan2(mouseDeltaY, mouseDeltaX);
                aimAngle.Rotation = (float)((mouseAngle * (180 / Math.PI)) - 45.0f);
                
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

                if (moveX == 0.0f && moveY == 0.0f)
                        direction = 4;

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
                    Stamina += (MaxStamina / 200.0f);
                    if (Stamina > MaxStamina) Stamina = MaxStamina;
                }


                tickAnimation();

                float currentEntityDeltaX, currentEntityDeltaY;
                float currentEntityAngle;

                //do attacking
                if (MouseHandler.LeftButton && attackCooldown.ElapsedTime.AsMilliseconds() >= attackInterval && !Handler.gameState.PlayerInventory.Active && !Handler.gameState.StorageInventory.Active) {
                    Assets.swish.Play();

                    foreach (Entity e in Handler.gameState.CurrentMap.Entities) {
                        if (!(e is ActiveEntity))
                            continue;

                        currentEntityDeltaX = e.X - X;
                        currentEntityDeltaY = e.Y - Y;
                        currentEntityAngle = (float)Math.Atan2(currentEntityDeltaY, currentEntityDeltaX);

                        attackCooldown.Restart();
                        if (e != this && Math.Abs(mouseAngle - currentEntityAngle) < 0.3f && Math.Sqrt((currentEntityDeltaX * currentEntityDeltaX) + (currentEntityDeltaY * currentEntityDeltaY)) <= InteractionRange) {
                            int damage = attackAlpha;
                            if (Hand != null) damage += Hand.Attack;

                            attack((ActiveEntity)e, currentEntityAngle, damage);
                            Assets.slice.Play();
                        }
                    }
                }

                int elapsed = attackCooldown.ElapsedTime.AsMilliseconds();
                cooldownBar.Size = new Vector2f((elapsed >= attackInterval ? 16.0f : (16.0f * ((float)elapsed / (float)attackInterval))), 2.0f);
                
                cooldownBar.FillColor = new Color(32, 32, 32);
                if (elapsed >= attackInterval)
                    cooldownBar.FillColor = new Color(200, 200, 200);
            };

            render += (RenderWindow window) => {
                aimAngle.Position = new Vector2f(X - Handler.gameState.gameCameraOffset.X + 16.0f, Y - Handler.gameState.gameCameraOffset.Y + 20.0f);
                window.Draw(aimAngle);

                cooldownBarBackground.Position = new Vector2f((int)(X - Handler.gameState.gameCameraOffset.X + 8), (int)(Y - Handler.gameState.gameCameraOffset.Y + 34));
                cooldownBar.Position = new Vector2f((int)(X - Handler.gameState.gameCameraOffset.X + 9), (int)(Y - Handler.gameState.gameCameraOffset.Y + 35));
                window.Draw(cooldownBarBackground);
                window.Draw(cooldownBar);
            };
        }

        public override void hurt(int damage) {
            Health -= damage;
            Handler.gameState.DamageNumbers.Add(new DamageNumber(X + 16.0f, Y - 8.0f, damage));
        }
    }
}
