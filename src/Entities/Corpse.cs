using SFML.Graphics;

namespace TAC {
    class Corpse : StorageEntity {
        public Corpse(float x, float y) : base(x, y) {
            EntitySprite = new Sprite(Assets.corpse, new IntRect(0, 0, 32, 32));
            collisionBounds = new FloatRect(0.0f, 0.0f, 0.0f, 0.0f);
        }
    }
 }