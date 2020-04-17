#include "headers/Player.h"

Player::Player() {

}

Player::Player(int x, int y, sf::RenderWindow* window, int* xCameraOffset, int* yCameraOffset) : Creature(x, y) {
    this->window = window;

    this->xCameraOffset = xCameraOffset;
    this->yCameraOffset = yCameraOffset;

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

void Player::tick(std::vector<Entity*>* entities) {

    //check for attacking (clicking)
    std::for_each(entities->begin(), entities->end(), [this](Entity* entity) {
        if (sf::IntRect(entity->getX() - *xCameraOffset, entity->getY() - *yCameraOffset, 32, 32).contains(sf::Mouse::getPosition(*window)) && sf::Mouse::isButtonPressed(sf::Mouse::Left) && attackCooldown.getElapsedTime().asMilliseconds() >= 500) {
            attackEntity(entity);
            
            float knockBackX = 0.0f, knockBackY = 0.0f;
            if (entity->getX() > x && entity->getY() > y - 16 && entity->getY() < y + 16) {
                knockBackX = 16.0f;
            } else if (entity->getX() < x && entity->getY() > y - 16 && entity->getY() < y + 16) {
                knockBackX = -16.0f;
            } else if (entity->getY() > y && entity->getX() > x - 16 && entity->getX() < x + 16) {
                knockBackY = 16.0f;
            } else if (entity->getY() < y && entity->getX() > x - 16 && entity->getX() < x + 16) {
                knockBackY = -16.0f;
            } else {
                if (entity->getX() > x)
                    knockBackX = 16.0f;
                if (entity->getX() < x)
                    knockBackX = -16.0f;
                if (entity->getY() > y)
                    knockBackY = 16.0f;
                if (entity->getY() < y)
                    knockBackY = -16.0f;
            }
            entity->knockBack(knockBackX, knockBackY);
            attackCooldown.restart();
        }
    });

    //set xMove and yMove to zero to be set later
     xMove = 0.0f;
     yMove = 0.0f;
    
    //if shift, then run
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::LShift)) {
        speed = 1.75f;
        animationCycleDelay = 125;
    } else {
        speed = 1.0f;
        animationCycleDelay = 200;
    }

    //do keyboard input
                                // TODO refactor to use angle based proximity
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

    //check collisions
    for (int i = 0; i < entities->size(); i++) {
        if (entities->at(i) != this && checkXCollision(entities->at(i), xMove))
            xMove = 0.0f;
        if (entities->at(i) != this && checkYCollision(entities->at(i), yMove))
            yMove = 0.0f;
    }
    
    //if there's any moving done at all then set the bool to true
    if (yMove != 0 || xMove != 0) moving = true;
    else moving = false;

    //tick the respective animation
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

    if (animationLimiter.getElapsedTime().asMilliseconds() > animationCycleDelay) {
        animationLimiter.restart();
        currentAnimation->cycleFrame(); 
    }

    //finally, move
    move(xMove, yMove);
}

bool Player::addItem(InventoryItem item) {
    if (weight + (item.getWeight() * item.getCount()) <= carryCapacity) {
        inventory.push_back(item);
        weight += (item.getWeight() * item.getCount());
        return true;
    }
    return false;
}

