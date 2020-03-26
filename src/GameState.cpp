#include "headers/definitions.h"
#include "headers/GameState.h"

GameState::GameState() {
    map = Map("res/maps/map1.map");
    player = new Player(10, 10);

    entities.push_back(new Enemy(100, 100, player));
    entities.push_back(player);
}

GameState::~GameState() {

}

void GameState::sortEntitiesByY() {
    std::sort(entities.begin(), entities.end(),
        [](Entity* entity, Entity* entity1) {
            return entity->getY() < entity1->getY();
        }
    );
}

void GameState::tick() {
    centerCameraOnPlayer();
    trimCameraEdges();
    map.tick();

    sortEntitiesByY();

    std::for_each(entities.begin(), entities.end(),
        [](Entity* entity) {
            entity->tick();
        }
    );
}

void GameState::render(sf::RenderWindow* window) {
    map.render(window, xCameraOffset, yCameraOffset);

    std::for_each(entities.begin(), entities.end(),
        [this, window](Entity* entity) {
            entity->render(window, xCameraOffset, yCameraOffset);
        }
    );
}

void GameState::centerCameraOnPlayer() {
    xCameraOffset = player->getX() - (WINDOW_WIDTH / 2);
    yCameraOffset = player->getY() - (WINDOW_HEIGHT / 2);
}

void GameState::trimCameraEdges() {
    if (xCameraOffset < 0) xCameraOffset = 0;
    if (xCameraOffset > (map.getWidth() * 32) - WINDOW_WIDTH) xCameraOffset = ((map.getWidth() * 32) - WINDOW_WIDTH);
    if (yCameraOffset < 0) yCameraOffset = 0;
    if (yCameraOffset > (map.getHeight() * 32) - WINDOW_HEIGHT) yCameraOffset = ((map.getHeight() * 32) - WINDOW_HEIGHT);
}
