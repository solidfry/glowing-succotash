using Events;
using ScriptableObjects;
using UnityEngine;
using Utility;

public class LoadNextLevel : MonoBehaviour
{
    [SerializeField] private GameData gameData;

    private void OnEnable() => GameEvents.onSendGameDataEvent += SetGameData;
    
    private void OnDisable() => GameEvents.onSendGameDataEvent -= SetGameData;
    
    private void SetGameData(GameData data) => gameData = data;

    public void OnButtonPress()
    {
        SceneHelpers.Load(gameData.PreviousLevel);
        Debug.Log($"Attempting to load {gameData.PreviousLevel}");
    }

}
