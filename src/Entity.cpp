#include "headers/Entity.h"

Entity::Entity(int x, int y) {
    this->x = x;
    this->y = y;
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