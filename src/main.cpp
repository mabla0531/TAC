/*
 * The Alpha Chronicles, made in 2020 by Matthew Bland
 * 
 * This is my source code. There are many like it,
 * but this one is mine. My source code is my friend. 
 * It is my life. I  must master it as I master my life.
 * Without me, my source code is useless. Without my
 * source code, I am useless. I must write my source
 * code true. I must write more concisely than my enemy
 * who is trying to outperform me. I must write code
 * before he writes code. I WILL. 
 */

#include <SDL2/SDL.h>
#include <iostream>
#include "Assets.h"

SDL_Window* window;
SDL_Renderer* renderer;
SDL_Texture* texture;
bool running = false, paused = false;
SDL_Event event;

void handleEvents() {
    while (SDL_PollEvent(&event)) {
        if (event.type == SDL_QUIT) {
            running = false;
        } else if (event.window.event == SDL_WINDOWEVENT_MINIMIZED) {
            paused = true;
        } else if (event.window.event == SDL_WINDOWEVENT_RESTORED) {
            paused = false;
        }
    }
}

void tick() {
    if (!paused) {
        
    }
}

void render() {
    SDL_RenderClear(renderer);
    SDL_RenderCopy(renderer, texture, NULL, NULL);
    SDL_RenderPresent(renderer);
}

void gameLoop() {
    while (running) {
        handleEvents();
        tick();
        render();
    }
}

int main(int args, char* argv[]) {
    SDL_Init(SDL_INIT_VIDEO);

    window = SDL_CreateWindow("The Alpha Chronicles", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, 800, 600, SDL_WINDOW_SHOWN);
    renderer = SDL_CreateRenderer(window, -1, SDL_RENDERER_ACCELERATED);
    
    SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);
    
    initAssets(renderer);
    texture = testTexture;
    running = true;
    gameLoop();

    SDL_DestroyWindow(window);
    SDL_Quit();
    return 0;
}