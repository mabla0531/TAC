#include "headers/Player.h"
#include <iostream>

Player::Player(int x, int y) {

    sf::IntRect standardRects[4] = {sf::IntRect(32, 0, 32, 32), sf::IntRect(64, 0, 32, 32), sf::IntRect(32, 0, 32, 32), sf::IntRect(0, 0, 32, 32)};
    sf::IntRect upRects[4] = {sf::IntRect(32, 96, 32, 32), sf::IntRect(64, 96, 32, 32), sf::IntRect(32, 96, 32, 32), sf::IntRect(0, 96, 32, 32)};
    sf::IntRect leftRects[4] = {sf::IntRect(32, 32, 32, 32), sf::IntRect(64, 32, 32, 32), sf::IntRect(32, 32, 32, 32), sf::IntRect(0, 32, 32, 32)};
    sf::IntRect rightRects[4] = {sf::IntRect(32, 64, 32, 32), sf::IntRect(64, 64, 32, 32), sf::IntRect(32, 64, 32, 32), sf::IntRect(0, 64, 32, 32)};
    up = Animation("res/entity.png", upRects, 4);
    standard = Animation("res/entity.png", standardRects, 4);
    left = Animation("res/entity.png", leftRects, 4);
    right = Animation("res/entity.png", rightRects, 4);
    
    currentAnimation = &standard;

    this->x = x;
    this->y = y;
}

Player::~Player() {

}

void Player::move(int x, int y) {
    this->x += x;
    this->y += y;
}

void Player::tick() {
    int xMove = 0, yMove = 0;
    
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::W)) {
        yMove--;
    }
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::S)) {
        yMove++;
    }
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::A)) {
        xMove--;
    }
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::D)) {  
        xMove++;
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

void Player::render(sf::RenderWindow* window) {
    if (moving)
        currentAnimation->render(window, x, y);
    else {
        standard.getFrames()[0].setPosition(x, y);
        window->draw(standard.getFrames()[0]);
    }
}
