#pragma once
#include <string>
#include "State.h"
#include "Map.h"
#include "Player.h"

class GameState : public State {

    private:
    Map* map;
    Player* player;

    int xCameraOffset, yCameraOffset;

    public:
    GameState();
    ~GameState();

    void tick();
    void render(sf::RenderWindow* window);

    void centerCameraOnPlayer();
    void trimCameraEdges();

};
