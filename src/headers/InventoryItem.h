#pragma once
#include <SFML/Graphics.hpp>
#include "GroundItem.h"

class InventoryItem {

    private:
    sf::Sprite icon;
    bool stackable;
    int count;
    int healthMod, defenseMod, attackMod;
    //stat mods here
    float weight;

    public:
    InventoryItem();
    InventoryItem(sf::Texture* texture, sf::IntRect textureRect, int mods[3], bool stackable, int count, float weight);
    
    void render(sf::RenderWindow* window, int x, int y);

    sf::Sprite getSprite();
    bool isStackable();
    int getCount();
    int* getStatMods();
    float getWeight();
};
