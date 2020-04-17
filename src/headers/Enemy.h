#pragma once
#include "Player.h"

class Enemy : public Creature {

    private:
    bool hostile;
    Player* player;
    Animation up, left, right;

    public:
    Enemy();
    Enemy(int x, int y, Player* player);
    ~Enemy();

    void tick(std::vector<Entity*>* entities);

};