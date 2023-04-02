using System;
using SFML.Graphics;
using SFML.System;

namespace TAC {
    class FriendlyMob : ActiveEntity {

        private RectangleShape healthBarBackground, healthBar;
        public string DialogType {get; set;}

        public FriendlyMob(float x, float y) : base() {
            X = x;
            Y = y;

            collisionBounds = new FloatRect(6.0f, 8.0f, 20.0f, 22.0f);

            EntitySprite = new Sprite(Assets.friendly1);
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

            attackAlpha = 0;

            InteractionRange = 32.0f;

            MaxHealth = 5;
            Health = MaxHealth;
            DisplayHealth = Health;

            healthBarBackground = new RectangleShape(new Vector2f(18, 4));
            healthBarBackground.FillColor = new Color(200, 200, 200);
            healthBar = new RectangleShape(new Vector2f(16, 2));
            healthBar.FillColor = new Color(32, 32, 32);

            DialogType = "merchant";

            tick += () => {

                doKnockback();
                X += moveX;
                Y += moveY;

                tickAnimation();
            };

            render += (RenderWindow window) => {
                
            };
        }

        public override void hurt(int damage) {
            Health -= damage;
            Handler.gameState.DamageNumbers.Add(new DamageNumber(X + 16.0f, Y - 8.0f, damage));
            Assets.grrr.Play();
        }
    }
}
