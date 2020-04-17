#pragma once
#include "Entity.h"

class Creature : public Entity {

    protected:
    bool moving;
    float xMove, yMove;
    float speed;
    int defense, attack;
    const char* name;
    sf::Clock attackCooldown;
    Animation standard;
    sf::Clock animationLimiter;

    public:
    Creature(int x, int y);
    Creature();
    ~Creature();

    virtual void tick();
    virtual void render(sf::RenderWindow* window, int xCameraOffset, int yCameraOffset);
    virtual void renderToolTip(sf::RenderWindow* window, int xCameraOffset, int yCameraOffset);
    void attackEntity(Entity* entity);
    void knockBack(float x, float y);

    void move(float x, float y);

    float getSpeed();
    void setSpeed(float speed);

    bool isMoving();
    void setMoving(bool moving);
};