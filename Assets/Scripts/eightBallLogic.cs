using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eightBallLogic : ballLogic
{    
    protected override void OnDisable()
    {
        base.OnDisable();
        sm.CheckGameState();
    }
}
