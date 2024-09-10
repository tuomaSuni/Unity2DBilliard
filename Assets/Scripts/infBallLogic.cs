using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infBallLogic : ballLogic
{
    private infBallGenerator ibf;
    private Vector2 onPlayLocation;

    protected override void Awake()
    {
        base.Awake();
        onPlayLocation = transform.localPosition;
    }

    private void OnEnable()
    {
        base.Start();

        ibf = transform.parent.gameObject.GetComponent<infBallGenerator>();

        transform.localScale = Vector3.one;
        transform.localPosition = onPlayLocation;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        if (ibf.AllSiblingsAreDisabled()) ibf.EnableAllBalls();
    }
}