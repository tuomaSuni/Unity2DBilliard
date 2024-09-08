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
        if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() && sm.AllBallsHasStopped()) SetRotationAmount();
        if (Input.GetKeyDown(KeyCode.Escape)) uim.ActivateRotationPanel();
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

    public void ResetRotationAmount()
    {
        rotator.anchoredPosition = Vector2.zero;
    }

    private void OnEnable()
    {
        Cursor.visible = true;
    }

    private void OnDisable()
    {
        Cursor.visible = false;
    }
}
