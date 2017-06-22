namespace Splorr.Wanderer

type creatureType =
    | Tagon

type statisticType =
    | Health
    | Energy

type statistic = 
    {current:int;
    maximum:int}

type tagonDescriptor =
    {statistics:Map<statisticType,statistic>}

type creature =
    | Tagon of tagonDescriptor





