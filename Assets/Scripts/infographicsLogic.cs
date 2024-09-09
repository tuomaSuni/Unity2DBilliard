using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class infographicsLogic : MonoBehaviour
{
    private TextMeshProUGUI tmpText;
    [TextArea]
    [SerializeField] string[] infoTexts;

    void Start()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
        tmpText.text = infoTexts[PlayerPrefs.GetInt("Type")];
    }

    void Update()
    {
        Color textColor = tmpText.color;
        textColor.a -= 0.002f;

        tmpText.color = textColor;

        if (textColor.a < 0)
        {
            gameObject.SetActive(false);
        }
    }
}
