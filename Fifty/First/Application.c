#include "Application_internals.h"

struct Application* Application_ctor(struct Application* _this)
{
	if (_this)
	{
		_this->run_function = 0;
	}

	return _this;
}

void Application_dtor(struct Application* _this, SDL_bool free_memory)
{
	if (_this && free_memory != SDL_FALSE)
	{
		SDL_free(_this);
	}
}

void Application_run(struct Application* _this)
{
	if (_this && _this->run_function)
	{
		_this->run_function(_this);
	}
}
