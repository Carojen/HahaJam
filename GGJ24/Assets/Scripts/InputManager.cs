using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    Camera _mainCamera;
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.E))
        {
            Debug.Log("Did someone press E?");            
        }
        if(Input.GetMouseButtonDown(0))
        {
            TryClick();
        }
    }

    private void TryClick()
    {
        Ray clickRay = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit clicked;

        if (!Physics.Raycast(clickRay, out clicked))
            return;
        Interactable go = clicked.collider.gameObject.GetComponentInParent<Interactable>();
        if (go is Clickable)
            (go as Clickable).OnClick();       
    }
}
