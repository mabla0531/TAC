#pragma once
#include "Entity.h"

class Creature : public Entity {

    protected:
    bool moving;
    float speed;

    public:
    Creature(int x, int y);
    Creature();
    ~Creature();

    virtual void tick();
    virtual void render(sf::RenderWindow* window, int xCameraOffset, int yCameraOffset);

    void move(float x, float y);
    
    float getX();
    float getY();

};