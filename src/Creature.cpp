#include "headers/Creature.h"

Creature::Creature(int x, int y) : Entity(x, y) {
    
}

Creature::Creature() {

}

Creature::~Creature() {

}

void Creature::tick() {

}

void Creature::render(sf::RenderWindow* window, int xCameraOffset, int yCameraOffset) {
    if (moving)
        currentAnimation->render(window, x - xCameraOffset, y - yCameraOffset);
    else {
        staticSprite.setPosition(x - xCameraOffset, y - yCameraOffset);
        window->draw(staticSprite);
    }
}

void Creature::move(float x, float y) {
    this->x += x;
    this->y += y;
}


float Creature::getX() {
    return x;
}

float Creature::getY() {
    return y;
}