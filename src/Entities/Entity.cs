using SFML.Graphics;
using SFML.System;

namespace TAC {
    abstract class Entity {
        public float X { get; set; }
        public float Y { get; set; }

        protected FloatRect collisionBounds;
        public bool Hovered {get; set;} = false;

        public int MaxHealth {get; set;}
        public int Health {get; set;}
        public float DisplayHealth {get; set;}
        public bool IsKillable {get; set;}

        public IntRect DefaultTextureRect {get; set;}
        public Sprite EntitySprite {get; set;}

        public Inventory inventory {get; set;} //I would love to capitalize it, I really would, but unfortunately my hands are tied


        public delegate void Tick();
        public Tick tick;
        public delegate void Render(RenderWindow window);
        public Render render;

        public Entity() {
            collisionBounds.Left   = 0;
            collisionBounds.Top    = 0;
            collisionBounds.Width  = 32;
            collisionBounds.Height = 32;

            IsKillable = false;

            inventory = new Inventory();

            tick += () => {
                Hovered = false;
                if (new IntRect((int)(X - Handler.gameState.gameCameraOffset.X), (int)(Y - Handler.gameState.gameCameraOffset.Y), EntitySprite.TextureRect.Width, EntitySprite.TextureRect.Height).Contains((int)MouseHandler.MouseX, (int)MouseHandler.MouseY))
                    Hovered = true;
            };

            render += (RenderWindow window) => {
                EntitySprite.Position = new Vector2f((int)(X - Handler.gameState.gameCameraOffset.X), (int)(Y - Handler.gameState.gameCameraOffset.Y));
                window.Draw(EntitySprite);
            };
        }

        public FloatRect getCollisionBounds() {
            return new FloatRect(collisionBounds.Left + X, collisionBounds.Top + Y, collisionBounds.Width, collisionBounds.Height);
        }
    }
}
