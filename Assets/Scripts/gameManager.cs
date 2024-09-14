using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    [SerializeField] private Transform sets;
    [SerializeField] private GameObject rock;

    void Awake()
    {
        Transform playedSet = sets.GetChild(PlayerPrefs.GetInt("Type"));
        playedSet.gameObject.SetActive(true);

        if (PlayerPrefs.GetInt("Type") == 2) 
        {
            rock.AddComponent<nineBallRockLogic>();
            rock.GetComponent<nineBallRockLogic>().nineset = sets.GetChild(2);
        }
    }
}
