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

        protected float knockbackX, knockbackY;
        protected float appliedKnockbackX, appliedKnockbackY;

        protected float moveX, moveY;

        public ActiveEntity() : base() {
            attackRNG = new Random();
            attackCooldown = new Clock();
            attackInterval = 1500;
            knockbackX = 0.0f;
            knockbackY = 0.0f;
            appliedKnockbackX = 0.0f;
            appliedKnockbackY = 0.0f;
            MaxHealth = 10;
            Health = MaxHealth;
        }

        protected void attack(ActiveEntity e) {
            e.hurt(attackAlpha);
        }

        protected void knockback(ActiveEntity e, double angle, float power) {
            e.knockbackX = (float)Math.Cos(angle) * power;
            e.knockbackY = (float)Math.Sin(angle) * power;
        }

        public abstract void hurt(int damage);

        public void checkCollision() {
            FloatRect absoluteCollisionBounds = getCollisionBounds();
            absoluteCollisionBounds.Left += moveX;
            absoluteCollisionBounds.Top  += moveY;
            foreach (Entity e in Handler.gameState.Entities) {
                if (e == this)
                    continue;
                if (e.getCollisionBounds().Intersects(absoluteCollisionBounds)) {
                    moveX = 0.0f;
                    moveY = 0.0f;
                }
            }

            if (((GameState.currentMap.solidTiles.Contains(GameState.currentMap.getTileByCoords((int)(X - curSpeed), (int)(Y))) || 
                  GameState.currentMap.solidTiles.Contains(GameState.currentMap.getTileByCoords((int)(X - curSpeed), (int)(Y + 32)))) && moveX < 0.0f) || 
                ((GameState.currentMap.solidTiles.Contains(GameState.currentMap.getTileByCoords((int)(X + 32 + curSpeed), (int)(Y))) || 
                  GameState.currentMap.solidTiles.Contains(GameState.currentMap.getTileByCoords((int)(X + 32 + curSpeed), (int)(Y + 32)))) && moveX > 0.0f)) {
                moveX = 0.0f;
            }
            if (((GameState.currentMap.solidTiles.Contains(GameState.currentMap.getTileByCoords((int)(X), (int)(Y - curSpeed))) || 
                  GameState.currentMap.solidTiles.Contains(GameState.currentMap.getTileByCoords((int)(X + 32), (int)(Y - curSpeed)))) && moveY < 0.0f) ||
                ((GameState.currentMap.solidTiles.Contains(GameState.currentMap.getTileByCoords((int)(X), (int)(Y + 32 + curSpeed))) || 
                  GameState.currentMap.solidTiles.Contains(GameState.currentMap.getTileByCoords((int)(X + 32), (int)(Y + 32 + curSpeed)))) && moveY > 0.0f)) {
                moveY = 0.0f;
            }
        }

        public void tickAnimation() {
            if (direction == 5) {
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

        public abstract void specificTick();

        public override void tick() {
            moveX = 0.0f;
            moveY = 0.0f;
            direction = 5;

            specificTick();
        }

        public abstract void specificRender(RenderWindow window);

        public override void render(RenderWindow window) {

            sprite.TextureRect = new IntRect(32, 0, 32, 32);
            if (direction != 5)
                sprite.TextureRect = animationFrames[currAnimFrame + (direction * 4)];

            sprite.Position = new Vector2f(X - Handler.gameState.gameCameraOffset.X, Y - Handler.gameState.gameCameraOffset.Y);
            window.Draw(sprite);

            specificRender(window);
        }
    }
}
