#pragma once
#include "Animation.h"

class Entity {

    protected:
    float x, y;
    int width, height;
    int health, maxHealth;
    Animation* currentAnimation;
    sf::Sprite staticSprite;
    
    sf::IntRect hitbox;

    sf::Font defaultFont;
    sf::Text tooltipText;
    sf::RectangleShape tooltip;
    sf::RectangleShape healthBar;
    sf::RectangleShape healthBarBackground;

    public:
    Entity(int x, int y);
    Entity();
    ~Entity();

    virtual void knockBack(float x, float y) = 0;

    virtual void tick(std::vector<Entity*>* entities) = 0;
    virtual void render(sf::RenderWindow* window, int xCameraOffset, int yCameraOffset) = 0;
    virtual void renderToolTip(sf::RenderWindow* window, int xCameraOffset, int yCameraOffset) = 0;
    
    float getX();
    float getY();
    
    int getHealth();
    void setHealth(int health);

    sf::Sprite getStaticSprite();
    sf::IntRect getHitbox();
    Animation* getCurrentAnimation();

    
    bool checkXCollision(Entity* entity, int xMove);
    bool checkYCollision(Entity* entity, int yMove);
};