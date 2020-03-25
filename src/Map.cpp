#include "headers/Map.h"
#include <fstream>

Map::Map(const char* path) {

    sf::Texture* textureMap = new sf::Texture();
    textureMap->loadFromFile("res/terrain.png");

    //storage int for current read tile, ifstream
    int tile;
    std::ifstream in;
    in.open(path);

    //read width and height from text file
    in >> width;
    in >> height;
    
    //create tile array
    tiles = new Tile[width * height];

    //for loop-specific vars
    Tile::Type type;
    int tileCounter = 0;

    //read and init every tile
    for (int y = 0; y < height; y++) {
        for (int x = 0; x < width; x++) {
            in >> tile;
            switch (tile) {
                case 0:
                type = Tile::Type::DIRT;
                break;

                case 1:
                type = Tile::Type::GRASS;
                break;

                case 2:
                type = Tile::Type::STONE;
                break;

                case 3:
                type = Tile::Type::WOOD;
                break;

                default:
                type = Tile::Type::GRASS;
            }
            tiles[tileCounter] = Tile(x, y, type, textureMap);
            tileCounter++;
        }
    }
}

Map::~Map() {
    
}

void Map::tick() {

}

void Map::render(sf::RenderWindow* window) {
    for (int i = 0; i < (width * height); i++) {
        tiles[i].render(window);
    }
}

Tile* Map::getTiles() {
    return tiles;
}

int Map::getWidth() {
    return width;
}

int Map::getHeight() {
    return height;
}
