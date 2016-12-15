#ifndef STATE_DESCRIPTOR_H
#define STATE_DESCRIPTOR_H

#include "SDL.h"
#include "common_structs.h"

#ifdef __cplusplus
extern "C" {
#endif

	typedef SDL_bool(*event_handler_func)(Game*,void*);
	typedef void(*update_func)(Game*, void*);
	typedef void(*draw_func)(const Game*, void*);

	struct StateDescriptor {
		event_handler_func HandleEvent;
		update_func Update;
		draw_func Draw;
		void* Context;
		void(*ContextCleanUp)(void*);
	};

	void StateDescriptor_Initialize(StateDescriptor* _this, event_handler_func handleEvent, update_func update, draw_func draw, void* context, void(*contextCleanUp)(void*));
	void StateDescriptor_CleanUp(StateDescriptor* _this);


#ifdef __cplusplus
}
#endif

#endif