#include "headers/definitions.h"
#include "headers/GameState.h"

GameState::GameState(sf::RenderWindow* window) {
    this->window = window;

    map = Map("res/maps/map1.map");
    player = new Player(10, 10, window, &xCameraOffset, &yCameraOffset);

    // Test Enemy
    entities.push_back(new Enemy(100, 200, player));
    entities.push_back(player);

    // Test Item
    sf::Texture* texture = new sf::Texture();
    texture->loadFromFile("res/item.png");
    int mods[3] = {1, 1, 1};
    items.push_back(new GroundItem(200, 200, texture, sf::IntRect(0, 0, 16, 16), mods, false, 2, 1.0f));
}

GameState::~GameState() {

}

void GameState::sortEntitiesByY() { // Sort all entities by their Y axis
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
        [this](Entity* entity) {
            entity->tick(&entities);
        }
    );
}

void GameState::renderTooltips() {
    for (int i = 0; i < items.size(); i++) {
        if (sf::IntRect(items.at(i)->getX() - xCameraOffset, items.at(i)->getY() - yCameraOffset, 32, 32).contains(sf::Mouse::getPosition(*window)))
                // TODO refactor to use angle based proximity
            items.at(i)->renderToolTip(window, xCameraOffset, yCameraOffset);
    }
    for (int i = 0; i < entities.size(); i++) {
        if (entities.at(i) != player) {
        if (sf::IntRect(entities.at(i)->getX() - xCameraOffset, entities.at(i)->getY() - yCameraOffset, 32, 32).contains(sf::Mouse::getPosition(*window)))
            entities.at(i)->renderToolTip(window, xCameraOffset, yCameraOffset);
        }
    }
}

void GameState::render() {
    map.render(window, xCameraOffset, yCameraOffset);

    renderTooltips();

    for (int i = 0; i < items.size(); i++) {
        items.at(i)->render(window, xCameraOffset, yCameraOffset);
    }

    std::for_each(entities.begin(), entities.end(),
        [this](Entity* entity) {
        entity->render(window, xCameraOffset, yCameraOffset);
    });
}

void GameState::centerCameraOnPlayer() { // Center GameCamera on the player
    xCameraOffset = player->getX() - (WINDOW_WIDTH / 2);
    yCameraOffset = player->getY() - (WINDOW_HEIGHT / 2);
}

void GameState::trimCameraEdges() { // Ensure the GameCamera isn't rendering anything off the map (so any black)
    if (xCameraOffset < 0) xCameraOffset = 0;
    if (xCameraOffset > (map.getWidth() * 32) - WINDOW_WIDTH) xCameraOffset = ((map.getWidth() * 32) - WINDOW_WIDTH);
    if (yCameraOffset < 0) yCameraOffset = 0;
    if (yCameraOffset > (map.getHeight() * 32) - WINDOW_HEIGHT) yCameraOffset = ((map.getHeight() * 32) - WINDOW_HEIGHT);
}

void GameState::addItem(GroundItem* groundItem) {
    items.push_back(groundItem);
}

void GameState::removeItem(GroundItem* groundItem) {
    for (int i = 0; i < items.size(); i++) {
        if (items.at(i) == groundItem) {
            items.erase(items.begin() + i);
        }
    }
}
