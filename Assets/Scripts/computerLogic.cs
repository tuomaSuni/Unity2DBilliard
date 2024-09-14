using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class computerLogic : MonoBehaviour
{
    [HideInInspector] public Transform set;

    public Vector2 SetTarget()
    {
        return set.GetChild(0).transform.position - transform.position.normalized;
    }

    public float SetForce()
    {
        return Random.Range(20f, 50f);
    }
}
