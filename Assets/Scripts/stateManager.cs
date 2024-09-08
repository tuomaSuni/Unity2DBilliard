using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateManager : MonoBehaviour
{
    [Header("Balls")]
    [SerializeField] private GameObject Rock;
    [SerializeField] public List<Rigidbody2D> listOfBalls = new List<Rigidbody2D>();

    [Header("Booleans")]
    [HideInInspector] public bool HasPlayerWon;
    [HideInInspector] public bool HasGameEnded = false;
    [HideInInspector] public bool isSettingRotation = false;
    [HideInInspector] public bool UIisInteractable = false;
    [HideInInspector] public bool isChargeable = false;
    [HideInInspector] public bool isCharged = false;

    [Header("GameObjects")]
    [SerializeField] private BoxCollider2D limit;
    [SerializeField] private uiManager uim;
    [SerializeField] private Transform set;

    public bool AllBallsHasStopped()
    {
        foreach (Rigidbody2D rb in listOfBalls)
        {
            if (rb.velocity.magnitude > 0.02f)
            {
                return false;
            }
        }
        return true;
    }

    private void Update()
    {
        if (!Rock.activeSelf)
        {
            Rock.SetActive(true);
            Rock.GetComponent<rockLogic>().enabled = true;
        }

        if (Rock.GetComponent<CircleCollider2D>().isTrigger == false && limit.enabled == true) limit.enabled = false;
    }

    public void CheckEightballGameState()
    {
        if (listOfBalls.Count == 1 && listOfBalls[0] == Rock.GetComponent<Rigidbody2D>()) EndGame(true);    
        else EndGame(false);
    }

    public void CheckNineballGameState()
    {
        EndGame(true);
    }


    private void EndGame(bool playerWon)
    {
        HasGameEnded = true;
        HasPlayerWon = playerWon;

        CleanUp();
    }

    private void CleanUp()
    {
        if (uim != null)
        {
            uim.GameEnd();
            Destroy(uim);
        }

        if (Rock != null)
        {
            Destroy(Rock);
        }

        Destroy(this);
    }
}