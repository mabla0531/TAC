namespace TAC {
    abstract class StorageEntity : Entity {
        
        public StorageEntity(float x, float y) : base() {
            Health = 1;
            MaxHealth = 1;

            X = x;
            Y = y;

            collisionBounds.Left   = 0;
            collisionBounds.Top    = 0;
            collisionBounds.Width  = 32;
            collisionBounds.Height = 32;

            inventory = new Inventory();
        }
    }
}
