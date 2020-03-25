#include <SFML/Graphics.hpp>
#include "Animation.h"
class Player {
    
    private:
    int x, y;
    sf::Clock animationLimiter;
    Animation standard, up, left, right;
    Animation* currentAnimation;
    bool moving;
    
    public:
    Player(int x, int y);
    ~Player();

    void move(int x, int y);
    void tick();
    void render(sf::RenderWindow* window, int xCameraOffset, int yCameraOffset);

    int getX();
    int getY();
};
