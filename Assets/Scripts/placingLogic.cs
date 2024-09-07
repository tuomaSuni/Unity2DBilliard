using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placingLogic : MonoBehaviour
{
    [HideInInspector] public bool isPlaceable = true;
    private HashSet<Collider2D> collidersInContact = new HashSet<Collider2D>();
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        collidersInContact.Add(collider);
        UpdateRockState();
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        collidersInContact.Add(collider);
        UpdateRockState();
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        collidersInContact.Remove(collider);
        UpdateRockState();
    }

    private void SetRockTransparency(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    private void UpdateRockState()
    {
        if (collidersInContact.Count > 0)
        {
            SetRockTransparency(0.7f);
            isPlaceable = false;
        }
        else
        {
            SetRockTransparency(1.0f);
            isPlaceable = true;
        }
    }
}
