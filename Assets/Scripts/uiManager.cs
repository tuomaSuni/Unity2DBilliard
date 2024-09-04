using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class uiManager : MonoBehaviour
{
    [SerializeField] stateManager sm;
    [SerializeField] private TextMeshProUGUI info;

    private void Awake()
    {
        if (sm.HasPlayerWon) info.text = "you won. play again?";
        else info.text = "you lost. play again?";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game"); 
    }
    public void GoBackToMenu()
    {
        SceneManager.LoadScene("Menu"); 
    }
}
