using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockLogic : MonoBehaviour
{
    [Header("Dependencies")]
    public stateManager stateManager;
    private lineLogic lineLogic;
    private rotationLogic rotationLogic;
    private placingLogic placingLogic;

    [Header("GameObjects")]
    [SerializeField] private GameObject cue;
    [SerializeField] private GameObject line;

    // Settings //
    private float pushForce = 0f;

    [Range(50f, 70f)]
    [SerializeField] private float maxPushForce  = 50.0f;
    [SerializeField] private float rotationForce = 10.0f;

    // Booleans //
    private bool isOnHand = true;
    private bool isAiming = false;

    // References //
    private SpriteRenderer cueSpriteRenderer;
    private AudioSource audioSource;
    private Rigidbody2D rb2D;
    
    // Directions //
    private Vector2 parallelDirection;
    private Vector2 perpendicularDirection;

    // Velocities //
    private float rockVelocity;
    private float velocityOnImpact;

    // Physics //
    private bool hasCollided = false;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();

        lineLogic = line.GetComponent<lineLogic>();
        rotationLogic = GetComponent<rotationLogic>();
        placingLogic = GetComponent<placingLogic>();

        cueSpriteRenderer = cue.GetComponent<SpriteRenderer>();
        
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        isOnHand = true;
        isAiming = false;
        hasCollided = false;
        stateManager.isCharged = false;
        stateManager.isChargeable = false;

        stateManager.listOfBalls.Add(rb2D);
        GetComponent<CircleCollider2D>().isTrigger = true;

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

    private void HandleShooting()
    {
        if (stateManager.isChargeable && Input.GetMouseButton(0) && CanChargeShot())
        {
            stateManager.isCharged = true;
            pushForce += Time.deltaTime * 30;
            cue.transform.localPosition += Vector3.left * Time.deltaTime * 2;
        }

        if (CanShoot())
        {
            if (Input.GetMouseButtonDown(0) && isOnHand && placingLogic.isPlaceable)
            {
                StartAiming();
            }

            if (Input.GetMouseButtonUp(0) && !isOnHand && stateManager.isChargeable)
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

        if (Input.GetMouseButtonUp(0) && !isOnHand)
        {
            stateManager.isChargeable = true;
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
        if (rotationLogic.rotationVector.y < 0) rb2D.drag -= rotationLogic.rotationVector.y * 2.0f;
        if (rotationLogic.rotationVector.y > 0) rb2D.drag -= rotationLogic.rotationVector.y * 0.7f;

        PlayShotSound();
        ApplyShotForce();

        cue.transform.localPosition = new Vector3(-18.75f, 0f, 0f);
        SetCueAndLineVisibility(false);

        Cursor.visible = true;
        isAiming = false;
    }

    private void PlayShotSound()
    {
        audioSource.volume = pushForce / maxPushForce;
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.Play();
    }

    private void ApplyShotForce()
    {
        parallelDirection = (lineLogic.mousePosition - rb2D.position).normalized;
        perpendicularDirection = new Vector2(parallelDirection.y, -parallelDirection.x);

        rb2D.velocity = parallelDirection * pushForce;
        pushForce = 0;
        stateManager.isCharged = false;
    }

    private void ChargeShot()
    {
        pushForce = maxPushForce;
        cue.transform.localPosition += Vector3.left * 4;
        stateManager.isCharged = true;
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
            Cursor.visible = true;
            this.enabled = false;
        }
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

    private IEnumerator ApplyRotationOnImpact()
    {
        velocityOnImpact = Mathf.Log(rockVelocity) * rotationForce;

        for (int i=0; i < velocityOnImpact; i++)
        {
            rb2D.AddForce(velocityOnImpact * rotationLogic.rotationVector.y * parallelDirection);
            rb2D.AddForce(velocityOnImpact * rotationLogic.rotationVector.x * perpendicularDirection);
            yield return new WaitForFixedUpdate();
        }
    }

    private void SetCueAndLineVisibility(bool visibility)
    {
        line.SetActive(visibility);
        cueSpriteRenderer.enabled = visibility;
    }
}