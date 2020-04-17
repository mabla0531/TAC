#include "headers/definitions.h"
#include <SFML/Graphics.hpp>
#include "headers/GameState.h"

sf::RenderWindow window(sf::VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT), "The Alpha Chronicles");
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
    gameState->render();

    //display window
    window.display();
}

int main() {
    gameState = new GameState(&window);

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
