#ifndef SDLAPPLICATION_H
#define SDLAPPLICATION_H

#ifdef __cplusplus
extern "C" {
#endif
	
#include <SDL.h>

	struct SDLApplication;

	struct SDLApplication* SDLApplication_ctor(struct SDLApplication* _this, Uint32 init_flags, int window_width, int window_height);
	void SDLApplication_dtor(struct SDLApplication* _this, SDL_bool free_memory);

	void SDLApplication_run(struct SDLApplication* _this);

	typedef SDLApplication SDLApplication;


#ifdef __cplusplus
}
#endif

#endif