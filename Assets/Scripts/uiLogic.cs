using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiLogic : MonoBehaviour
{
    [SerializeField] private uiManager uim;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) uim.GameEnd();
    }
}
