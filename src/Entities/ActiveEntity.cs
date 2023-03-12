using System;
using SFML.System;
using SFML.Graphics;

namespace TAC {

    abstract class ActiveEntity : Entity {
        protected float curSpeed = 1.0f;
        protected IntRect[] animationFrames;
        protected int currAnimFrame = 0, animFrameInterTime = 250, direction = 0; //0 = down, 1 = up, 2 = left, 3 = right, 4 = idle
        protected Clock animFrameDelay;

        protected int attackAlpha;
        protected Random attackRNG;
        protected Clock attackCooldown;
        protected int attackInterval;

        public float InteractionRange {get; set;}

        protected float knockbackX, knockbackY;
        protected float appliedKnockbackX, appliedKnockbackY;

        protected float moveX, moveY;

        public ActiveEntity() : base() {
            attackRNG = new Random();
            attackCooldown = new Clock();
            attackInterval = 1500;
            InteractionRange = 64.0f;
            knockbackX = 0.0f;
            knockbackY = 0.0f;
            appliedKnockbackX = 0.0f;
            appliedKnockbackY = 0.0f;
            MaxHealth = 10;
            Health = MaxHealth;

            IsKillable = true;

            tick += () => {
                moveX = 0.0f;
                moveY = 0.0f;
                direction = 4;

                if (DisplayHealth > Health) {
                    DisplayHealth -= 0.05f;
                }
            };

            render += (RenderWindow window) => {
                EntitySprite.TextureRect = new IntRect(32, 0, 32, 32);

                if (direction != 4)
                    EntitySprite.TextureRect = animationFrames[currAnimFrame + (direction * 4)];

            };
        }

        protected void attack(ActiveEntity e, float angle, int damage) {
            e.hurt(damage);
            knockback(e, angle, 32.0f);
        }

        protected void knockback(ActiveEntity e, double angle, float power) {
            e.knockbackX = (float)Math.Cos(angle) * power;
            e.knockbackY = (float)Math.Sin(angle) * power;
        }

        public abstract void hurt(int damage);

        public void checkCollision() {
            FloatRect collisionBounds = getCollisionBounds();
            FloatRect movingCollisionBounds = collisionBounds;
            FloatRect movingCollisionBoundsX = collisionBounds;
            FloatRect movingCollisionBoundsY = collisionBounds;
            movingCollisionBoundsX.Left += moveX; //offset moving collision bounds by movement variables
            movingCollisionBoundsY.Top  += moveY;
            movingCollisionBounds.Left += moveX;
            movingCollisionBounds.Top  += moveY;

            //entity collision
            foreach (Entity e in Handler.gameState.Entities) {
                if (e != this && e.getCollisionBounds().Intersects(movingCollisionBoundsX)) {
                    moveX = 0.0f;
                }
                if (e != this && e.getCollisionBounds().Intersects(movingCollisionBoundsY)) {
                    moveY = 0.0f;
                }
            }

            //tile collision
            if (((GameState.currentMap.SolidTiles.Contains(GameState.currentMap.getTileByCoords((int)(movingCollisionBounds.Left), (int)(collisionBounds.Top))) || 
                  GameState.currentMap.SolidTiles.Contains(GameState.currentMap.getTileByCoords((int)(movingCollisionBounds.Left), (int)(collisionBounds.Top + collisionBounds.Height)))) && moveX < 0.0f) || 
                ((GameState.currentMap.SolidTiles.Contains(GameState.currentMap.getTileByCoords((int)(movingCollisionBounds.Left + collisionBounds.Width), (int)(collisionBounds.Top))) || 
                  GameState.currentMap.SolidTiles.Contains(GameState.currentMap.getTileByCoords((int)(movingCollisionBounds.Left + collisionBounds.Width), (int)(collisionBounds.Top + collisionBounds.Height)))) && moveX > 0.0f)) {
                moveX = 0.0f;
            }
            if (((GameState.currentMap.SolidTiles.Contains(GameState.currentMap.getTileByCoords((int)(collisionBounds.Left), (int)(movingCollisionBounds.Top))) || 
                  GameState.currentMap.SolidTiles.Contains(GameState.currentMap.getTileByCoords((int)(collisionBounds.Left + collisionBounds.Width), (int)(movingCollisionBounds.Top)))) && moveY < 0.0f) ||
                ((GameState.currentMap.SolidTiles.Contains(GameState.currentMap.getTileByCoords((int)(collisionBounds.Left), (int)(movingCollisionBounds.Top + collisionBounds.Height))) || 
                  GameState.currentMap.SolidTiles.Contains(GameState.currentMap.getTileByCoords((int)(collisionBounds.Left + collisionBounds.Width), (int)(movingCollisionBounds.Top + collisionBounds.Height)))) && moveY > 0.0f)) {
                moveY = 0.0f;
            }
        }

        public void tickAnimation() {
            if (direction == 4) {
                currAnimFrame = 0;
            } else {
                if (animFrameDelay.ElapsedTime.AsMilliseconds() >= animFrameInterTime * 1)
                    currAnimFrame = 1;
                if (animFrameDelay.ElapsedTime.AsMilliseconds() >= animFrameInterTime * 2)
                    currAnimFrame = 2;
                if (animFrameDelay.ElapsedTime.AsMilliseconds() >= animFrameInterTime * 3)
                    currAnimFrame = 3;
                if (animFrameDelay.ElapsedTime.AsMilliseconds() >= animFrameInterTime * 4) {
                    currAnimFrame = 0;
                    animFrameDelay.Restart();
                }
            }
        }

        public void doKnockback() {
            
            if (Math.Abs(appliedKnockbackX) < Math.Abs(knockbackX)) {
                appliedKnockbackX += (knockbackX / 15.0f);
                moveX += (knockbackX / 15.0f);
            }
            if (Math.Abs(appliedKnockbackX) >= Math.Abs(knockbackX)) {
                knockbackX = 0.0f;
                appliedKnockbackX = 0.0f;
            }
            if (Math.Abs(appliedKnockbackY) < Math.Abs(knockbackY)) {
                appliedKnockbackY += (knockbackY / 15.0f);
                moveY += (knockbackY / 15.0f);
            }
            if (Math.Abs(appliedKnockbackY) >= Math.Abs(knockbackY)) {
                knockbackY = 0.0f;
                appliedKnockbackY = 0.0f;
            }
        }
    }
}
