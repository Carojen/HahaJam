using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    Camera _mainCamera;

    [SerializeField]
    TMPro.TextMeshProUGUI jokeField;

    [SerializeField]
    TMPro.TextMeshProUGUI answerField;

    [SerializeField]
    TMPro.TextMeshProUGUI laughField;

    [SerializeField]
    Color wrongResponseColor = Color.red;
    Color defaultResponseColor;

    Interactable activeTarget;
    string lastKey = "";
    string currentInput = "";

    private void Start()
    {
        defaultResponseColor = laughField.color;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryClick();
        }
        UpdateJokeMode();
    }

    private void UpdateJokeMode()
    {
        lastKey = Input.inputString;
        if (lastKey != "")
        {
            if (currentInput == "")
            {
                laughField.color = defaultResponseColor;
            }
            currentInput += lastKey;
            if (activeTarget is BadJoke && lastKey != "")
            {
                laughField.text = currentInput;

                BadJoke joke = activeTarget as BadJoke;

                if (joke.CheckFail(currentInput))
                {
                    laughField.text = "!! " + laughField.text + " !!";
                    laughField.color = wrongResponseColor;
                    laughField.alpha = 1.0f;            
                    currentInput = "";
                    lastKey = "";
                }
                else if (joke.CheckResponded(currentInput))
                {
                    activeTarget = null;
                    answerField.text = "";
                    jokeField.text = "JOKE DEFEATED";
                }
            }
        }
    }


    private void TryClick()
    {
        Ray clickRay = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit clicked;

        if (!Physics.Raycast(clickRay, out clicked))
            return;
        Interactable newTarget = clicked.collider.gameObject.GetComponentInParent<Interactable>();
        if (newTarget is Clickable)
        {
            if (newTarget != activeTarget)
            {
                activeTarget = newTarget;
                (newTarget as Clickable).OnClick();
                if (newTarget is BadJoke)
                {
                    BadJoke joke = newTarget as BadJoke;
                    jokeField.text = joke.GetJoke();
                    answerField.text = joke.GetAnswer();
                }
                else
                {
                    jokeField.text = "";
                    answerField.text = "";
                }
            }
        }
    }
}
