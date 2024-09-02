using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLogic : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform cue;
    private Transform line;

    [SerializeField] private float pushForce = 0f;
    [SerializeField] private float maxPushForce = 50.0f;

    private Vector2 dir;
    private bool isOnHand = true;
    private bool isFree = true;
    private bool initialClickReleased = false;
    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cue = transform.GetChild(0).transform;
        line = transform.GetChild(1).transform;

        SetVisibility(false);
        GetComponent<CircleCollider2D>().isTrigger = true;
        transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
    }

    void Update()
    {
        HandleAiming();
        HandleShooting();
        HandlePushForce();
        HandleMovementState();
    }

    private void HandleAiming()
    {
        if (isOnHand)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 1f;
            transform.position = mousePosition;
        }
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) && isOnHand && isFree && !isMoving)
        {
            StartAiming();
        }

        if (Input.GetMouseButtonUp(0) && !isOnHand && !isMoving && initialClickReleased)
        {
            Shoot();
        }

        if (!isOnHand && Input.GetMouseButtonDown(1) && !isMoving)
        {
            ChargeShot();
        }

        if (!isOnHand && Input.GetMouseButtonUp(1) && !isMoving)
        {
            Shoot();
        }
    }

    private void HandlePushForce()
    {
        if (initialClickReleased && Input.GetMouseButton(0) && pushForce < maxPushForce && !isMoving)
        {
            pushForce += Time.deltaTime * 20;
            cue.localPosition += Vector3.left * Time.deltaTime * 2;
        }
    }

    private void HandleMovementState()
    {
        if (!isOnHand && rb.velocity.magnitude < 0.01f)
        {
            SetVisibility(true);
            isMoving = false;
        }

        if (transform.localScale == new Vector3(0.30f, 0.30f, 0.30f))
        {
            SetVisibility(false);
        }
    }

    private void StartAiming()
    {
        GetComponent<CircleCollider2D>().isTrigger = false;
        isOnHand = false;
        initialClickReleased = false;
        SetVisibility(true);
    }

    private void Shoot()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dir = (mousePosition - rb.position).normalized;
        rb.velocity = dir * pushForce;

        cue.gameObject.SetActive(false);
        line.gameObject.SetActive(false);
        cue.localPosition = new Vector3(-18.75f, 0f, 0f);
        pushForce = 0;
        isMoving = true;
    }

    private void ChargeShot()
    {
        pushForce = maxPushForce;
        cue.localPosition += Vector3.left * 4;
    }

    private void SetVisibility(bool visibility)
    {
        cue.gameObject.SetActive(visibility);
        line.gameObject.SetActive(visibility);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        isFree = false;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        isFree = true;
    }
}