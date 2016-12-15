#include "SDL.h"
#include "game.h"
#include "constants.h"
#include "utility_macros.h"
#include "state_descriptor.h"

typedef enum {
	STATE_SPLASH,
	STATE_COUNT
} STATE;

struct Game {
	SDL_Event event;
	SDL_Window* window;
	SDL_Renderer* renderer;
	StateDescriptor states[STATE_COUNT];
	STATE state;
};

void SplashState_Draw(const Game* _this, void* context)
{
	SDL_SetRenderDrawColor(_this->renderer, 0, 0, 0, 255);
	SDL_RenderClear(_this->renderer);

	SDL_RenderPresent(_this->renderer);
}

void SplashState_Update(Game* _this, void* context)
{

}

SDL_bool SplashState_HandleEvent(Game* _this, void* context)
{
	switch (_this->event.type)
	{
	case SDL_QUIT:
		return SDL_TRUE;
	default:
		return SDL_FALSE;
	}
}

Game* Game_Create()
{
	Game* _this = NEW(Game);
	SDL_memset(_this, 0, sizeof(Game));

	return _this;
}

void Game_Initialize(Game* _this, int argc, char** argv)
{
	SDL_assert(_this);

	SDL_Init(SDL_INIT_EVENTS | SDL_INIT_VIDEO);

	SDL_CreateWindowAndRenderer(WINDOW_WIDTH, WINDOW_HEIGHT, 0, &_this->window, &_this->renderer);

	StateDescriptor_Initialize(&_this->states[STATE_SPLASH], SplashState_HandleEvent, SplashState_Update, SplashState_Draw, 0, 0);

	_this->state = STATE_SPLASH;

}

SDL_bool Game_HandleEvent(Game* _this)
{
	SDL_assert(_this);
	SDL_assert(_this->states[_this->state].HandleEvent);

	return _this->states[_this->state].HandleEvent(_this, _this->states[_this->state].Context);
}

void Game_Draw(const Game* _this)
{
	SDL_assert(_this);

	_this->states[_this->state].Draw(_this, _this->states[_this->state].Context);
}

void Game_Update(Game* _this)
{
	SDL_assert(_this);

	_this->states[_this->state].Update(_this, _this->states[_this->state].Context);
}

void Game_Loop(Game* _this)
{
	SDL_assert(_this);

	SDL_bool done = SDL_FALSE;
	while (SDL_FALSE == done)
	{
		if (SDL_PollEvent(&_this->event))
		{
			done = Game_HandleEvent(_this);
		}
		else
		{
			Game_Update(_this);
			Game_Draw(_this);
		}
	}
}

int Game_CleanUp(Game* _this)
{
	SDL_assert(_this);

	for (int index = 0; index < STATE_COUNT; ++index)
	{
		StateDescriptor_CleanUp(&_this->states[index]);
	}

	SAFE_DESTROY(_this->renderer, SDL_DestroyRenderer);
	SAFE_DESTROY(_this->window, SDL_DestroyWindow);

	SDL_Quit();

	return 0;
}

void Game_Destroy(Game* _this)
{
	SDL_assert(_this);

	SDL_free(_this);
}
