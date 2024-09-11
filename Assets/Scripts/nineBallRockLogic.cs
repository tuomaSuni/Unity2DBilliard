using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(rockLogic))]
public class nineBallRockLogic : MonoBehaviour
{
    private stateManager sm;
    [HideInInspector] public Transform nineset;
    private bool hasCollided = false;
    [HideInInspector] public bool isJustified = true;

    private void Start()
    {
        sm = GetComponent<rockLogic>().sm;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasCollided == false && collision.gameObject.CompareTag("Ball"))
        {
            hasCollided = true;

            if (collision.transform != nineset.GetChild(0))
            {
                isJustified = false;
                StartCoroutine(SetRockToHand());
            }
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

    private void OnDisable()
    {
        hasCollided = false;
        isJustified = true;
    }
}
