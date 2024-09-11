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
    [SerializeField] GameObject menuPanel;

    private void Awake()
    {
        Cursor.visible = true;
    }
    
    public void ActivateMenuPanel()
    {
        SetUIactive();
        
        menuPanel.SetActive(!menuPanel.activeSelf);

        if (sm.HasGameEnded)
        {
            if (sm.HasPlayerWon) info.text = "you won. play again?";
            else info.text = "you lost. play again?";
        }
    }

    public void ActivateRotationPanel()
    {
        SetUIactive();

        rotationPanel.SetActive(!rotationPanel.activeSelf);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && sm.UIisInteractable) ActivateRotationPanel();
        if (Input.GetKeyDown(KeyCode.Escape) && sm.UIisInteractable) ActivateMenuPanel();
    }

    private void SetUIactive()
    {
        sm.UIisInteractable = !sm.UIisInteractable;
        sm.UIisInteracted = !sm.UIisInteracted;

        Cursor.visible = !Cursor.visible;
        
        if (Input.GetMouseButton(0)) sm.isInitiallyClicked = false;
    }
}
