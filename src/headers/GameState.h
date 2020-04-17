#pragma once
#include <vector>
#include "State.h"
#include "Map.h"
#include "Player.h"
#include "Enemy.h"
#include "GroundItem.h"
#include "InventoryItem.h"

class GameState : public State {

    protected:
    Map map;
    Player* player;
    std::vector<Entity*> entities;
    std::vector<GroundItem*> items;

    int xCameraOffset, yCameraOffset;

    public:
    GameState(sf::RenderWindow* window);
    ~GameState();

    void sortEntitiesByY();

    void tick();
    void render();
    void renderTooltips();

    void centerCameraOnPlayer();
    void trimCameraEdges();

    void addItem(GroundItem* groundItem);
    void removeItem(GroundItem* groundItem);
};
