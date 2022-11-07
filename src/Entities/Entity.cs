using SFML.Graphics;
using SFML.System;

namespace TAC {
    abstract class Entity {
        public float X { get; set; }
        public float Y { get; set; }

        protected FloatRect collisionBounds;

        public int MaxHealth {get; set;}
        public int Health {get; set;}
        public float DisplayHealth {get; set;}

        protected Sprite sprite;

        public Entity() {

        }

        public abstract void tick();
        public abstract void render(RenderWindow window);

        public FloatRect getCollisionBounds() {
            return new FloatRect(collisionBounds.Left + X, collisionBounds.Top + Y, collisionBounds.Width, collisionBounds.Height);
        }
    }
}
