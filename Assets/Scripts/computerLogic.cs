using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class computerLogic : MonoBehaviour
{
    [SerializeField] private stateManager sm;
    [HideInInspector] public Transform set;    

    public Vector2 SetTarget()
    {
        Transform currentTargetBall = null;
        
        if (PlayerPrefs.GetInt("Type") == 0)
        {
            List<Transform> enabledObjects = new List<Transform>();

            for (int i = 0; i < set.childCount; i++)
            {
                Transform child = set.GetChild(i);
                if (child.gameObject.activeSelf)
                {
                    enabledObjects.Add(child);
                }
            }

            if (enabledObjects.Count > 0)
            {
                int randomIndex = Random.Range(0, enabledObjects.Count);
                currentTargetBall = enabledObjects[randomIndex];
            }
        }
        else if (PlayerPrefs.GetInt("Type") == 1)
        {
            for (int i = 0; i < set.childCount; i++)
            {
                if (set.GetChild(i).gameObject.activeSelf == true)
                {
                    currentTargetBall = set.GetChild(i-sm.ballType ?? 0);

                    break;
                }
            }
        }
        else if (PlayerPrefs.GetInt("Type") == 2)
        {
            for (int i = 0; i < set.childCount; i++)
            {
                if (set.GetChild(i).gameObject.activeSelf == true)
                {
                    currentTargetBall = set.GetChild(i);

                    break;
                }
            }
        }

        return currentTargetBall.transform.position - transform.position.normalized;
    }

    public float SetForce()
    {
        return Random.Range(10f, 30f);
    }
}
