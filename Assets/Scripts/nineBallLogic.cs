using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nineBallLogic : ballLogic
{
    [SerializeField] private GameObject rock;
    private bool isQuitting = false;

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    protected override void OnDisable()
    {
        if (isQuitting) return;

        base.OnDisable();

        nineBallRockLogic rockLogic = rock.GetComponent<nineBallRockLogic>();
        sm.CheckNineballGameState(rockLogic.savedIsJustified);
    }
}
