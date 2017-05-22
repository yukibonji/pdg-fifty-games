#ifndef SDLAPPLICATION_INTERNALS_H
#define SDLAPPLICATION_INTERNALS_H

#include "SDLApplication.h"

#ifdef __cplusplus
extern "C" {
#endif

#include "Application_internals.h"

	struct SDLApplication {
		Application base;
		SDL_Window* window;
		SDL_Renderer* renderer;

	};



#ifdef __cplusplus
}
#endif

#endif