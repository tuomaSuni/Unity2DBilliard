using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nineBallLogic : ballLogic
{
    [SerializeField] private GameObject rock;

    protected override void OnDisable()
    {
        base.OnDisable();
        sm.CheckNineballGameState(rock.GetComponent<nineBallRockLogic>().isJustified);
    }
}
