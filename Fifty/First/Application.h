#ifndef APPLICATION_H
#define APPLICATION_H

#include <SDL.h>

#ifdef __cplusplus
extern "C"{
#endif

	struct Application;

	struct Application* Application_ctor(struct Application* _this);
	void Application_dtor(struct Application* _this, SDL_bool free_memory);
	void Application_run(struct Application* _this);

	typedef struct Application Application;

#ifdef __cplusplus
}
#endif

#endif