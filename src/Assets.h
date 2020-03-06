#pragma once

#include <SDL2/SDL.h>
#include <SDL2/SDL_image.h>
#include <string>

SDL_Surface* surface;
SDL_Texture* testTexture;

void initAssets(SDL_Renderer* renderer) {
    IMG_Init(IMG_INIT_PNG);
    surface = IMG_Load("img.png");
    testTexture = SDL_CreateTextureFromSurface(renderer, surface);
    SDL_FreeSurface(surface);
}