#include <SDL.h>
#include <stdio.h>

#define INIT_FLAGS SDL_INIT_EVERYTHING
#define WINDOW_WIDTH 640
#define WINDOW_HEIGHT 480
#define WINDOW_FLAGS 0

int Run(Uint32 initFlags, int windowWidth,int windowheight, Uint32 windowFlags, void* (*contextFactory)(), void (*renderFunc)(SDL_Renderer*, const void*), void (*handleEvent)(const SDL_Event*, void*))
{
	SDL_Init(initFlags);

	SDL_Window* window;
	SDL_Renderer* renderer;

	SDL_CreateWindowAndRenderer(windowWidth, windowheight, windowFlags, &window, &renderer);

	void* context = contextFactory ? contextFactory() : 0;
	SDL_Event event = { 0 };
	for (;;)
	{
		SDL_RenderClear(renderer);
		if (renderFunc)
		{
			renderFunc(renderer, context);
		}
		SDL_RenderPresent(renderer);
		if (SDL_WaitEvent(&event))
		{
			if (event.type == SDL_QUIT)
			{
				break;
			}
			else if(handleEvent)
			{
				handleEvent(&event, context);
			}
		}
		else
		{
			break;
		}
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

void* ContextFactory()
{
	return 0;
}

void RenderFunc(SDL_Renderer* renderer, const void* context)
{

}

void HandleEvent(const SDL_Event* event, void* context)
{

}

int main(int argc, char** argv)
{
	return Run(INIT_FLAGS, WINDOW_WIDTH, WINDOW_HEIGHT, WINDOW_FLAGS, ContextFactory, RenderFunc, HandleEvent);
}