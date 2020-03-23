#include "headers/Tile.h"

Tile::Tile(int x, int y, Type type, sf::Texture* textureMap) {
    this->x = x;
    this->y = y;

    sprite = sf::Sprite(*textureMap);
    
    sf::IntRect textureRect;
    switch(type) {
        case GRASS:
            textureRect = sf::IntRect(32, 64, 32, 32);
        break;
        case DIRT:
            textureRect = sf::IntRect(288, 64, 32, 32);
        break;
        case STONE:
            textureRect = sf::IntRect(416, 64, 32, 32);
        break;
        case WOOD:
            textureRect = sf::IntRect(608, 640, 32, 32);
        break;
        default:
            textureRect = sf::IntRect(32, 64, 32, 32);
    }

    sprite.setTextureRect(textureRect);
    sprite.setPosition(sf::Vector2f(x * 32, y * 32));
}

Tile::Tile() {

}

Tile::~Tile() {

}

void Tile::render(sf::RenderWindow* window) {
    window->draw(sprite);
}

int Tile::getX() {
    return x;
}

int Tile::getY() {
    return y;
}

bool Tile::isSolid() {
    return solid;
}

sf::Sprite Tile::getSprite() {
    return sprite;
}