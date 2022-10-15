using UnityEngine;

namespace ScriptableObjects
{
    public class GameData : ScriptableObject
    {
        [SerializeField] private string previousLevel, currentLevel;

        public string PreviousLevel
        {
            get => previousLevel;
            set => previousLevel = value;
        }
        
        public string CurrentLevel
        {
            get => currentLevel;
            set => currentLevel = value;
        }
    }
}