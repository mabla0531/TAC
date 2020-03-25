#include <SFML/Graphics.hpp>
#include "headers/GameState.h"
#include <iostream>

sf::RenderWindow window(sf::VideoMode(1280, 720), "The Alpha Chronicles");
State* gameState;
sf::Clock fpsLimiter;

void tick() {

    //poll events for window
    sf::Event event;
    while (window.pollEvent(event))
    {
        if (event.type == sf::Event::Closed)
            window.close();
    }

    gameState->tick();
}

void render() {

    //clear window 
    window.clear();
    
    //render here
    gameState->render(&window);

    //display window
    window.display();
}

int main() {
    gameState = new GameState();

    while (window.isOpen())
    {
        //tick 100 times per second
        if (fpsLimiter.getElapsedTime().asMilliseconds() > 10) {
            tick();
            fpsLimiter.restart();
        }

        //render everything
        render();
    }

    return 0;
}
