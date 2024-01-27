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

    Interactable activeTarget;
    string lastKey = "";
    string currentInput = "";

    void Start()
    {

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
            currentInput += lastKey;
            if (activeTarget is BadJoke)
            {
                laughField.text = currentInput;
                BadJoke joke = activeTarget as BadJoke;
                Debug.Log("This is a really bad joke");
                if (joke.CheckResponded(currentInput))
                {
                    Debug.Log("Joke defeated!");
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
