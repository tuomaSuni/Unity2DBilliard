using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    [SerializeField] private Transform sets;

    void Awake()
    {
        sets.GetChild(PlayerPrefs.GetInt("Type")).gameObject.SetActive(true);
    }
}
