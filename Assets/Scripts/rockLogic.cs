using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockLogic : MonoBehaviour
{
    [Header("Dependencies")]
    public stateManager   sm;
    private lineLogic     ll;
    private rotationLogic rl;
    private placingLogic  pl;
    private nineBallRockLogic nbrl;

    [Header("Utilities")]
    [SerializeField] private Transform cue;
    [SerializeField] private GameObject line;

    // Settings //
    
    [Range(50f, 70f)]
    [SerializeField] private float maxPushForce  = 50.0f;
    [SerializeField] private float rotationForce = 10.0f;

    private float pushForce = 0f;

    // Booleans //
    private bool isOnHand = true;
    private bool isAiming = false;

    // References //
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

        ll = line.GetComponent<lineLogic>();
        rl = GetComponent<rotationLogic>();
        pl = GetComponent<placingLogic>();

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        nbrl = GetComponent<nineBallRockLogic>();
    }

    private void OnEnable()
    {
        isOnHand = true;
        isAiming = false;

        hasCollided = false;
        sm.isInitiallyClicked = false;

        sm.listOfBalls.Add(rb2D);
        GetComponent<CircleCollider2D>().isTrigger = true;

        transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        rb2D.simulated = true;
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
        if (isOnHand && IsMouseOverGameWindow())
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 1f;
            transform.position = mousePosition;
        }
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButton(0) && CanChargeShot() && sm.isInitiallyClicked)
        {
            pushForce += Time.deltaTime * 30;
            cue.localPosition += Vector3.left * Time.deltaTime * 2;

            sm.isCharged = true;
        }

        if (!isOnHand)
        {
            if (CanShoot())
            {
                if (Input.GetMouseButtonUp(0) && sm.isInitiallyClicked)
                {
                    Shoot();
                }

                if (Input.GetMouseButtonDown(1))
                {
                    ChargeShot();
                }

                if (Input.GetMouseButtonUp(1))
                {
                    Shoot();
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                sm.isInitiallyClicked = true;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && pl.isPlaceable)
            {
                StartAiming();
            }
        }
    }

    private void StartAiming()
    {
        GetComponent<CircleCollider2D>().isTrigger = false;

        isOnHand = false;
        Cursor.visible = false;
        sm.UIisInteractable = true;
    }

    private void Shoot()
    {
        if (rl.rotationVector.y < 0) rb2D.drag -= rl.rotationVector.y * 2.0f;
        if (rl.rotationVector.y > 0) rb2D.drag -= rl.rotationVector.y * 0.7f;

        PlayShotSound();
        ApplyShotForce();

        cue.localPosition = new Vector3(-18.75f, 0f, 0f);
        SetLineVisibility(false);
        
        Cursor.visible = true;
        sm.UIisInteractable = false;

        sm.isCharged = false;
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
        parallelDirection = (ll.mousePosition - rb2D.position).normalized;
        perpendicularDirection = new Vector2(parallelDirection.y, -parallelDirection.x);

        rb2D.velocity = parallelDirection * pushForce;
        pushForce = 0;
    }

    private void ChargeShot()
    {
        sm.isCharged = true;
        pushForce = maxPushForce;
        cue.localPosition += Vector3.left * 4;
    }

    private bool CanChargeShot()
    {
        return pushForce < maxPushForce && sm.AllBallsHasStopped() && !sm.UIisInteracted;
    }

    private bool CanShoot()
    {
        return sm.AllBallsHasStopped() && !sm.UIisInteracted;
    }

    public bool IsInAimingState()
    {
        return (!isOnHand && !isAiming && sm.AllBallsHasStopped());
    }

    private void SetIntoAimingState()
    {
        rl.ResetRotationVector();
        isAiming = true;
        sm.UIisInteractable = true;
        SetLineVisibility(true);
        hasCollided = false;
        rb2D.drag = 1;
        Cursor.visible = false;

        if (nbrl != null)
        {
            nbrl.hasCollided = false;
            nbrl.isJustified = true;
        }
    }

    private bool IsBagged()
    {
        return (transform.localScale == new Vector3(0.30f, 0.30f, 0.30f));
    }

    private void HandleMovementState()
    {
        if (IsInAimingState())
        {
            SetIntoAimingState();
        }

        if (IsBagged())
        {
            rb2D.simulated = false;
            ResetState();
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
            rb2D.AddForce(velocityOnImpact * rl.rotationVector.y * parallelDirection);
            rb2D.AddForce(velocityOnImpact * rl.rotationVector.x * perpendicularDirection);
            yield return new WaitForFixedUpdate();
        }
    }

    private void SetLineVisibility(bool visibility)
    {
        line.SetActive(visibility);
        ll.HandleLineRendering();
    }

    public void ResetState()
    {
        SetLineVisibility(false);
        Cursor.visible = true;
        sm.UIisInteractable = false;
        this.enabled = false;
    }

    bool IsMouseOverGameWindow()
    {
        Vector3 mousePosition = Input.mousePosition;
        return !( 0 > mousePosition.x || 0 > mousePosition.y || Screen.width < mousePosition.x || Screen.height < mousePosition.y );
    }
}