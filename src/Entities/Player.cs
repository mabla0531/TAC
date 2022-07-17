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

        //HUD objects
        private Sprite hudFrameCornerNW, hudFrameCornerNE, hudFrameCornerSW, hudFrameCornerSE, 
                       hudFrameEdgeN, hudFrameEdgeS, hudFrameEdgeW, hudFrameEdgeE, hudFrameMiddle;

        //healthbar objects
        private Sprite statBarL, statBar, statBarR;
        private Sprite healthFill;
        private Sprite staminaFill;

        private float mouseAngle;
        private Sprite sword;

        private RectangleShape cooldownBarBackground, cooldownBar;

        //Inventory
        private InventoryInterface inventoryInterface;
        private bool inventoryKeyPressed = false;

        public Player (float x, float y) : base() {
            X = x;
            Y = y;

            collisionBounds = new FloatRect(6.0f, 8.0f, 20.0f, 22.0f);

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
            displayHealth = (float)Health;

            attackAlpha = 1;

            cooldownBarBackground = new RectangleShape(new Vector2f(18, 4));
            cooldownBarBackground.FillColor = new Color(200, 200, 200);
            cooldownBar = new RectangleShape(new Vector2f(16, 2));
            cooldownBar.FillColor = new Color(32, 32, 32);

            initHUD();

            inventoryInterface = new InventoryInterface(inventory);
            inventory.addItem(Item.sword);
            inventory.addItem(Item.axe);
        }

        public void initHUD() {
            
            hudFrameCornerNW= new Sprite(Assets.ui);
            hudFrameCornerNE= new Sprite(Assets.ui);
            hudFrameCornerSW= new Sprite(Assets.ui);
            hudFrameCornerSE= new Sprite(Assets.ui);
            hudFrameEdgeN   = new Sprite(Assets.ui);
            hudFrameEdgeS   = new Sprite(Assets.ui);
            hudFrameEdgeW   = new Sprite(Assets.ui);
            hudFrameEdgeE   = new Sprite(Assets.ui);
            hudFrameMiddle  = new Sprite(Assets.ui);

            statBarL     = new Sprite(Assets.ui);
            statBar      = new Sprite(Assets.ui);
            statBarR     = new Sprite(Assets.ui);
            healthFill   = new Sprite(Assets.ui);
            staminaFill  = new Sprite(Assets.ui);

            hudFrameCornerNW.TextureRect= new IntRect(16,  40, 32, 32);
            hudFrameCornerNE.TextureRect= new IntRect(82,  40, 32, 32);
            hudFrameCornerSW.TextureRect= new IntRect(478, 80, 32, 32);
            hudFrameCornerSE.TextureRect= new IntRect(544, 80, 32, 32);
            hudFrameEdgeN.TextureRect   = new IntRect(49,  40, 32, 32);
            hudFrameEdgeS.TextureRect   = new IntRect(511, 80, 32, 32);
            hudFrameEdgeW.TextureRect   = new IntRect(478, 24, 32, 32);
            hudFrameEdgeE.TextureRect   = new IntRect(544, 24, 32, 32);
            hudFrameMiddle.TextureRect  = new IntRect(511, 24, 32, 32);
            hudFrameCornerNW.Position   = new Vector2f(0, Game.displayHeight - 128);
            hudFrameCornerNE.Position   = new Vector2f(Game.displayWidth - 32, Game.displayHeight - 128);
            hudFrameCornerSW.Position   = new Vector2f(0, Game.displayHeight - 32);
            hudFrameCornerSE.Position   = new Vector2f(Game.displayWidth - 32, Game.displayHeight - 32);
            hudFrameEdgeN.Position      = new Vector2f(32, Game.displayHeight - 128);
            hudFrameEdgeN.Scale         = new Vector2f((Game.displayWidth / 32) - 2, 1.0f);
            hudFrameEdgeS.Position      = new Vector2f(32, Game.displayHeight - 32);
            hudFrameEdgeS.Scale         = new Vector2f((Game.displayWidth / 32) - 2, 1.0f);
            hudFrameEdgeE.Position      = new Vector2f(Game.displayWidth - 32, Game.displayHeight - 96);
            hudFrameEdgeE.Scale         = new Vector2f(1.0f, 2.0f);
            hudFrameEdgeW.Position      = new Vector2f(0, Game.displayHeight - 96);
            hudFrameEdgeW.Scale         = new Vector2f(1.0f, 2.0f);
            hudFrameMiddle.Position     = new Vector2f(32, Game.displayHeight - 96);
            hudFrameMiddle.Scale        = new Vector2f((Game.displayWidth / 32) - 2, 2.0f);

            statBarL.TextureRect        = new IntRect(258, 38, 24, 24);
            statBar.TextureRect         = new IntRect(283, 38, 24, 24);
            statBarR.TextureRect        = new IntRect(308, 38, 24, 24);
            statBar.Scale               = new Vector2f(2.5f, 1.0f);
            healthFill.TextureRect      = new IntRect(349, 39, 8, 16);
            healthFill.Position         = new Vector2f(17,  Game.displayHeight - 116);
            healthFill.Scale            = new Vector2f(10.0f * (displayHealth / (float)MaxHealth), 1.0f);
            staminaFill.TextureRect     = new IntRect(349, 72, 8, 16);
            staminaFill.Position        = new Vector2f(17,  Game.displayHeight - 91);
            staminaFill.Scale           = new Vector2f(10.0f * (Stamina / (float)MaxStamina), 1.0f);
        }

        public override void hurt(int damage) {
            Health -= damage;
        }

        public override void specificTick() {

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

            if (displayHealth > Health) {
                displayHealth -= 0.1f;
            }
            
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


            if (MouseHandler.LeftClick && attackCooldown.ElapsedTime.AsMilliseconds() >= attackInterval) {
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
            statBar.Position    = new Vector2f(28, Game.displayHeight - 120);
            statBarR.Position   = new Vector2f(82, Game.displayHeight - 120);
            window.Draw(statBarL);
            window.Draw(statBar);
            window.Draw(statBarR);

            //draw in position of stamina bar
            statBarL.Position   = new Vector2f(8,  Game.displayHeight - 96);
            statBar.Position    = new Vector2f(28, Game.displayHeight - 96);
            statBarR.Position   = new Vector2f(82, Game.displayHeight - 96);
            window.Draw(statBarL);
            window.Draw(statBar);
            window.Draw(statBarR);

            healthFill.Scale = new Vector2f(10.0f * (displayHealth / (float)MaxHealth), 1.0f);
            window.Draw(healthFill);
            staminaFill.Scale = new Vector2f(10.0f * (Stamina / (float)MaxStamina), 1.0f);
            window.Draw(staminaFill);
        }

        public override void specificRender(RenderWindow window) {
            sword.Position = new Vector2f(X - Handler.gameState.gameCameraOffset.X + 16.0f, Y - Handler.gameState.gameCameraOffset.Y + 20.0f);
            window.Draw(sword);

            cooldownBarBackground.Position = new Vector2f(X - Handler.gameState.gameCameraOffset.X + 8, Y - Handler.gameState.gameCameraOffset.Y + 34);
            cooldownBar.Position = new Vector2f(X - Handler.gameState.gameCameraOffset.X + 9, Y - Handler.gameState.gameCameraOffset.Y + 35);
            window.Draw(cooldownBarBackground);
            window.Draw(cooldownBar);

            renderHUD(window);

            inventoryInterface.render(window);
        }
    }
}
