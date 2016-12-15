#include "game.h"

int main(int argc, char** argv)
{
	Game* game = Game_Create();

	Game_Initialize(game, argc, argv);
	Game_Loop(game);
	int result = Game_CleanUp(game);

	if (game)
	{
		Game_Destroy(game);
		game = 0;
	}

	return result;
}