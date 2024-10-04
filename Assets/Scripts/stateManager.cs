using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateManager : MonoBehaviour
{
    public bool isSoloMode = true;
    public bool isLegalMove = false;
    public int? ballType = null;

    [Header("Computer")]
    [SerializeField] private GameObject Computer;
    [SerializeField] private computerLogic cl;

    [Header("Stone")]
    [SerializeField] private GameObject Stone;
    [SerializeField] private stoneLogic sl;

    [Header("Balls")]
    [SerializeField] public List<Rigidbody2D> listOfBalls = new List<Rigidbody2D>();

    // Booleans
    [HideInInspector] public bool HasPlayerWon;
    [HideInInspector] public bool HasGameEnded = false;
    [HideInInspector] public bool UIisInteractable = false;
    [HideInInspector] public bool UIisInteracted = false;
    [HideInInspector] public bool isCharged = false;
    [HideInInspector] public bool isInitiallyClicked = false;
    [HideInInspector] public bool isPlayerTurn = true;

    [Header("GameObjects")]
    [SerializeField] private BoxCollider2D limit;
    [SerializeField] private uiManager uim;

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
        if (!Stone.activeSelf)
        {
            OnStoneBagged();
        }

        if (Stone.GetComponent<CircleCollider2D>().isTrigger == false && limit.enabled == true) limit.enabled = false;

    }

    public void CheckEightballGameState()
    {
        EndGame(listOfBalls.Count == 1 && listOfBalls[0] == Stone.GetComponent<Rigidbody2D>() == isPlayerTurn);
    }

    public void CheckNineballGameState(bool wasLegalShot)
    {
        EndGame((listOfBalls.Count == 1 && wasLegalShot) == isPlayerTurn);
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
            uim.ActivateMenuPanel();
            Destroy(uim);
        }

        if (Stone != null)
        {
            sl.ResetState();
        }

        Destroy(this);
    }

    public void InitializeStateCheck()
    {
        if (isSoloMode)     StartCoroutine(SoloMode());
        else                StartCoroutine(ComputerMode());
    }

    private void OnStoneBagged()
    {
        Stone.SetActive(true);
        sl.enabled = true;
        isPlayerTurn = true;
    }

    private IEnumerator SoloMode()
    {
        while (AllBallsHasStopped() == false)
        {
            yield return null;
        }
        
        sl.SetIntoAimingState();
    }

    private IEnumerator ComputerMode()
    {
        while (AllBallsHasStopped() == false)
        {
            yield return null;
        }
        
        if (!isLegalMove) isPlayerTurn = !isPlayerTurn;
        
        if (isPlayerTurn) sl.SetIntoAimingState();
        else StartCoroutine(cl.Shoot(sl));

        isLegalMove = false;
    }
}