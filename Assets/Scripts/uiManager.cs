using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class uiManager : MonoBehaviour
{
    [SerializeField] stateManager sm;
    [SerializeField] private TextMeshProUGUI info;
    [SerializeField] GameObject rotationPanel;
    [SerializeField] GameObject GameEndPanel;

    public void GameEnd()
    {
        GameEndPanel.SetActive(!GameEndPanel.activeSelf);
        sm.isSettingRotation = !sm.isSettingRotation;

        if (sm.HasGameEnded)
        {
            if (sm.HasPlayerWon) info.text = "you won. play again?";
            else info.text = "you lost. play again?";
        }
    }

    public void ActivateRotationPanel()
    {
        rotationPanel.SetActive(!rotationPanel.activeSelf);
        sm.isSettingRotation = !sm.isSettingRotation;
        sm.isChargeable = false;
        sm.UIisInteractable = !sm.UIisInteractable;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && sm.UIisInteractable) ActivateRotationPanel();
        if (Input.GetKeyDown(KeyCode.Escape) && sm.UIisInteractable) GameEnd();
    }
}
