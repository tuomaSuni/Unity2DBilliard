using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infBallGenerator : MonoBehaviour
{
    public bool AllSiblingsAreDisabled()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                return false;
            }
        }

        return true;
    }

    public void EnableAllBalls()
    {
        StartCoroutine(EnableAllBallsWithDelay());
    }
    public IEnumerator EnableAllBallsWithDelay()
    {
        yield return new WaitForEndOfFrame();

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
