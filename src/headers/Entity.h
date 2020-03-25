#pragma once
#include "Animation.h"

class Entity {

    protected:
    float x, y;
    Animation* currentAnimation;
    sf::Sprite staticSprite;

    public:
    Entity(int x, int y);
    Entity();
    ~Entity();

    virtual void tick() = 0;
    virtual void render(sf::RenderWindow* window, int xCameraOffset, int yCameraOffset) = 0;
};