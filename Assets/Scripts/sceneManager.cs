using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    [SerializeField] private GameObject settings_tab;
    public void ChangeScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenSettingsTab()
    {
        settings_tab.SetActive(!settings_tab.activeSelf);
    }
}
