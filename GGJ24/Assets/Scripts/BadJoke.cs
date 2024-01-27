using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadJoke : Clickable
{
    [SerializeField] 
    string jokeString = "This is a joke";

    [SerializeField]
    string responseString = "hahaha";

    [SerializeField]
    Effect onClick;

    [SerializeField]
    Effect onDefeat;

    public string GetJoke() => jokeString;
    public string GetAnswer() => responseString;
    public bool CheckResponded(string input)
    {
        bool defeated = input.Contains(responseString);
        Debug.Log($"Is {responseString} part of {input}? {(defeated ? "yes" : "no")}");
        if (defeated) OnDefeat();
        return defeated;        
    }

    public override void OnClick()
    {
        onClick?.OnTrigger();
    }
    public void OnDefeat()
    {
        onDefeat?.OnTrigger();
    }
}
