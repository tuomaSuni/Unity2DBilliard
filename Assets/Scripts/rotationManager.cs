using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class rotationManager : MonoBehaviour
{
    [SerializeField] private stateManager sm;
    [SerializeField] private uiManager uim;

    public RectTransform rotator;

    private void Update()
    {
        if (Input.GetMouseButton(0) && sm.AllBallsHasStopped() && CloseEnough()) SetRotationAmount();
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab)) uim.ActivateRotationPanel();
    }

    private void SetRotationAmount()
    {
        Vector3 mousePosition = Input.mousePosition;
        rotator.position = mousePosition;
        Vector3 direction = mousePosition - transform.position;

        if (Vector2.Distance(transform.position, rotator.position) > 95f)
        {
            direction.Normalize();
            mousePosition = transform.position + direction * 95f;
        }

        rotator.position = mousePosition;
    }
    
    private bool CloseEnough()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (Vector2.Distance(Vector2.zero, mousePos) < 5f) return true;
        else return false;
    }

    public void ResetRotationAmount()
    {
        rotator.anchoredPosition = Vector2.zero;
    }
}
