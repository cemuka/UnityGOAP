using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GWorld 
{
    private static readonly GWorld instance = new GWorld();
    // Our world states
    private static WorldStates world;

    static GWorld() 
    {
        // Create our world
        world = new WorldStates();
    }

    public static GWorld Instance {
        get { return instance; }
    }

    public WorldStates GetWorld() {
        return world;
    }    
}
