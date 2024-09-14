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

    private float pushForce = 0.0f;

    // Booleans //
    private bool isOnHand = true;

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

        hasCollided = false;
        sm.isInitiallyClicked = false;

        sm.listOfBalls.Add(rb2D);
        GetComponent<CircleCollider2D>().isTrigger = true;

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
                    Shoot(ll.mousePosition, pushForce);
                }

                if (Input.GetMouseButtonDown(1))
                {
                    ChargeShot();
                }

                if (Input.GetMouseButtonUp(1))
                {
                    Shoot(ll.mousePosition, pushForce);
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
                SetIntoAimingState();
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

    public void Shoot(Vector2 direction, float force)
    {
        if (rl.rotationVector.y < 0) rb2D.drag -= rl.rotationVector.y * 2.0f;
        if (rl.rotationVector.y > 0) rb2D.drag -= rl.rotationVector.y * 0.7f;

        PlayShotSound();
        ApplyShotForce(direction, force);

        cue.localPosition = new Vector3(-18.75f, 0f, 0f);
        SetLineVisibility(false);
        
        Cursor.visible = true;
        sm.UIisInteractable = false;

        sm.isCharged = false;

        sm.InitializeStateCheck();
    }

    private void ApplyShotForce(Vector2 direction, float force)
    {
        parallelDirection = (direction - rb2D.position).normalized;
        perpendicularDirection = new Vector2(parallelDirection.y, -parallelDirection.x);

        rb2D.velocity = parallelDirection * force;
        pushForce = 0;
    }

    private void PlayShotSound()
    {
        audioSource.volume = pushForce / maxPushForce;
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.Play();
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
        return sm.AllBallsHasStopped() && !sm.UIisInteracted && sm.isPlayerTurn;
    }

    public void SetIntoAimingState()
    {
        rl.ResetRotationVector();
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
        if (IsBagged())
        {
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
        ll.HandleLineRendering();
        line.SetActive(visibility);
    }

    public void ResetState()
    {
        SetLineVisibility(false);
        isOnHand = true;
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