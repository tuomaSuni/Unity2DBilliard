using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingsLogic : MonoBehaviour
{
    public void OpenSettingsTab()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) OpenSettingsTab();
    }
}
