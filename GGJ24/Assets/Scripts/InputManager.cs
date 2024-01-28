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
    Image dadCharacter;

    [SerializeField]
    float maxAllowedSecondsToAnswer = 10f;

    [SerializeField]
    TMPro.TextMeshProUGUI timerField;

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
    float elapsedTime;
    float startTime;
    int numberOfJokesDone = 0;
    bool finished = false;

    [SerializeField]
    List<Sprite> backgrounds = new();
    [SerializeField]
    List<Color> textColors = new();
    [SerializeField]
    List<Sprite> dadSprites = new();

    private void ChangeColorScheme()
    {
        Color textColor = Color.white;
        Sprite background = jokeBackground.sprite;
        if (backgrounds.Count > 0)
        {
            background = backgrounds[Random.Range(0, backgrounds.Count)];
        }
        if (textColors.Count > 0)
        {
            textColor = textColors[Random.Range(0, textColors.Count)];
        }
        if (dadSprites.Count > 0)
        {
            dadCharacter.sprite = dadSprites[Random.Range(0, dadSprites.Count)];
        }

        jokeField.color = textColor;
        secondJokeField.color = textColor;
        jokeBackground.sprite = background;
    }


    private void Start()
    {
        defaultResponseColor = Color.white;
    }

    void Update()
    {
        if (finished) return;
        if (Input.GetMouseButtonDown(0))
        {
            TryClick();
        }

        UpdateJokeMode();
    }


    private void UpdateJokeMode()
    {
        if (activeTarget == null && Input.anyKeyDown)
        {
            GetComponent<JokeSpawner>().SpawnJoke();
            startTime = Time.time;
            elapsedTime = 0f;
            return;
        }
        elapsedTime = Time.time - startTime;
        if(activeTarget == null)
        {
            return;
        }
        timerField.text = (maxAllowedSecondsToAnswer - ((int)elapsedTime)).ToString();

        lastKey = Input.inputString;

        if (elapsedTime > maxAllowedSecondsToAnswer)
        {
            Destroy(activeTarget);
            activeTarget = null;
            elapsedTime = 0;
            ResetJoke();
            int jokesDone = numberOfJokesDone;
            Debug.Log($"Finished {numberOfJokesDone}");
            jokeField.text = $"You finished {jokesDone} jokes.";
            numberOfJokesDone = 0;
            return;
        }
        

        if (activeTarget is BadJoke && lastKey != "")
        {
            BadJoke joke = activeTarget as BadJoke;

            if (lastKey[0] == (char)8 && currentInput.Length > 0)
            {
                currentInput = currentInput.Remove(currentInput.Length - 1);
            }
            else if (lastKey != "\n")
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
                ResetJoke();
                numberOfJokesDone++;
                Debug.Log($"Jokes done: {numberOfJokesDone}");
                GetComponent<JokeSpawner>().SpawnJoke();
            }
        }
    }
    private void ResetJoke()
    {
        activeTarget = null;
        answerField.text = "Yes";
        jokeField.text = "Done!";
        secondJokeField.text = "Do you want to hear another joke?";
        laughField.text = "";
        currentInput = "";
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
