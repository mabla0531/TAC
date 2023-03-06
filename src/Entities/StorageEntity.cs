using SFML.System;
using SFML.Graphics;

namespace TAC {
    abstract class StorageEntity : Entity {
        
        public StorageEntity(Vector2f p) : base() {
            Health = 1;
            MaxHealth = 1;

            X = p.X;
            Y = p.Y;

            collisionBounds.Left   = 0;
            collisionBounds.Top    = 0;
            collisionBounds.Width  = 32;
            collisionBounds.Height = 32;

            inventory = new Inventory();
        }
    }
}