using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JokeSpawner : MonoBehaviour
{
    [SerializeField]
    Transform jokeParent;

    [SerializeField]
    TextAsset jokeAsset;
    [SerializeField]
    TextAsset laughAsset;

    [SerializeField]
    GameObject jokeTemplate;

    List<string[]> jokeBank = new();
    List<string> laughBank = new();
    void Start()
    {
        InitializeJokes();
    }

    private void InitializeJokes()
    {
        string[] jokes = jokeAsset.ToString().Split("\n");
        string[] laughs = laughAsset.ToString().Split("\n");
        foreach (string laugh in laughs)
        {
            if (laugh == "") continue;
            laughBank.Add(laugh);
        }
        foreach (string joke in jokes)
        {
            if (joke == "") continue;
            string[] jokeParts = joke.Split(";");
            jokeBank.Add(jokeParts);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("j"))
        {
            SetupJoke(jokeBank[Random.Range(0, jokeBank.Count)]);
        }
    }

    public void SpawnJoke()
    {
        SetupJoke(jokeBank[Random.Range(0, jokeBank.Count)]);
    }

    private void SetupJoke(string[] joke)
    {
        BadJoke newJoke = Instantiate(jokeTemplate).GetComponent<BadJoke>();
        newJoke.transform.SetParent(jokeParent);
        Vector3 position = Vector3.zero;
        position.x += Random.Range(-2f, 2f);
        position.z += Random.Range(-2f, 2f);
        newJoke.transform.SetPositionAndRotation(position, Quaternion.identity);
        newJoke.Setup(joke, laughBank[Random.Range(0, laughBank.Count)]);        
    }
}
