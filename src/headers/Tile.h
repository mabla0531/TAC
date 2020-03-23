#pragma once

#include <SFML/Graphics.hpp>

class Tile {

    static const sf::Vector2i grassTile, dirtTile;

    private:
    int x, y;
    bool solid;
    sf::Sprite sprite;
    
    public:
    enum Type {
        GRASS,
        STONE,
        DIRT,
        WOOD
    };

    Tile(int x, int y, Type type, sf::Texture* textureMap);
    Tile();
    ~Tile();

    void render(sf::RenderWindow* window);

    int getX();
    int getY();
    bool isSolid();
    sf::Sprite getSprite();
    
};
