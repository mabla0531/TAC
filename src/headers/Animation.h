#pragma once

#include <SFML/Graphics.hpp>

class Animation {

    private:
    sf::Sprite* sprite;
    int spriteIndex, spriteFrames;

    public:
    Animation();
    Animation(sf::Texture* texture, sf::IntRect* textureRects, int frameCount);
    ~Animation();

    void cycleFrame();
    void resetFrame();
    void render(sf::RenderWindow* window, int x, int y);

    void setLocation(int x, int y);
    sf::Sprite* getFrames();

};