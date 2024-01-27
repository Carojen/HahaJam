using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    Camera _mainCamera;

    [SerializeField]
    TMPro.TextMeshProUGUI jokeField;
    [SerializeField]
    TMPro.TextMeshProUGUI secondJokeField;
    [SerializeField]
    Image jokeBackground;

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

    [SerializeField]
    List<Color> backgroundColors = new();
    [SerializeField]
    List<Color> textColors = new();

    public struct ColorScheme
    {
        public Color textColor;
        public Color backgroundColor;
    }
    private void ChangeColorScheme()
    {
        ColorScheme scheme = new ColorScheme();
        scheme.textColor = Color.white;
        scheme.backgroundColor = Color.green;
        if (backgroundColors.Count > 0)
        {
            scheme.backgroundColor = backgroundColors[Random.Range(0, backgroundColors.Count)];
        }
        if (textColors.Count > 0)
        {
            scheme.textColor = textColors[Random.Range(0, textColors.Count)];
        }

        jokeField.color = scheme.textColor;
        secondJokeField.color = scheme.textColor;
        jokeBackground.material.color = scheme.backgroundColor;
    }


    private void Start()
    {
        defaultResponseColor = Color.white;
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
        if(activeTarget == null && Input.GetKeyDown(KeyCode.Return))
        {
            GetComponent<JokeSpawner>().SpawnJoke();
        }

        lastKey = Input.inputString;

        if (activeTarget is BadJoke && lastKey != "")
        {
            BadJoke joke = activeTarget as BadJoke;

            if (lastKey[0] == (char)8 && currentInput.Length > 0)
            {
                currentInput = currentInput.Remove(currentInput.Length - 1);                
            }
            else if(lastKey != "\n")
            {
                currentInput += lastKey;
            }
            laughField.text = currentInput;

            if (joke.CheckFail(currentInput))
            {
                laughField.color = wrongResponseColor;
                laughField.alpha = 1.0f;
            }
            else
            {
                laughField.color = defaultResponseColor;
            }
            if (joke.CheckResponded(currentInput))
            {
                activeTarget = null;
                answerField.text = "";
                jokeField.text = "JOKE DEFEATED";
                secondJokeField.text = "";
                laughField.text = "";
                currentInput = "";
            }
        }
    }

    public void SetActiveJoke(BadJoke newTarget)
    {
        currentInput = "";
        activeTarget = newTarget;
        string[] jokeParts = newTarget.GetJoke();
        jokeField.text = jokeParts.Length > 0 ? jokeParts[0] : "???";
        secondJokeField.text = jokeParts.Length > 1 ? jokeParts[1] : "???";
        answerField.text = newTarget.GetAnswer();
        ChangeColorScheme();
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
                currentInput = "";
                activeTarget = newTarget;
                (newTarget as Clickable).OnClick();
                if (newTarget is BadJoke)
                {
                    SetActiveJoke(newTarget as BadJoke);
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
