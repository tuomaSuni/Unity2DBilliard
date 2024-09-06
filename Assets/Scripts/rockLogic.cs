using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockLogic : MonoBehaviour
{
    [Header("Dependencies")]
    public stateManager stateManager;
    private lineLogic lineLogic;
    private rotationLogic rotationLogic;

    [Header("GameObjects")]
    [SerializeField] private GameObject cue;
    [SerializeField] private GameObject line;

    // Settings //
    private float pushForce = 0f;
    private readonly float maxPushForce = 50.0f;
    private readonly float rotationForce = 1.2f;

    // Booleans //
    private bool isFree = true;
    private bool isOnHand = true;
    private bool initialClickReleased = false;
    private bool isAiming = false;

    // References //
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private Rigidbody2D rb2D;
    
    // Directions //
    private Vector2 parallelDirection;
    private Vector2 perpendicularDirection;

    // Velocities //
    private float rockVelocity;
    private float velocityOnImpact;

    // Physics //
    private HashSet<Collider2D> collidersInContact = new HashSet<Collider2D>();
    private bool hasCollided = false;

    private void Awake()
    {
        lineLogic = line.GetComponent<lineLogic>();
        rotationLogic = GetComponent<rotationLogic>();
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        ResetRockState();
        isAiming = false;
        stateManager.listOfBalls.Add(rb2D);
        GetComponent<CircleCollider2D>().isTrigger = true;
        hasCollided = false;
        stateManager.UIisInteractable = false;
        transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
    }

    private void Update()
    {
        rockVelocity = rb2D.velocity.magnitude;
        
        HandleAiming();
        HandleShooting();
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

    private void StartAiming()
    {
        GetComponent<CircleCollider2D>().isTrigger = false;
        isOnHand = false;
        stateManager.UIisInteractable = true;
        Cursor.visible = false;
    }

    private void Shoot()
    {
        if (rotationLogic.rotationVector.y < 0) rb2D.drag -= rotationLogic.rotationVector.y / 30f;
        if (rotationLogic.rotationVector.y > 0) rb2D.drag -= rotationLogic.rotationVector.y / 60f;

        Cursor.visible = true;
        SetCueAndLineVisibility(false);
        isAiming = false;

        cue.transform.localPosition = new Vector3(-18.75f, 0f, 0f);

        PlayShotSound();
        ApplyShotForce();
    }

    private void ApplyShotForce()
    {
        parallelDirection = (lineLogic.mousePosition - rb2D.position).normalized;
        perpendicularDirection = new Vector2(parallelDirection.y, -parallelDirection.x);

        rb2D.velocity = parallelDirection * pushForce;
        pushForce = 0;
    }

    private void ChargeShot()
    {
        pushForce = maxPushForce;
        cue.transform.localPosition += Vector3.left * 4;
    }

    private void PlayShotSound()
    {
        audioSource.volume = pushForce / maxPushForce;
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.Play();
    }

    private void SetCueAndLineVisibility(bool isVisible)
    {
        cue.SetActive(isVisible);
        line.SetActive(isVisible);
    }

    private void HandleShooting()
    {
        if (initialClickReleased && Input.GetMouseButton(0) && CanChargeShot())
        {
            pushForce += Time.deltaTime * 30;
            cue.transform.localPosition += Vector3.left * Time.deltaTime * 2;
        }

        if (CanShoot())
        {
            if (Input.GetMouseButtonDown(0) && isOnHand && isFree)
            {
                StartAiming();
            }

            if (Input.GetMouseButtonUp(0) && !isOnHand && initialClickReleased)
            {
                Shoot();
            }

            if (Input.GetMouseButtonDown(1) && !isOnHand)
            {
                ChargeShot();
            }

            if (Input.GetMouseButtonUp(1) && !isOnHand)
            {
                Shoot();
            }
        }

        if (Input.GetMouseButtonUp(0) && !isOnHand && stateManager.AllBallsHasStopped())
        {
            initialClickReleased = true;
        }
    }

    private bool CanChargeShot()
    {
        return pushForce < maxPushForce && stateManager.AllBallsHasStopped() && !stateManager.isSettingRotation;
    }

    private bool CanShoot()
    {
        return stateManager.AllBallsHasStopped() && !stateManager.isSettingRotation;
    }

    private void HandleMovementState()
    {
        if (!isAiming && !isOnHand && stateManager.AllBallsHasStopped())
        {
            isAiming = true;
            SetCueAndLineVisibility(true);
            hasCollided = false;
            rb2D.drag = 1;
            rotationLogic.ResetRotation();
            if (!stateManager.isSettingRotation) Cursor.visible = false;
        }

        if (transform.localScale == new Vector3(0.30f, 0.30f, 0.30f))
        {
            SetCueAndLineVisibility(false);
            this.enabled = false;
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasCollided == false)
        {
            rb2D.drag = 1;

            StartCoroutine(ApplyRotationOnImpact());
        }

        hasCollided = true;
    }

    private void UpdateRockState()
    {
        if (collidersInContact.Count > 0)
        {
            SetRockTransparency(0.7f);
            isFree = false;
        }
        else
        {
            SetRockTransparency(1.0f);
            isFree = true;
        }
    }

    private void SetRockTransparency(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    private IEnumerator ApplyRotationOnImpact()
    {
        velocityOnImpact = rockVelocity;

        for (int i=0; i < velocityOnImpact * rotationForce; i++)
        {
            rb2D.AddForce(velocityOnImpact * rotationForce * rotationLogic.rotationVector.y * parallelDirection      * Time.deltaTime);
            rb2D.AddForce(velocityOnImpact * rotationForce * rotationLogic.rotationVector.x * perpendicularDirection * Time.deltaTime);
            yield return null;
        }
    }

    private void ResetRockState()
    {
        isFree = true;
        isOnHand = true;
        initialClickReleased = false;
    }
}