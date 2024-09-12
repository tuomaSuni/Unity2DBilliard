using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(rockLogic))]
public class nineBallRockLogic : MonoBehaviour
{
    private rockLogic rl;
    private stateManager sm;
    [HideInInspector] public Transform nineset;
    [HideInInspector] public bool hasCollided = false;
    [HideInInspector] public bool isJustified = true;
    [HideInInspector] public bool savedIsJustified = true;

    private void Start()
    {
        rl = GetComponent<rockLogic>();
        sm = GetComponent<rockLogic>().sm;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasCollided == false && collision.gameObject.CompareTag("Ball"))
        {
            hasCollided = true;
            Transform currentTargetBall = null;

            for (int i = 0; i < nineset.childCount; i++)
            {
                if (nineset.GetChild(i).gameObject.activeSelf == true)
                {
                    currentTargetBall = nineset.GetChild(i);

                    break;
                }
            }
            
            if (collision.transform != currentTargetBall)
            {
                isJustified = false;
                StartCoroutine(SetRockToHand());
            }

            savedIsJustified = isJustified;
        }
    }

    private IEnumerator SetRockToHand()
    {
        while (sm.AllBallsHasStopped() == false)
        {
            yield return null;
        }

        GetComponent<rockLogic>().ResetState();
        gameObject.SetActive(false);
    }
}
