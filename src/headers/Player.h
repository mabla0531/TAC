#include "Creature.h"

class Player : public Creature {
    
    private:
    sf::Clock animationLimiter;
    Animation standard, up, left, right;
    
    public:
    Player(int x, int y);
    ~Player();

    void tick();

};
