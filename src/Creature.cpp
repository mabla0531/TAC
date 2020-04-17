#include "headers/Creature.h"

Creature::Creature(int x, int y) : Entity(x, y) {
    defense = 0;
    attack = 1;
    name = "Bad Guy";
    tooltipText.setString(name);
    healthBar.setFillColor(sf::Color::Green);
    healthBar.setSize(sf::Vector2f(health / maxHealth * 56, 4));
    healthBarBackground.setFillColor(sf::Color(164, 164, 164));
    healthBarBackground.setSize(sf::Vector2f(60, 8));
}

Creature::Creature() {

}

Creature::~Creature() {

}

void Creature::tick() {
    healthBar.setSize(sf::Vector2f(health / maxHealth * 56, 4));
}

void Creature::render(sf::RenderWindow* window, int xCameraOffset, int yCameraOffset) {
    if (moving)
        currentAnimation->render(window, x - xCameraOffset, y - yCameraOffset);
    else {
        staticSprite.setPosition(x - xCameraOffset, y - yCameraOffset);
        window->draw(staticSprite);
    }
}

void Creature::renderToolTip(sf::RenderWindow* window, int xCameraOffset, int yCameraOffset) {
    tooltip.setPosition(x - 16 - xCameraOffset, y + 40 - yCameraOffset);
    window->draw(tooltip);
    tooltipText.setPosition(x - 12 - xCameraOffset, y + 34 - yCameraOffset);
    window->draw(tooltipText);
    healthBarBackground.setPosition(x - 14 - xCameraOffset, y + 52 - yCameraOffset);
    window->draw(healthBarBackground);
    healthBar.setPosition(x - 12 - xCameraOffset, y + 54 - yCameraOffset);
    window->draw(healthBar);
}

void Creature::attackEntity(Entity* entity) {
    entity->setHealth(entity->getHealth() - attack);
}

void Creature::knockBack(float x, float y) {
    this->x += x;
    this->y += y;
}

void Creature::move(float x, float y) {
    xMove = x;
    yMove = y;
    this->x += x;
    this->y += y;
}

float Creature::getSpeed() {
    return speed;
}

void Creature::setSpeed(float speed) {
    this->speed = speed;
}

bool Creature::isMoving() {
    return moving;
}

void Creature::setMoving(bool moving) {
    this->moving = moving;
}