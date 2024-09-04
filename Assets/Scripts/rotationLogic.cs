using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class rotationLogic : MonoBehaviour
{
    [HideInInspector] public Vector2 rotationVector;
    private Transform rotationDot;

    private void Start()
    {
        rotationDot = transform.GetChild(0).transform;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject()) SetRotationAmount();
    }

    private void SetRotationAmount()
    {
        Vector3 mousePosition = Input.mousePosition;
        rotationDot.position = mousePosition;
        Vector3 direction = mousePosition - transform.position;

        if (Vector2.Distance(transform.position, rotationDot.position) > 100f)
        {
            direction.Normalize();
            mousePosition = transform.position + direction * 100f;
        }

        rotationDot.position = mousePosition;
    }

    public void ResetRotation()
    {
        rotationDot.position = Vector3.zero;
    }

    private void SetRotationVector()
    {
        
    }
}
