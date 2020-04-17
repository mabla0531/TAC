#pragma once

#include <SFML/Graphics.hpp>

class State {
    protected:
    sf::RenderWindow* window;
    
    public:
    virtual void tick() = 0;
    virtual void render() = 0;
};