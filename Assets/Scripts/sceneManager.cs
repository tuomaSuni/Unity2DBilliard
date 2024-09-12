using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class sceneManager : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    private float fadeDuration = 0.5f;

    private void Awake()
    {
        Cursor.visible = true;
        fadeImage.gameObject.SetActive(true);
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void StartGame(int gametype)
    {
        PlayerPrefs.SetInt("Type", gametype);
        
        if (PlayerPrefs.GetInt("Mode") == 2) StartCoroutine(FadeOut("Lobby"));
        else StartCoroutine(FadeOut("Game"));
    }
    public void LoadScene(string scene)
    {
        StartCoroutine(FadeOut(scene));
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
