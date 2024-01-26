using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField]
    ParticleSystem emitParticles;

    [SerializeField]
    List<GameObject> toggleObjects = new List<GameObject>();
    public void OnTrigger()
    {
        if(emitParticles) emitParticles.Emit(10);
        foreach(GameObject go in toggleObjects)
        {
            if (!go) continue;
            go.SetActive(!go.activeSelf);
        }
    }
}
