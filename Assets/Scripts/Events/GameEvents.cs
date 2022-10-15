using ScriptableObjects;
using UnityEngine;

namespace Events
{
    public class GameEvents : MonoBehaviour
    {
//        public delegate void PlayClip(AudioClip clip);

        public delegate void CharacterDamaged(float normalisedValue);
        public delegate void CharacterDied();
        public delegate void ChargesCount(int count);
        public delegate void SetChargeCount(int count);
        public delegate void IsLit(bool lit);
        public delegate void NextLevel(string level);
        public delegate void SendGameData(GameData data);
        
//        public static PlayClip onAudioCollisionEvent;
        public static CharacterDamaged onCharacterDamagedEvent;
        public static CharacterDied onCharacterDiedEvent;
        public static SetChargeCount onSetChargesCountEvent;
        public static ChargesCount onChargesChangedEvent;
        public static IsLit onLitEvent;
        public static NextLevel onNextLevelEvent;
        public static SendGameData onSendGameDataEvent;
    }
}
