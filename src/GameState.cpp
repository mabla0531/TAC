#include "headers/GameState.h"

GameState::GameState() {
    map = new Map("res/maps/map1.map");
    player = new Player(10, 10);
}

GameState::~GameState() {

}

void GameState::tick() {
    map->tick();
    player->tick();
}

void GameState::render(sf::RenderWindow* window) {
    map->render(window);
    player->render(window);
}
