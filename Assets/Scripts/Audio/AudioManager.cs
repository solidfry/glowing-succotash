using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager audioManager = null;

        private void Awake()
        {
            if (audioManager == null)
            {
                audioManager = this;
                DontDestroyOnLoad(this.gameObject);
            } else if (audioManager != null)
            {
                Destroy(this.gameObject);
            }
        }
    }
}