using System.Collections.Generic;
using UnityEngine;

public class LineLogic : MonoBehaviour
{
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private GameObject endPointPrefab;
    [SerializeField] private Transform startPoint;
    [SerializeField] private float dotSpacing;

    private List<GameObject> dots = new List<GameObject>();
    private GameObject endPoint;
    
    private void Start()
    {
        // Instantiate and cache the endPointPrefab, since it's a single object
        endPoint = Instantiate(endPointPrefab, Vector3.zero, Quaternion.identity);
        endPoint.transform.SetParent(transform, false);
        endPoint.SetActive(false); // Initially inactive
    }

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // Calculate distance and number of dots
        float distance = Vector3.Distance(startPoint.position, mousePosition);
        int dotCount = Mathf.CeilToInt(distance / dotSpacing);

        // Adjust dots
        AdjustDots(dotCount);

        // Set positions for dots
        Vector3 direction = (mousePosition - startPoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        for (int i = 0; i < dotCount; i++)
        {
            Vector3 dotPosition = Vector3.Lerp(startPoint.position, mousePosition, i / (float)dotCount);
            dots[i].transform.position = dotPosition;
            dots[i].transform.rotation = Quaternion.Euler(0, 0, angle);
            dots[i].SetActive(true);
        }

        // Set position and rotation for the endpoint
        endPoint.transform.position = mousePosition;
        endPoint.transform.rotation = Quaternion.identity;
        endPoint.SetActive(true);

        // Update start point rotation
        startPoint.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void AdjustDots(int requiredCount)
    {
        // Deactivate excess dots
        if (dots.Count > requiredCount)
        {
            for (int i = requiredCount; i < dots.Count; i++)
            {
                dots[i].SetActive(false);
            }
            dots.RemoveRange(requiredCount, dots.Count - requiredCount);
        }

        // Create new dots if needed
        while (dots.Count < requiredCount)
        {
            GameObject newDot = Instantiate(dotPrefab);
            newDot.transform.SetParent(transform, false);
            newDot.SetActive(false);
            dots.Add(newDot);
        }
    }
}
