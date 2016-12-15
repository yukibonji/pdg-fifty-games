#include "state_descriptor.h"

void StateDescriptor_Initialize(StateDescriptor* _this, event_handler_func handleEvent, update_func update, draw_func draw, void* context, void(*contextCleanUp)(void*))
{
	SDL_assert(_this);

	_this->Draw = draw;
	_this->Update = update;
	_this->HandleEvent = handleEvent;
	_this->Context = context;
	_this->ContextCleanUp = contextCleanUp;
}

void StateDescriptor_CleanUp(StateDescriptor* _this)
{
	SDL_assert(_this);

	if (_this->ContextCleanUp)
	{
		_this->ContextCleanUp(_this->Context);
		_this->ContextCleanUp = 0;
	}

	_this->Context = 0;
}
