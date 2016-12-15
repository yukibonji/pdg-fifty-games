#ifndef GAME_H
#define GAME_H

#include "SDL.h"
#include "common_structs.h"

#ifdef __cplusplus
extern "C" {
#endif

	Game* Game_Create();
	void Game_Initialize(Game* _this, int argc, char** argv);
	void Game_Loop(Game* _this);
	int Game_CleanUp(Game* _this);//TODO: combine cleanup and destroy?
	void Game_Destroy(Game* _this);

#ifdef __cplusplus
}
#endif

#endif