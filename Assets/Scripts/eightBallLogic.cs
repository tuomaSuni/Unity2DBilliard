using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eightBallLogic : ballLogic
{
    private bool isQuitting = false;
    
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    protected override void OnDisable()
    {
        if (isQuitting) return;

        base.OnDisable();
        sm.CheckEightballGameState();
    }
}