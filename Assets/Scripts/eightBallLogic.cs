using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eightBallLogic : ballLogic
{    
    protected override void OnDestroy()
    {
        if (sm != null) sm.CheckGameState();
    }
}
