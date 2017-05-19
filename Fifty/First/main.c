#include <SDL.h>
#include <stdio.h>


int main(int argc, char** argv)
{
	int result = SDL_Init(SDL_INIT_EVERYTHING);
	if (result)
	{
		printf("Init Error: %s", SDL_GetError());
		return -1;
	}

	SDL_Window* window = 0;
	SDL_Renderer* renderer = 0;

	result = SDL_CreateWindowAndRenderer(640, 480, 0, &window, &renderer);
	if (result)
	{
		char* c = SDL_GetError();
		printf("Init Error: %s", SDL_GetError());
		return -1;
	}

	SDL_RenderClear(renderer);
	SDL_RenderPresent(renderer);
	SDL_Event event;
	while (SDL_WaitEvent(&event))
	{
		if (event.type == SDL_QUIT)
		{
			break;
		}
		SDL_RenderPresent(renderer);
	}

	if (renderer)
	{
		SDL_DestroyRenderer(renderer);
		renderer = 0;
	}

	if (window)
	{
		SDL_DestroyWindow(window);
		window = 0;
	}

	SDL_Quit();

	return 0;
}