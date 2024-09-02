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
    private SpriteRenderer nan;
    private HashSet<Collider2D> collidersInContact = new HashSet<Collider2D>();

    void Awake()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 1f;
        transform.position = mousePosition;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        cue = transform.GetChild(0).transform;
        line = transform.GetChild(1).transform;
        nan = transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();

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

        if (Input.GetMouseButtonUp(0) && !isOnHand)
        {
            initialClickReleased = true;
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
            Destroy(this);
        }
    }

    private void StartAiming()
    {
        GetComponent<CircleCollider2D>().isTrigger = false;
        isOnHand = false;
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

    void OnTriggerEnter2D(Collider2D col)
    {
        // Add the collider to the contact list
        collidersInContact.Add(col);

        // Update the state based on contact
        UpdateState();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        // Ensure the collider is in the contact list
        collidersInContact.Add(col);

        // Update the state
        UpdateState();
    }

    void OnTriggerExit2D(Collider2D col)
    {
        // Remove the collider from the contact list
        collidersInContact.Remove(col);

        // Update the state
        UpdateState();
    }

    private void UpdateState()
    {
        // If there are any colliders still in contact, set the state accordingly
        if (collidersInContact.Count > 0)
        {
            nan.enabled = true;
            isFree = false;
        }
        else
        {
            nan.enabled = false;
            isFree = true;
        }
    }
}