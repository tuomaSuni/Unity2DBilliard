using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infBallLogic : ballLogic
{
    protected override void OnDisable()
    {
        base.OnDisable();

        if (AreAllSiblingsDisabled())
        {
            EnableAllSiblings();
        }
    }

    bool AreAllSiblingsDisabled()
    {
        foreach (Transform sibling in transform.parent)
        {
            if (sibling.gameObject.activeSelf)
            {
                return false;
            }
        }

        return true;
    }

    void EnableAllSiblings()
    {
        foreach (Transform sibling in transform.parent)
        {
            sibling.gameObject.SetActive(true);
        }
    }
}