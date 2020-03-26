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

    texture->loadFromFile("res/entity.png");
    up = Animation(texture, upRects, 4);
    standard = Animation(texture, standardRects, 4);
    left = Animation(texture, leftRects, 4);
    right = Animation(texture, rightRects, 4);
    staticSprite = sf::Sprite(*texture, sf::IntRect(224, 128, 32, 32));

    currentAnimation = &standard;

    this->player = player;
    speed = 1.0f;
}

Enemy::~Enemy() {

}

void Enemy::tick() {

    if (player->getX() < x + TILE_SIZE + 100 && player->getX() > x - 100 && player->getY() < y + TILE_SIZE + 100 && player->getY() > y - 100) { //if the player is close
        moving = true;
        float xMove = 0.0f, yMove = 0.0f;

        //move towards the player
        if ((int)player->getX() < (int)x) xMove = -speed;
        else if ((int)player->getX() > (int)x) xMove = speed;
    
        if ((int)player->getY() < (int)y) yMove = -speed;
        else if ((int)player->getY() > (int)y) yMove = speed;

        if (yMove < 0) { //if the enemy is moving anywhere up
            if (currentAnimation != &up) up.resetFrame(); //if the animation is being switched then reset the new animation's frame index
            currentAnimation = &up;
        } else if (yMove > 0) { //if the enemy is moving anywhere down
            if (currentAnimation != &standard) standard.resetFrame(); //if the animation is being switched then reset the new animation's frame index
            currentAnimation = &standard;
        } else if (xMove < 0 && yMove == 0) { //if the enemy is moving directly left
            if (currentAnimation != &left) left.resetFrame(); //if the animation is being switched then reset the new animation's frame index
            currentAnimation = &left;
        } else if (xMove > 0 && yMove == 0) { //if the enemy is moving directly right
            if (currentAnimation != &right) right.resetFrame(); //if the animation is being switched then reset the new animation's frame index
            currentAnimation = &right;
        } else { //if something messed up but we still need to animate then just use the standard
            if (currentAnimation != &standard) standard.resetFrame(); //if the animation is being switched then reset the new animation's frame index
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