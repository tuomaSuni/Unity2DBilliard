using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    [SerializeField] GameObject eight_set;
    [SerializeField] GameObject nine_set;

    void Awake()
    {
        if (PlayerPrefs.GetInt("Type") == 0) Destroy(eight_set);

        if (PlayerPrefs.GetInt("Type") == 1) Destroy(nine_set);
    }
}
