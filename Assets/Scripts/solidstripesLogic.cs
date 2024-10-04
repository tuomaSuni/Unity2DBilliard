using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class solidstripesLogic : ballLogic
{
    [SerializeField] private int ballType;
    protected override void OnCollisionEnter2D(Collision2D col)
    {
        base.OnCollisionEnter2D(col);

        if (col.gameObject.CompareTag("Hole"))
        {
            if (sm.ballType == null)
            {
                sm.ballType = ballType;
            }

            if (sm.isLegalMove == false)
            {
                sm.isLegalMove = sm.isPlayerTurn == (sm.ballType == ballType);
            }
        }
    }
}
