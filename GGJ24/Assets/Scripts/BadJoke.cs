using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadJoke : Clickable
{
    [SerializeField] 
    string[] jokeString = { "This is a joke", "Really?" };

    [SerializeField]
    string responseString = "hahaha";

    [SerializeField]
    Effect onClick;

    [SerializeField]
    Effect onDefeat;    

    public string[] GetJoke() => jokeString;
    public string GetAnswer() => responseString;
    public bool CheckResponded(string input)
    {
        bool defeated = input.Contains(responseString);        
        if (defeated) OnDefeat();
        return defeated;        
    }

    public void Setup(string[] joke, string laugh)
    {
        jokeString = joke;
        responseString = laugh;
    }

    public bool CheckFail(string input)
    {
        return !responseString.Contains(input);
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
