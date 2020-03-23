#pragma once

#include <SFML/Graphics.hpp>

class State {
    public:
    virtual void tick() = 0;
    virtual void render(sf::RenderWindow* window) = 0;
};