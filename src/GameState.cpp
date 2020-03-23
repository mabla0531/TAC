#include "headers/GameState.h"

GameState::GameState() {
    map = new Map("res/maps/map1.map");
}

GameState::~GameState() {

}

void GameState::tick() {

}

void GameState::render(sf::RenderWindow* window) {
    map->render(window);
}
