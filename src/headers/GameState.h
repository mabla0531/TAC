#pragma once
#include <vector>
#include "State.h"
#include "Map.h"
#include "Player.h"
#include "Enemy.h"

class GameState : public State {

    private:
    Map map;
    Player* player;
    std::vector<Entity*> entities;

    int xCameraOffset, yCameraOffset;

    public:
    GameState();
    ~GameState();

    void sortEntitiesByY();

    void tick();
    void render(sf::RenderWindow* window);

    void centerCameraOnPlayer();
    void trimCameraEdges();

};
