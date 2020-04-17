#include "headers/definitions.h"
#include "headers/Enemy.h"

Enemy::Enemy() {

}

Enemy::Enemy(int x, int y, Player* player) : Creature(x, y) {

    //load the rectangles to take from the texture
    sf::IntRect standardRects[4] = {sf::IntRect(224, 128, 32, 32), sf::IntRect(256, 128, 32, 32), sf::IntRect(224, 128, 32, 32), sf::IntRect(192, 128, 32, 32)};
    sf::IntRect upRects[4] = {sf::IntRect(224, 224, 32, 32), sf::IntRect(256, 224, 32, 32), sf::IntRect(224, 224, 32, 32), sf::IntRect(192, 224, 32, 32)};
    sf::IntRect leftRects[4] = {sf::IntRect(224, 160, 32, 32), sf::IntRect(256, 160, 32, 32), sf::IntRect(224, 160, 32, 32), sf::IntRect(192, 160, 32, 32)};
    sf::IntRect rightRects[4] = {sf::IntRect(224, 192, 32, 32), sf::IntRect(256, 192, 32, 32), sf::IntRect(224, 192, 32, 32), sf::IntRect(192, 192, 32, 32)};
    sf::Texture* texture;
    texture = new sf::Texture();

    //load the texture and animations accordingly
    texture->loadFromFile("res/entity.png");
    up = Animation(texture, upRects, 4);
    standard = Animation(texture, standardRects, 4);
    left = Animation(texture, leftRects, 4);
    right = Animation(texture, rightRects, 4);
    staticSprite = sf::Sprite(*texture, sf::IntRect(224, 128, 32, 32));

    //set the current animation to be used to the standard animation (downward-facing)
    currentAnimation = &standard;

    this->player = player;
    speed = 1.0f;
}

Enemy::~Enemy() {

}

void Enemy::tick(std::vector<Entity*>* entities) {
    healthBar.setSize(sf::Vector2f((float)health / (float)maxHealth * 56, 4));

    if (player->getX() < x + TILE_SIZE + 100 && player->getX() > x - 100 && player->getY() < y + TILE_SIZE + 100 && player->getY() > y - 100) { //if the player is close
        moving = true;
        float xMove = 0.0f, yMove = 0.0f;

        //move towards the player
                                                                // TODO refactor to use angle based proximity
        if ((int)player->getX() < (int)x) xMove = -speed;
        else if ((int)player->getX() > (int)x) xMove = speed;
    
        if ((int)player->getY() < (int)y) yMove = -speed;
        else if ((int)player->getY() > (int)y) yMove = speed;

        //check entity collisions
        for (int i = 0; i < entities->size(); i++) {
            if (entities->at(i) != this && checkXCollision(entities->at(i), xMove))
                xMove = 0.0f;
            if (entities->at(i) != this && checkYCollision(entities->at(i), yMove))
                yMove = 0.0f;
        }

        //tick respective animation
        if (yMove < 0) {
            if (currentAnimation != &up) up.resetFrame();
            currentAnimation = &up;
        } else if (yMove > 0) {
            if (currentAnimation != &standard) standard.resetFrame();
            currentAnimation = &standard;
        } else if (xMove < 0 && yMove == 0) {
            if (currentAnimation != &left) left.resetFrame();
            currentAnimation = &left;
        } else if (xMove > 0 && yMove == 0) {
            if (currentAnimation != &right) right.resetFrame();
            currentAnimation = &right;
        } else {
            if (currentAnimation != &standard) standard.resetFrame();
            currentAnimation = &standard;
        }

        if (animationLimiter.getElapsedTime().asMilliseconds() > 200) {
            animationLimiter.restart();
            currentAnimation->cycleFrame(); 
        }

        move(xMove, yMove);
    } else {
        moving = false;
    }
}