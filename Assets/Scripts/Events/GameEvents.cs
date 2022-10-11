using UnityEngine;

namespace Events
{
    public class GameEvents : MonoBehaviour
    {
        public delegate void PlayClip(AudioClip clip);

        public delegate void OnLight(bool l);
        public delegate void OnDark(bool l);
        public static PlayClip OnAudioCollisionEvent;
        public static OnLight OnInLightEvent;
        public static OnDark OnInDarkEvent;
        
  
    }
}
