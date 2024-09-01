using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockLogic : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private Transform cue;
    [SerializeField] private Transform line;
    [SerializeField] private float pushForce = 1f;
    [SerializeField] private float maxpushForce = 10.0f;
    private bool isstable = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && pushForce < maxpushForce)
        {
            pushForce += Time.deltaTime * 5;
            cue.localPosition += Vector3.left * Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - rb.position).normalized;
            rb.velocity = direction * pushForce;

            cue.gameObject.SetActive(false);
            line.gameObject.SetActive(false);
            cue.localPosition = new Vector3(-18.75f, 0f, 0f);

            isstable = true;

            pushForce = 0;
        }

        if (rb.velocity.magnitude < 0.01f && isstable)
        {
            isstable = false;
            OnAction();
        }
    }

    void OnAction()
    {
        cue.gameObject.SetActive(true);
        line.gameObject.SetActive(true);
    }
}