#pragma once
#include <string>
#include "State.h"
#include "Map.h"

class GameState : public State {

    private:
    Map* map;

    public:
    GameState();
    ~GameState();

    void tick();
    void render(sf::RenderWindow* window);

};
