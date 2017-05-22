#ifndef APPLICATION_INTERNALS_H
#define APPLICATION_INTERNALS_H

#include "Application.h"

#ifdef __cplusplus
extern "C" {
#endif

	struct Application	{
		void(*run_function)(struct Application*);
	};

#ifdef __cplusplus
}
#endif

#endif