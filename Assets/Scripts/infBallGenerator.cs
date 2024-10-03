using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infBallGenerator : MonoBehaviour
{
    [SerializeField] private GameObject stone;

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

        stone.SetActive(false);
        
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
