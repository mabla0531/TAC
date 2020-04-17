#pragma once
#include <SFML/Graphics.hpp>
#include "Entity.h"
#include "InventoryItem.h"

class GroundItem : public Entity {
    private:
    bool stackable;
    int count;
    int healthMod, defenseMod, attackMod;
    float weight;

    public:
    GroundItem();
    GroundItem(int x, int y, sf::Texture* texture, sf::IntRect textureRect, int mods[3], bool stackable, int count, float weight);
    
    void knockBack(float x, float y);

    void tick(std::vector<Entity*>* entities);
    void render(sf::RenderWindow* window, int xCameraOffset, int yCameraOffset);
    void renderToolTip(sf::RenderWindow* window, int xCameraOffset, int yCameraOffset);

    sf::Sprite getSprite();
    bool isStackable();
    int getCount();
    int* getStatMods();
    float getWeight();
};
