using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class computerLogic : MonoBehaviour
{
    [HideInInspector] public Transform set;

    public Vector2 SetTarget()
    {
        Transform currentTargetBall = null;

        for (int i = 0; i < set.childCount; i++)
        {
            if (set.GetChild(i).gameObject.activeSelf == true)
            {
                currentTargetBall = set.GetChild(i);

                break;
            }
        }

        return currentTargetBall.transform.position - transform.position.normalized;
    }

    public float SetForce()
    {
        return Random.Range(20f, 50f);
    }
}
