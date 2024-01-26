using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected void Act()
    {
        GetComponent<Effect>()?.OnTrigger();
    }
}
