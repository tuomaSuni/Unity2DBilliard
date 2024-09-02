using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateManager : MonoBehaviour
{
    [SerializeField] private GameObject rock;
    private GameObject currentRock;
    [SerializeField] private BoxCollider2D limit;
    [SerializeField] private Transform set;
    private List<Rigidbody2D> ballRigidbodies = new List<Rigidbody2D>();
    private void Start()
    {
        foreach (Transform ball in set)
        {
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            ballRigidbodies.Add(rb);
        }

        currentRock = Instantiate(rock, new Vector3(-5, 0, 0), Quaternion.identity);
        AddBall(currentRock.GetComponent<Rigidbody2D>());

        currentRock.GetComponent<RockLogic>().sm = this;
    }

    private void Update()
    {
        if (currentRock == null)
        {
            currentRock = Instantiate(rock);
            currentRock.GetComponent<RockLogic>().sm = this;

            AddBall(currentRock.GetComponent<Rigidbody2D>());
        }
        if (currentRock.GetComponent<CircleCollider2D>().isTrigger == false && limit.enabled == true) limit.enabled = false;
    }

    private void AddBall(Rigidbody2D ballRigidbody)
    {
        if (ballRigidbody != null)
        {
            ballRigidbodies.Add(ballRigidbody);
        }
    }

    public bool AllBallsStopped()
    {
        foreach (Rigidbody2D rb in ballRigidbodies)
        {
            if (rb.velocity.magnitude > 0.02f)
            {
                return false;
            }
        }
        return true;
    }
}