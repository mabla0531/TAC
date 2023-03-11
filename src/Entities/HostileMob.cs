using System;
using SFML.Graphics;
using SFML.System;

namespace TAC {
    class HostileMob : ActiveEntity {

        private RectangleShape healthBarBackground, healthBar;

        public HostileMob(float x, float y) : base() {
            X = x;
            Y = y;

            collisionBounds = new FloatRect(6.0f, 8.0f, 20.0f, 22.0f);

            EntitySprite = new Sprite(Assets.enemy);
            EntitySprite.TextureRect = new IntRect(32, 0, 32, 32);
            
            DefaultTextureRect = new IntRect(32, 0,  32, 32);
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

            curSpeed = 0.5f;

            attackAlpha = 1;

            InteractionRange = 32.0f;

            MaxHealth = 5;
            Health = MaxHealth;
            DisplayHealth = Health;

            healthBarBackground = new RectangleShape(new Vector2f(18, 4));
            healthBarBackground.FillColor = new Color(200, 200, 200);
            healthBar = new RectangleShape(new Vector2f(16, 2));
            healthBar.FillColor = new Color(32, 32, 32);

            tick += () => {
                float playerDeltaX = (Handler.gameState.player.X + 16) - (X + 16);
                float playerDeltaY = (Handler.gameState.player.Y + 16) - (Y + 16); //add 16 to get center

                float distanceToPlayer = (float)Math.Sqrt((playerDeltaX * playerDeltaX) + (playerDeltaY * playerDeltaY)); //pythagorean theorem
                float angle = (float)Math.Atan2(playerDeltaY, playerDeltaX);

                //attack player
                if (distanceToPlayer < InteractionRange && attackCooldown.ElapsedTime.AsMilliseconds() >= attackInterval) {
                    attack(Handler.gameState.player, angle, attackAlpha);
                    attackCooldown.Restart();
                }

                //move towards player
                if (distanceToPlayer < 400.0f) {
                    moveX = (float)Math.Cos(angle) * curSpeed;
                    moveY = (float)Math.Sin(angle) * curSpeed;

                    if (angle < -(Math.PI * 3/4) || angle > (Math.PI * 3/4))
                        direction = 2;
                    if ((angle < 0.0f && angle > -(Math.PI * 1/4)) || (angle > 0.0f && angle < (Math.PI * 1/4)))
                        direction = 3;
                    if (angle > -(Math.PI * 3/4) && angle < -(Math.PI * 1/4))
                        direction = 1;
                    if (angle < (Math.PI * 3/4) && angle > (Math.PI * 1/4))
                        direction = 0;

                    checkCollision();

                    if (moveX == 0.0f && moveY == 0.0f)
                        direction = 4;
                }

                doKnockback();
                X += moveX;
                Y += moveY;

                tickAnimation();
            };

            render += (RenderWindow window) => {
                healthBar.Size = new Vector2f(16.0f * (DisplayHealth / (float)MaxHealth), 2.0f);
                healthBar.FillColor = new Color(158, 43, 35);
                healthBarBackground.Position = new Vector2f(X - Handler.gameState.gameCameraOffset.X + 8, Y - Handler.gameState.gameCameraOffset.Y - 4);
                healthBar.Position = new Vector2f(X - Handler.gameState.gameCameraOffset.X + 9, Y - Handler.gameState.gameCameraOffset.Y - 3);
                
                window.Draw(healthBarBackground);
                window.Draw(healthBar);
            };

        }

        public override void hurt(int damage) {
            Health -= damage;
            Handler.gameState.DamageNumbers.Add(new DamageNumber(X + 16.0f, Y - 8.0f, damage));
            Assets.grrr.Play();
        }
    }
}
