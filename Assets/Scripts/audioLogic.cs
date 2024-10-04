using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioLogic : MonoBehaviour
{
    [SerializeField] AudioSource background;
    [SerializeField] AudioSource selection;

    private void Start()
    {
        SetBackgroundVolume();
        SetSelectionVolume();
    }

    public void SetBackgroundVolume()
    {
        background.volume = PlayerPrefs.GetFloat("Song", 0.5f);
    }

    public void SetSelectionVolume()
    {
        selection.volume = PlayerPrefs.GetFloat("Sound", 0.5f);
    }
}
