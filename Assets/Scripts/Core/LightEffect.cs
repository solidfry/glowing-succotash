using Events;
using UnityEngine;

namespace Core
{
    public class LightEffect : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col) => GameEvents.OnInDarkEvent?.Invoke(false);
        private void OnTriggerExit2D(Collider2D other) => GameEvents.OnInDarkEvent?.Invoke(true);
    }
}