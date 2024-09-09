using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuManager : MonoBehaviour
{
    [SerializeField] private GameObject type;
    public void SetGamemode(int gamemode)
    {
        PlayerPrefs.SetInt("Mode", gamemode);
        type.SetActive(true);
    }

    public void PreviousSet()
    {
        type.SetActive(false);
    }
}
