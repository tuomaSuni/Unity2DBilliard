using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockLogic : MonoBehaviour
{
    [Header("Dependencies")]
    public stateManager sm;
    
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
    
    private SpriteRenderer sr;
    private HashSet<Collider2D> collidersInContact = new HashSet<Collider2D>();
    private AudioSource audiosource;

    private lineLogic ll;

    private Rigidbody2D rb;
    private Vector2 dir;

    void Awake()
    {
        rb   = GetComponent<Rigidbody2D>();
        sr   = GetComponent<SpriteRenderer>();
        ll   = line.GetComponent<lineLogic>();

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

    private void HandlePushForce()
    {
        if (initialClickReleased && Input.GetMouseButton(0) && pushForce < maxPushForce && sm.AllBallsHasStopped() && !sm.isInteractable)
        {
            pushForce += Time.deltaTime * 30;
            cue.transform.localPosition += Vector3.left * Time.deltaTime * 2;
        }
    }

    private void HandleMovementState()
    {
        if (!isOnHand && sm.AllBallsHasStopped())
        {
            SetVisibility(true);
        }

        if (transform.localScale == new Vector3(0.30f, 0.30f, 0.30f))
        {
            SetVisibility(false);
            this.enabled = false;
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
        SetVisibility(false);
        cue.transform.localPosition = new Vector3(-18.75f, 0f, 0f);
        
        dir = (ll.mousePosition - rb.position).normalized;
        rb.velocity = dir * pushForce;

        if (pushForce > 0)
        {
            audiosource.volume = pushForce / maxPushForce;
            audiosource.pitch = Random.Range(0.95f, 1.05f);
            audiosource.Play();
        }

        pushForce = 0;
    }

    private void ChargeShot()
    {
        pushForce = maxPushForce;
        cue.transform.localPosition += Vector3.left * 4;
    }

    private void SetVisibility(bool visibility)
    {
        cue.SetActive(visibility);
        line.SetActive(visibility);
        sm.isInteractable = visibility;
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

    // INPUT LOGIC //
    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) && isOnHand && isFree && sm.AllBallsHasStopped() && sm.HasGameEnded == false)
        {
            StartAiming();
        }

        if (Input.GetMouseButtonUp(0) && !isOnHand && sm.AllBallsHasStopped())
        {
            initialClickReleased = true;
        }

        if (Input.GetMouseButtonUp(0) && !isOnHand && initialClickReleased && sm.AllBallsHasStopped())
        {
            Shoot();
        }

        if (!isOnHand && Input.GetMouseButtonDown(1) && sm.AllBallsHasStopped() && !sm.isInteractable)
        {
            ChargeShot();
        }

        if (!isOnHand && Input.GetMouseButtonUp(1) && sm.AllBallsHasStopped())
        {
            Shoot();
        }
    }
    // INPUT LOGIC //
}