#pragma once

#include <SDL2/SDL.h>

class State {
    private:


    public:
    virtual void tick() = 0;
    virtual void render(SDL_Renderer* renderer) = 0;
};