using System.Collections;
using Events;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager manager = null;
        public string deathScreen = "Death";
        [SerializeField] private float delayLoad = 2f;
        private SceneHelpers sceneHelpers;
        [SerializeField] private GameData gameData;
        
        private void Awake()
        {
            if (manager == null)
            {
                manager = this;
                DontDestroyOnLoad(this.gameObject);
            } else if (manager != null)
            {
                Destroy(this.gameObject);
            }
            
            if(gameData == null)
                gameData = GameData.CreateInstance<GameData>();
        }

        private void OnEnable()
        {
            GameEvents.onCharacterDiedEvent += PlayerDead;
            GameEvents.onNextLevelEvent += LoadNextLevel;
            SceneManager.sceneLoaded += UpdateSceneValues;
        }

        private void OnDisable()
        {
            GameEvents.onCharacterDiedEvent -= PlayerDead;
            GameEvents.onNextLevelEvent -= LoadNextLevel;
            SceneManager.sceneLoaded -= UpdateSceneValues;
            RenewSessionData();
        }

        private void PlayerDead() => DelayLoad(deathScreen, delayLoad);

        private void LoadNextLevel(string level) => DelayLoad(level, delayLoad);
        
        public void DelayLoad(string sceneToLoad, float delay)
        {
            StartCoroutine(DelayLoadCoroutine(sceneToLoad, delay));
        }
        
        IEnumerator DelayLoadCoroutine(string sceneToLoad, float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneHelpers.Load(sceneToLoad);
        }
        
        private void RenewSessionData()
        {
            Debug.Log("Trying to destroy playerData");
            
            if(gameData != null)
                DestroyImmediate(gameData);
            
            gameData = ScriptableObject.CreateInstance<GameData>();
        }
        
        private void UpdateSceneValues(Scene scene, LoadSceneMode mode)
        {
            if (scene.name.Contains("Level"))
            {
                gameData.CurrentLevel = scene.name;
            }

            if (scene.name != gameData.PreviousLevel)
                gameData.PreviousLevel = gameData.CurrentLevel;
            
            GameEvents.onSendGameDataEvent?.Invoke(gameData);
        }

    }
}
