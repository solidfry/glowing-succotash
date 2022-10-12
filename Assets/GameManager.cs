using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string deathScreen = "Death";
    [SerializeField] private float delayLoad = 2f;
    
    private void OnEnable()
    {
        GameEvents.onCharacterDiedEvent += PlayerDead;
    }

    private void OnDisable()
    {
        GameEvents.onCharacterDiedEvent -= PlayerDead;
    }

    private void PlayerDead() => StartCoroutine(DelayLoad(deathScreen, delayLoad));

    IEnumerator DelayLoad(string sceneToLoad, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneHelpers.Load(sceneToLoad);
    }

}
