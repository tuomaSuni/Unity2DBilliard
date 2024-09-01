using System.Collections.Generic;
using UnityEngine;

public class LineLogic : MonoBehaviour
{
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private GameObject endPointPrefab;
    [SerializeField] private Transform startPoint;
    [SerializeField] private float dotSpacing;
    [SerializeField] private int initialPoolSize = 10;

    private Queue<GameObject> dotPool = new Queue<GameObject>();
    private GameObject endPoint;

    private void Start()
    {
        InitializeEndPoint();
        InitializeDotPool();
    }

    private void Update()
    {
        HandleLineRendering();
    }

    private void InitializeEndPoint()
    {
        endPoint = Instantiate(endPointPrefab, Vector3.zero, Quaternion.identity);
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
        Vector3 mousePosition = GetMouseWorldPosition();
        int dotCount = CalculateDotCount(mousePosition);

        UpdateDotPool(dotCount);
        UpdateDots(dotCount, mousePosition);
        UpdateEndPoint(mousePosition);
        UpdateStartPoint(mousePosition);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 1f; // Ensure the z-position is set correctly for 2D
        return mousePosition;
    }

    private int CalculateDotCount(Vector3 mousePosition)
    {
        float distance = Vector3.Distance(startPoint.position, mousePosition);
        return Mathf.CeilToInt(distance / dotSpacing);
    }

    private void UpdateDotPool(int requiredCount)
    {
        // Add more dots if needed
        while (dotPool.Count < requiredCount)
        {
            CreateAndQueueDot();
        }

        // Deactivate excess dots
        foreach (GameObject dot in dotPool)
        {
            dot.SetActive(false);
        }

        // Activate dots needed
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

    private void UpdateDots(int dotCount, Vector3 mousePosition)
    {
        Vector3 direction = (mousePosition - startPoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        int index = 0;
        foreach (GameObject dot in dotPool)
        {
            if (index >= dotCount) break;

            Vector3 dotPosition = Vector3.Lerp(startPoint.position, mousePosition, (index + 1) / (float)dotCount);
            dot.transform.position = dotPosition;
            dot.transform.rotation = Quaternion.Euler(0, 0, angle);

            index++;
        }
    }

    private void UpdateEndPoint(Vector3 mousePosition)
    {
        endPoint.transform.position = mousePosition;
        endPoint.transform.rotation = Quaternion.identity;
        endPoint.SetActive(true);
    }

    private void UpdateStartPoint(Vector3 mousePosition)
    {
        Vector3 direction = (mousePosition - startPoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        startPoint.rotation = Quaternion.Euler(0, 0, angle);
    }
}
