#include "headers/Entity.h"
#include <iostream>

Entity::Entity(int x, int y) {
    this->x = x;
    this->y = y;

    width = 32;
    height = 32;

    hitbox.left = 0;
    hitbox.top = 0;
    hitbox.width = 32;
    hitbox.height = 32;
    
    defaultFont.loadFromFile("res/fonts/Romulus.ttf");
    tooltipText = sf::Text("Hello", defaultFont);
    tooltipText.setScale(sf::Vector2f(0.5f, 0.5f));
    tooltipText.setPosition(x, y + 36);
    tooltip = sf::RectangleShape(sf::Vector2f(64, 32));
    tooltip.setFillColor(sf::Color(105, 105, 105));
    tooltip.setPosition(x - 16, y + 40);

    maxHealth = 10;
    health = maxHealth;
}

Entity::Entity() {

}

Entity::~Entity() {
    
}

float Entity::getX() {
    return x;
}

float Entity::getY() {
    return y;
}

int Entity::getHealth() {
    return health;
}

void Entity::setHealth(int health) {
    this->health = health;
}

sf::Sprite Entity::getStaticSprite() {
    return staticSprite;
}

sf::IntRect Entity::getHitbox() {
    return hitbox;
}

Animation* Entity::getCurrentAnimation() {
    return currentAnimation;
}

bool Entity::checkXCollision(Entity* entity, int xMove) {
    sf::IntRect entityHitbox = entity->getHitbox();
    if (sf::IntRect(this->x + hitbox.left + xMove, this->y + hitbox.top, hitbox.width, hitbox.height).intersects(sf::IntRect(entity->getX() + entityHitbox.left, entity->getY() + entityHitbox.top, entityHitbox.width, entityHitbox.height))) {
        return true;
    }
    return false;
}

bool Entity::checkYCollision(Entity* entity, int yMove) {
    sf::IntRect entityHitbox = entity->getHitbox();
    if (sf::IntRect(this->x + hitbox.left, this->y + hitbox.top + yMove, hitbox.width, hitbox.height).intersects(sf::IntRect(entity->getX() + entityHitbox.left, entity->getY() + entityHitbox.top, entityHitbox.width, entityHitbox.height))) {
        return true;
    }
    return false;
}