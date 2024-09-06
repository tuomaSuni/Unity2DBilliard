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

    [Header("Settings")]
    [SerializeField] private float pushForce = 0f;
    [SerializeField] private float maxPushForce = 50.0f;

    [Header("Booleans")]
    private bool isFree = true;
    private bool isOnHand = true;
    private bool initialClickReleased = false;

    [Header("References")]
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private Rigidbody2D rigidbody2D;
    
    [Header("Directions")]
    private Vector2 parallelDirection;
    private Vector2 perpendicularDirection;

    [Header("Velocities")]
    private float rockVelocity;
    private float velocityOnImpact;

    [Header("Physics")]
    private HashSet<Collider2D> collidersInContact = new HashSet<Collider2D>();
    private int collisionsCount = 0;

    private void Awake()
    {
        // Initialize component references
        lineLogic = line.GetComponent<lineLogic>();
        rotationLogic = GetComponent<rotationLogic>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        // Reset variables on enable
        ResetRockState();
        stateManager.listOfBalls.Add(rigidbody2D);
        GetComponent<CircleCollider2D>().isTrigger = true;
        collisionsCount = 0;
        transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
    }

    private void Update()
    {
        // Track velocity
        rockVelocity = rigidbody2D.velocity.magnitude;

        // Handle user inputs and actions
        HandleAiming();
        HandleShooting();
        HandleMovementState();
    }

    private void HandleAiming()
    {
        if (isOnHand && !stateManager.isSettingRotation)
        {
            // Move rock with mouse if on hand
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 1f;  // Keep the rock on the same Z plane
            transform.position = mousePosition;
        }
    }

    private void StartAiming()
    {
        GetComponent<CircleCollider2D>().isTrigger = false;
        isOnHand = false;
    }

    private void Shoot()
    {
        Cursor.visible = true;
        SetCueAndLineVisibility(false);
        cue.transform.localPosition = new Vector3(-18.75f, 0f, 0f);

        ApplyShotForce();
        rotationLogic.ResetRotation();
        PlayShotSound();
    }

    private void ApplyShotForce()
    {
        parallelDirection = (lineLogic.mousePosition - rigidbody2D.position).normalized;
        perpendicularDirection = new Vector2(parallelDirection.y, -parallelDirection.x);

        rigidbody2D.velocity = parallelDirection * pushForce;
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
            // Charge shot logic
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
        if (!isOnHand && stateManager.AllBallsHasStopped())
        {
            SetCueAndLineVisibility(true);
            collisionsCount = 0;
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
        if (collisionsCount == 0)
        {
            StartCoroutine(ApplyRotationOnImpact());
        }
        collisionsCount++;
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

        for (int i = 0; i < velocityOnImpact; i++)
        {
            rigidbody2D.AddForce(3 * velocityOnImpact * rotationLogic.rotationVector.y * parallelDirection * Time.deltaTime);
            rigidbody2D.AddForce(3 * velocityOnImpact * rotationLogic.rotationVector.x * perpendicularDirection * Time.deltaTime);
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
