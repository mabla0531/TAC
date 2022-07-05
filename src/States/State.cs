using SFML.Graphics;

namespace TAC {
    
    abstract class State {

        public State() {
            
        }

        public abstract void tick();
        public abstract void render(RenderWindow window);
    }
}