using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    [SerializeField] private GameObject settings_tab;

    public void GoBackToMenu()
    {
        SceneManager.LoadScene("Menu"); 
    }
    public void StartGame(int gametype)
    {
        PlayerPrefs.SetInt("Type", gametype);
        SceneManager.LoadScene("Game"); 
    }
    public void GoToLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
    public void OpenSettingsTab()
    {
        settings_tab.SetActive(!settings_tab.activeSelf);
    }
}
