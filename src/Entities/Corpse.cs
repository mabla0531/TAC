/*
 * Corpse class is used universally with all active entities that are capable of death
 * Takes a copy of the idle sprite for that entity and flips it -90/90 degrees randomly
 * Will hold an inventory later
 */

using SFML.Graphics;

namespace TAC {
    class Corpse : StorageEntity {
        public Corpse(float x, float y) : base(x, y) {
            EntitySprite = new Sprite(Assets.corpse, new IntRect(0, 0, 32, 32));
            collisionBounds = new FloatRect(0.0f, 8.0f, 32.0f, 16.0f);
        }
    }
 }