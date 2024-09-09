using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class sceneManager : MonoBehaviour
{
    [SerializeField] private Image fadeImage; // Assign your UI Image here
    private float fadeDuration = 0.5f;
    [SerializeField] private GameObject settings_tab;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void StartGame(int gametype)
    {
        PlayerPrefs.SetInt("Type", gametype);
        StartCoroutine(FadeOut("Game"));
    }
    public void LoadScene(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }
    public void OpenSettingsTab()
    {
        settings_tab.SetActive(!settings_tab.activeSelf);
    }

    private IEnumerator FadeOut(string scene) {
        for (float t = 0; t <= 1; t += Time.deltaTime / fadeDuration) {
            Color c = fadeImage.color;
            c.a = t;
            fadeImage.color = c;
            yield return null;
        }

        SceneManager.LoadScene(scene);
    }

    private IEnumerator FadeIn() {
        for (float t = 1; t >= 0; t -= Time.deltaTime / fadeDuration) {
            Color c = fadeImage.color;
            c.a = t;
            fadeImage.color = c;
            yield return null;
        }
    }
}
