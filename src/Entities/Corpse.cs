using SFML.Graphics;

namespace TAC {
    class Corpse : StorageEntity {
        public Corpse(float x, float y) : base(x, y) {
            EntitySprite = new Sprite(Assets.corpse, new IntRect(0, 0, 32, 32));
            collisionBounds = new FloatRect(4.0f, 8.0f, 24.0f, 12.0f);
        }
    }
 }