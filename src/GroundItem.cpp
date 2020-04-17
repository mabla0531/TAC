#include "headers/GroundItem.h"

GroundItem::GroundItem() {

}

GroundItem::GroundItem(int x, int y, sf::Texture* texture, sf::IntRect textureRect, int mods[3], bool stackable, int count, float weight) : Entity(x, y) {
    staticSprite = sf::Sprite(*texture, textureRect);

    width = 16;
    height = 16;

    healthMod = mods[0];
    defenseMod = mods[1];
    attackMod = mods[2];

    this->stackable = stackable;
    this->count = count;
    this->weight = weight;
}

void GroundItem::knockBack(float x, float y) {
    
}

void GroundItem::tick(std::vector<Entity*>* entities) {
    
}

void GroundItem::render(sf::RenderWindow* window, int xCameraOffset, int yCameraOffset) {
    staticSprite.setPosition(x - xCameraOffset, y - yCameraOffset);
    window->draw(staticSprite);
}

void GroundItem::renderToolTip(sf::RenderWindow* window, int xCameraOffset, int yCameraOffset) {
    tooltip.setPosition(x - 24 - xCameraOffset, y + 24 - yCameraOffset);
    window->draw(tooltip);
    tooltipText.setPosition(x - 20 - xCameraOffset, y + 20 - yCameraOffset);
    window->draw(tooltipText);
}

bool GroundItem::isStackable() {
    return stackable;
}

int GroundItem::getCount() {
    return count;
}

int* GroundItem::getStatMods() {
     int statMods[3] = {healthMod, defenseMod, attackMod};
     return statMods;
}

float GroundItem::getWeight() {
    return weight;
}
