#include "headers/InventoryItem.h"

InventoryItem::InventoryItem() {

}

InventoryItem::InventoryItem(sf::Texture* texture, sf::IntRect textureRect, int mods[3], bool stackable, int count, float weight) {
    icon = sf::Sprite(*texture, textureRect);
    
    healthMod = mods[0];
    defenseMod = mods[1];
    attackMod = mods[2];

    this->stackable = stackable;
    this->count = count;
    this->weight = weight;
}

void InventoryItem::render(sf::RenderWindow* window, int x, int y) {
    icon.setPosition(x, y);
    window->draw(icon);
}

sf::Sprite InventoryItem::getSprite() {
    return icon;
}

bool InventoryItem::isStackable() {
    return stackable;
}

int InventoryItem::getCount() {
    return count;
}

int* InventoryItem::getStatMods() {
     int statMods[3] = {healthMod, defenseMod, attackMod};
     return statMods;
}

float InventoryItem::getWeight() {
    return weight;
}
