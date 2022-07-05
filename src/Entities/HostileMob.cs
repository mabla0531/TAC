using System;
using SFML.Graphics;
using SFML.System;

namespace TAC {
    class HostileMob : ActiveEntity
    {

        public HostileMob(float x, float y) : base() {
            X = x;
            Y = y;

            collisionBounds = new FloatRect(6.0f, 8.0f, 20.0f, 22.0f);

            sprite = new Sprite(Assets.enemy);
            sprite.TextureRect = new IntRect(32, 0, 32, 32);

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

            MaxHealth = 5;
            Health = MaxHealth;
        }

        public override void hurt(int damage) {
            Health -= damage;
            Assets.grrr.Play();
        }

        public override void specificTick() {

            float playerDeltaX = (Handler.gameState.player.X + 16) - (X + 16);
            float playerDeltaY = (Handler.gameState.player.Y + 16) - (Y + 16); //add 16 to get center

            float distanceToPlayer = (float)Math.Sqrt((playerDeltaX * playerDeltaX) + (playerDeltaY * playerDeltaY)); //pythagorean theorem
            double angle = Math.Atan2(playerDeltaY, playerDeltaX);

            //attack player
            if (distanceToPlayer < 32.0f && attackCooldown.ElapsedTime.AsMilliseconds() >= attackInterval) {
                attack(Handler.gameState.player);
                knockback(Handler.gameState.player, angle, 50.0f);
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
                    direction = 5;
            }

            doKnockback();
            X += moveX;
            Y += moveY;

            tickAnimation();
        }

        public override void specificRender(RenderWindow window) {
            
        }
    }
}