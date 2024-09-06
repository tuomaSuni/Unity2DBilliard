using System.Collections.Generic;
using UnityEngine;

public class lineLogic : MonoBehaviour
{
    [Header("Dependencies")]
    public stateManager sm;

    [Header("Utilities")]
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private GameObject endPointPrefab;
    [SerializeField] private Transform startPoint;
    [SerializeField] private float dotSpacing;
    private int initialPoolSize = 10;

    private Queue<GameObject> dotPool = new Queue<GameObject>();
    private GameObject endPoint;
    [HideInInspector] public Vector2 mousePosition;

    private void Start()
    {
        InitializeEndPoint();
        InitializeDotPool();
    }

    private void Update()
    {
        bool isSettingRotation = sm.isSettingRotation;
        bool noMouseButtonsPressed = !Input.GetMouseButton(0) && !Input.GetMouseButton(1);

        if (sm.isSettingRotation || noMouseButtonsPressed)
        {
            HandleLineRendering();
        }
    }

    private void InitializeEndPoint()
    {
        endPoint = Instantiate(endPointPrefab, Vector2.zero, Quaternion.identity);
        endPoint.transform.SetParent(transform, false);
        endPoint.SetActive(false); // Initially inactive
    }

    private void InitializeDotPool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateAndQueueDot();
        }
    }

    private void CreateAndQueueDot()
    {
        GameObject newDot = Instantiate(dotPrefab);
        newDot.transform.SetParent(transform, false);
        newDot.SetActive(false);
        dotPool.Enqueue(newDot);
    }

    private void HandleLineRendering()
    {
        mousePosition = GetMouseWorldPosition();
        int dotCount = CalculateDotCount(mousePosition);

        // Ensure one less dot is rendered so the last dot doesn't coincide with the end point
        dotCount = Mathf.Max(dotCount - 1, 0);

        UpdateDotPool(dotCount);
        UpdateDots(dotCount, mousePosition);
        UpdateEndPoint(mousePosition);
        UpdateStartPoint(mousePosition);
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector3 mousePosition3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2(mousePosition3D.x, mousePosition3D.y);
    }

    private int CalculateDotCount(Vector2 mousePosition)
    {
        float distance = Vector2.Distance(startPoint.position, mousePosition);
        return Mathf.CeilToInt(distance / dotSpacing);
    }

    private void UpdateDotPool(int requiredCount)
    {
        // Add more dots if needed
        while (dotPool.Count < requiredCount)
        {
            CreateAndQueueDot();
        }

        // Deactivate all dots initially
        foreach (GameObject dot in dotPool)
        {
            dot.SetActive(false);
        }

        // Activate only the necessary dots
        int index = 0;
        foreach (GameObject dot in dotPool)
        {
            if (index < requiredCount)
            {
                dot.SetActive(true);
            }
            index++;
        }
    }

    private void UpdateDots(int dotCount, Vector2 mousePosition)
    {
        Vector2 direction = (mousePosition - (Vector2)startPoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        int index = 0;
        foreach (GameObject dot in dotPool)
        {
            if (index >= dotCount) break;

            Vector2 dotPosition = Vector2.Lerp(startPoint.position, mousePosition, (index + 1) / (float)(dotCount + 1));
            dot.transform.position = new Vector3(dotPosition.x, dotPosition.y, dot.transform.position.z);
            dot.transform.rotation = Quaternion.Euler(0, 0, angle);

            index++;
        }
    }

    private void UpdateEndPoint(Vector2 mousePosition)
    {
        endPoint.transform.position = new Vector3(mousePosition.x, mousePosition.y, endPoint.transform.position.z);
        endPoint.transform.rotation = Quaternion.identity;
        endPoint.SetActive(true);
    }

    private void UpdateStartPoint(Vector2 mousePosition)
    {
        Vector2 direction = (mousePosition - (Vector2)startPoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        startPoint.rotation = Quaternion.Euler(0, 0, angle);
    }
}
