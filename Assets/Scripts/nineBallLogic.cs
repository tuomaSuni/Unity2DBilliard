using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nineBallLogic : ballLogic
{
    protected override void OnDisable()
    {
        base.OnDisable();
        sm.CheckNineballGameState();
    }
}
