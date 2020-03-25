#include "headers/Animation.h"

Animation::Animation() {

}

Animation::Animation(const char* path, sf::IntRect* textureRects, int frameCount) {
    sf::Texture* texture;
    texture = new sf::Texture();

    texture->loadFromFile(path);
    spriteFrames = frameCount;
    sprite = new sf::Sprite[spriteFrames];

    for (int i = 0; i < frameCount; i++) {
        sprite[i] = sf::Sprite(*texture, textureRects[i]);
    }
    
    spriteIndex = 0;
}

Animation::~Animation() {

}

void Animation::cycleFrame() {
    spriteIndex++;
    if (spriteIndex >= spriteFrames) {
        spriteIndex = 0;
    }
}

void Animation::resetFrame() {
    spriteIndex = 0;
}

void Animation::render(sf::RenderWindow* window, int x, int y) {
    sprite[spriteIndex].setPosition(x, y);
    window->draw(sprite[spriteIndex]);
}

void Animation::setLocation(int x, int y) {
    for (int i = 0; i < spriteFrames; i++) {
        sprite[i].setPosition(x, y);
    }
}

sf::Sprite* Animation::getFrames() {
    return sprite;
}