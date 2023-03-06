
using SFML.System;
using SFML.Graphics;

namespace TAC {

    class Tree : Entity
    {

        public Tree(float x = 0.0f, float y = 0.0f) : base() {
            MaxHealth = 1;
            Health = MaxHealth;
            
            X = x;
            Y = y;

            collisionBounds = new FloatRect(16.0f, 96.0f, 16.0f, 32.0f);//preliminarily make the boundary just the trunk (or close to it)
            EntitySprite = new Sprite(Assets.tree);

            tick += () => {};

            render += (RenderWindow window) => {
                EntitySprite.Position = new Vector2f(X - Handler.gameState.gameCameraOffset.X, Y - Handler.gameState.gameCameraOffset.Y);
                window.Draw(EntitySprite);
            };
        }
    }
}