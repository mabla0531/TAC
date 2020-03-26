#pragma once
#include "Creature.h"

class Player : public Creature {
    
    private:
    Animation up, left, right;
    
    public:
    Player();
    Player(int x, int y);
    ~Player();

    void tick();
};
