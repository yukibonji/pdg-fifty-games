#ifndef UTILITY_MACROS_H
#define UTILITY_MACROS_H

#define SAFE_DESTROY(ptr,func) if(ptr){func(ptr);(ptr)=0;}
#define NEW(typ) (typ*)SDL_malloc(sizeof(typ))

#endif
