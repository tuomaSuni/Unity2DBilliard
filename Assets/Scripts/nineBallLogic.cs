using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nineBallLogic : ballLogic
{
    [SerializeField] private GameObject stone;
    private bool isQuitting = false;

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    protected override void OnDisable()
    {
        if (isQuitting) return;

        base.OnDisable();

        nineBallStoneLogic stoneLogic = stone.GetComponent<nineBallStoneLogic>();
        sm.CheckNineballGameState(stoneLogic.savedIsJustified);
    }
}
