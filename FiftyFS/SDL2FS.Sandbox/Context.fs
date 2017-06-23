namespace Sdl

open System

type context<'TWindow,'TRenderer when 'TRenderer:comparison and 'TWindow:comparison> =
    {windows:Map<'TWindow,IntPtr>;
    renderers:Map<'TRenderer,IntPtr>}

module Context =
    
    let create () : context<'TWindow,'TRenderer> = 
        {windows = Map.empty;
        renderers = Map.empty}



