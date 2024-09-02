using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateManager : MonoBehaviour
{
    [SerializeField] private GameObject rock;
    private GameObject currentRock;

    private void Start()
    {
        currentRock = Instantiate(rock);
    }

    private void Update()
    {
        if (currentRock == null) currentRock = Instantiate(rock);
    }
}
