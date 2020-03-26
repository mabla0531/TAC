#include "headers/Player.h"

Player::Player() {

}

Player::Player(int x, int y) : Creature(x, y) {

    sf::IntRect standardRects[4] = {sf::IntRect(32, 0, 32, 32), sf::IntRect(64, 0, 32, 32), sf::IntRect(32, 0, 32, 32), sf::IntRect(0, 0, 32, 32)};
    sf::IntRect upRects[4] = {sf::IntRect(32, 96, 32, 32), sf::IntRect(64, 96, 32, 32), sf::IntRect(32, 96, 32, 32), sf::IntRect(0, 96, 32, 32)};
    sf::IntRect leftRects[4] = {sf::IntRect(32, 32, 32, 32), sf::IntRect(64, 32, 32, 32), sf::IntRect(32, 32, 32, 32), sf::IntRect(0, 32, 32, 32)};
    sf::IntRect rightRects[4] = {sf::IntRect(32, 64, 32, 32), sf::IntRect(64, 64, 32, 32), sf::IntRect(32, 64, 32, 32), sf::IntRect(0, 64, 32, 32)};

    sf::Texture* texture;
    texture = new sf::Texture();

    texture->loadFromFile("res/entity.png");
    up = Animation(texture, upRects, 4);
    standard = Animation(texture, standardRects, 4);
    left = Animation(texture, leftRects, 4);
    right = Animation(texture, rightRects, 4);
    staticSprite = sf::Sprite(*texture, sf::IntRect(32, 0, 32, 32));

    currentAnimation = &standard;

    speed = 1.0f;
}

Player::~Player() {

}

void Player::tick() {
    float xMove = 0, yMove = 0;
    
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::LShift)) {
        speed = 1.5f;
    } else {
        speed = 1.0f;
    }

    if (sf::Keyboard::isKeyPressed(sf::Keyboard::W)) {
        yMove -= speed;
    }
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::S)) {
        yMove += speed;
    }
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::A)) {
        xMove -= speed;
    }
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::D)) {  
        xMove += speed;
    }
    
    if (yMove != 0 || xMove != 0) moving = true;
    else moving = false;

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
    
    //finally, move the player
    move(xMove, yMove);
}
