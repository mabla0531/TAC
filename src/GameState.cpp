#include "headers/definitions.h"
#include "headers/GameState.h"

GameState::GameState() {
    map = new Map("res/maps/map1.map");
    player = new Player(10, 10);
}

GameState::~GameState() {

}

void GameState::tick() {
    centerCameraOnPlayer();
    trimCameraEdges();
    map->tick();
    player->tick();
}

void GameState::render(sf::RenderWindow* window) {
    map->render(window, xCameraOffset, yCameraOffset);
    player->render(window, xCameraOffset, yCameraOffset);
}

void GameState::centerCameraOnPlayer() {
    xCameraOffset = player->getX() - (WINDOW_WIDTH / 2);
    yCameraOffset = player->getY() - (WINDOW_HEIGHT / 2);
}

void GameState::trimCameraEdges() {
    if (xCameraOffset < 0) xCameraOffset = 0;
    if (xCameraOffset > (map->getWidth() * 32) - WINDOW_WIDTH) xCameraOffset = ((map->getWidth() * 32) - WINDOW_WIDTH);
    if (yCameraOffset < 0) yCameraOffset = 0;
    if (yCameraOffset > (map->getHeight() * 32) - WINDOW_HEIGHT) yCameraOffset = ((map->getHeight() * 32) - WINDOW_HEIGHT);
}
