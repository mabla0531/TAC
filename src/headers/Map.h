#pragma once

#include <string>
#include "Tile.h"

class Map {
    private:
    int width, height;
    Tile* tiles;

    public:
    Map(const char* path);
    ~Map();

    void tick();
    void render(sf::RenderWindow* window, int xCameraOffset, int yCameraOffset);

    Tile* getTiles();

    int getWidth();
    int getHeight();
};
