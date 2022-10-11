using UnityEngine;

namespace Events
{
    public class GameEvents : MonoBehaviour
    {
        public delegate void PlayClip(AudioClip clip);
        
        public static PlayClip OnAudioCollisionEvent;
    }
}
