using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioLogic : MonoBehaviour
{
    [SerializeField] private settingsLogic sl;
    [SerializeField] AudioSource background;
    [SerializeField] AudioSource selection;

    private void Start()
    {
        sl.OnSongSet += SetBackgroundVolume;
        sl.OnSoundSet += SetSelectionVolume;

        SetBackgroundVolume();
        SetSelectionVolume();
    }

    private void SetBackgroundVolume()
    {
        background.volume = PlayerPrefs.GetFloat("Song", 0.5f);
    }

    private void SetSelectionVolume()
    {
        selection.volume = PlayerPrefs.GetFloat("Sound", 0.5f);
    }

    private void OnDestroy()
    {
        sl.OnSongSet -= SetBackgroundVolume;
        sl.OnSoundSet -= SetBackgroundVolume;
    }
}
