using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    [SerializeField] private stateManager sm;
    [SerializeField] private Transform sets;
    [SerializeField] private GameObject stone;
    [SerializeField] private GameObject computer;

    void Awake()
    {
        Transform playedSet = sets.GetChild(PlayerPrefs.GetInt("Type"));
        playedSet.gameObject.SetActive(true);

        if (PlayerPrefs.GetInt("Type") == 2) 
        {
            stone.AddComponent<nineBallStoneLogic>();
            stone.GetComponent<nineBallStoneLogic>().nineset = sets.GetChild(2);
        }

        if (PlayerPrefs.GetInt("Mode") == 1)
        {
            computer.SetActive(true);
            computer.GetComponent<computerLogic>().set = playedSet;
        }
    }

    void Start()
    {
        if (computer.activeSelf) sm.isSoloMode = false;
    }
}
