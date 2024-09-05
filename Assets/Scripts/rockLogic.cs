using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockLogic : MonoBehaviour
{
    [Header("Dependencies")]
    public stateManager sm;
    private lineLogic ll;
    private rotationLogic rl;

    [Header("GameObjects")]
    [SerializeField] private GameObject cue;
    [SerializeField] private GameObject line;
    [SerializeField] private GameObject mark;
    
    [Header("Settings")]
    [SerializeField] private float pushForce = 0f;
    [SerializeField] private float maxPushForce = 50.0f;

    [Header("Booleans")]
    private bool isFree = true;
    private bool isOnHand = true;
    private bool initialClickReleased = false;
    private int collisionsSince = 0;
    private SpriteRenderer sr;
    private AudioSource audiosource;
    
    private HashSet<Collider2D> collidersInContact = new HashSet<Collider2D>();
    [Header("Directions")]
    private Vector2 dir_paraller;
    private Vector2 dir_perpendicular;
    private Rigidbody2D rb;

    void Awake()
    {
        ll   = line.GetComponent<lineLogic>();
        rl   = GetComponent<rotationLogic>();
        rb   = GetComponent<Rigidbody2D>();
        sr   = GetComponent<SpriteRenderer>();
        

        audiosource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        isFree = true;
        isOnHand = true;
        initialClickReleased = false;

        sm.listOfBalls.Add(rb);

        GetComponent<CircleCollider2D>().isTrigger = true;

        transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

        collisionsSince = 0;
    }

    void Update()
    {
        HandleAiming();
        HandleShooting();
        HandleMovementState();
    }

    private void HandleAiming()
    {
        if (this.isOnHand && !sm.isSettingRotation)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 1f;
            transform.position = mousePosition;
        }
    }

    private void StartAiming()
    {
        GetComponent<CircleCollider2D>().isTrigger = false;
        this.isOnHand = false;
    }

    private void Shoot()
    {
        Cursor.visible = true;
        SetCueAndLineVisibility(false);
        cue.transform.localPosition = new Vector3(-18.75f, 0f, 0f);

        SetVelocity();
        
        rl.ResetRotation();
        PlaySoundEffect();
    }

    private void PlaySoundEffect()
    {
        audiosource.volume = pushForce / maxPushForce;
        audiosource.pitch = Random.Range(0.95f, 1.05f);
        audiosource.Play();
    }

    private void SetVelocity()
    {
        dir_paraller = (ll.mousePosition - rb.position).normalized;
        dir_perpendicular = new Vector2(dir_paraller.y, -dir_paraller.x);

        rb.velocity = dir_paraller * pushForce;

        pushForce = 0;
    }

    private void ChargeShot()
    {
        pushForce = maxPushForce;
        cue.transform.localPosition += Vector3.left * 4;
    }

    private void SetCueAndLineVisibility(bool visibility)
    {
        cue.SetActive(visibility);
        line.SetActive(visibility);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        collidersInContact.Add(col);

        UpdateState();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        collidersInContact.Add(col);

        UpdateState();
    }

    void OnTriggerExit2D(Collider2D col)
    {
        collidersInContact.Remove(col);

        UpdateState();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (collisionsSince == 0) StartCoroutine(AddRotation());

        collisionsSince += 1;
    }

    private void UpdateState()
    {
        if (this.enabled)
        {
            if (collidersInContact.Count > 0)
            {
                mark.SetActive(true);
                SetAlpha(0.7f);
                isFree = false;
            }
            else
            {
                mark.SetActive(false);
                SetAlpha(1.0f);
                isFree = true;
            }
        }
    }

    private void SetAlpha(float alphaValue)
    {
        Color color = sr.color;
        color.a = alphaValue;
        sr.color = color;
    }

    private void HandleShooting()
    {
        if (initialClickReleased && Input.GetMouseButton(0) && pushForce < maxPushForce && sm.AllBallsHasStopped() && !sm.isSettingRotation)
        {
            pushForce += Time.deltaTime * 30;
            cue.transform.localPosition += Vector3.left * Time.deltaTime * 2;
        }

        if (sm.AllBallsHasStopped() && !sm.isSettingRotation)
        {
            if (Input.GetMouseButtonDown(0) && this.isOnHand && isFree)
            {
                StartAiming();
            }

            if (Input.GetMouseButtonUp(0)   && !this.isOnHand && initialClickReleased)
            {
                Shoot();
            }

            if (Input.GetMouseButtonDown(1) && !this.isOnHand && !sm.isSettingRotation)
            {
                ChargeShot();
            }

            if (Input.GetMouseButtonUp(1)   && !this.isOnHand)
            {
                Shoot();
            }
        }

        if (Input.GetMouseButtonUp(0) && !this.isOnHand && sm.AllBallsHasStopped())
        {
            initialClickReleased = true;
        }
    }
    private void HandleMovementState()
    {
        if (!this.isOnHand && sm.AllBallsHasStopped())
        {
            SetCueAndLineVisibility(true);
            collisionsSince = 0;
        }

        if (transform.localScale == new Vector3(0.30f, 0.30f, 0.30f))
        {
            SetCueAndLineVisibility(false);
            this.enabled = false;
        }
    }

    private IEnumerator AddRotation()
    {
        int initialVelocity = (int)(Mathf.Pow(rb.velocity.magnitude, 0.3f) * 30);

        for (int i = 0; i < initialVelocity; i++)
        {
            rb.AddForce(initialVelocity * Mathf.Pow(rb.velocity.magnitude, 0.15f) * rl.rotationVector.y * dir_paraller * 2f * Time.deltaTime);
            rb.AddForce(initialVelocity * Mathf.Pow(rb.velocity.magnitude, 0.20f) * rl.rotationVector.x * dir_perpendicular * Time.deltaTime);

            yield return null;
        }
    }
}