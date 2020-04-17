#pragma once
#include "Creature.h"
#include <vector>
#include "InventoryItem.h"

class Player : public Creature {
    
    private:
    sf::RenderWindow* window;
    int* xCameraOffset;
    int* yCameraOffset;
    Animation up, left, right;
    int animationCycleDelay = 200;
    
    float carryCapacity = 10.0f, weight = 0.0f;
    std::vector<InventoryItem> inventory;

    public:
    Player();
    Player(int x, int y, sf::RenderWindow* window, int* xCameraOffset, int* yCameraOffset);
    ~Player();

    void tick(std::vector<Entity*>* entities);
    bool addItem(InventoryItem item);
};
