using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers 
{
    public static readonly int W = 0;
    public static readonly int A = 1;
    public static readonly int S = 2;
    public static readonly int D = 3;


    public static Vector2[] Directions = { new Vector2(-0.6f,  0.3f), 
                                           new Vector2(-0.6f, -0.3f), 
                                           new Vector2( 0.6f, -0.3f), 
                                           new Vector2( 0.6f,  0.3f) 
                                         };

    public static Vector2 OffsetBetTiles = new Vector2(0.6f, 0.3f);

}
