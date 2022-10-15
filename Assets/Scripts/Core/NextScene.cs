using System;
using Events;
using UnityEngine;

namespace Core
{
    public class NextScene : MonoBehaviour
    {
        [SerializeField] private string levelToLoad;
        private void OnTriggerEnter2D(Collider2D col) => GameEvents.onNextLevelEvent?.Invoke(levelToLoad);
    }
}