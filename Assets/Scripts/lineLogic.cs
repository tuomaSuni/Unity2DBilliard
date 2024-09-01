using UnityEngine;

public class lineLogic : MonoBehaviour
{
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform startPoint;
    [SerializeField] private float dotSpacing;

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        float distance = Vector3.Distance(startPoint.position, mousePosition);
        int dotCount = Mathf.CeilToInt(distance / dotSpacing);

        Vector3 direction = (mousePosition - startPoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        for (int i = 0; i <= dotCount; i++)
        {
            Vector3 dotPosition = Vector3.Lerp(startPoint.position, mousePosition, i / (float)dotCount);
            GameObject dot = Instantiate(dotPrefab, dotPosition, Quaternion.Euler(0, 0, angle), transform);
        }

        startPoint.rotation = Quaternion.Euler(0, 0, angle);
    }
}
