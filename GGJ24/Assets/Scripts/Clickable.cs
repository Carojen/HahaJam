using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : Interactable
{    
    public virtual void OnClick()
    {
        Debug.Log("Clicked!");
        Act();
    }
}
