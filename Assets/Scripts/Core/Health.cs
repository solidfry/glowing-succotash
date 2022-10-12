using System.Collections;
using Events;
using UnityEngine;

namespace Core
{
    public class Health : MonoBehaviour
    {
        public int hitPoints;
        public int maxHitPoints = 100;
        public int tickTime;
        public int hitPointDecrementValue = 1;
        [SerializeField] private bool isDecrementing;

        public int HitPoints {
            get => hitPoints;
            set { 
                hitPoints = value;
                if (hitPoints >= maxHitPoints) hitPoints = maxHitPoints;
                if(hitPoints == 0) GameEvents.onCharacterDiedEvent?.Invoke() ;
            }
        }
        
        public bool IsDecrementing
        {
            get => isDecrementing;
            set
            {
                isDecrementing = value;
                if(isDecrementing)
                {
                    GameEvents.onLitEvent?.Invoke(false);
                }
                else
                {
                    GameEvents.onLitEvent?.Invoke(true);
                }
                
            }
        }

        public int HitPointDecrementValue
        {
            get => hitPointDecrementValue;
            set => hitPointDecrementValue = value;
        }

        private void Start() => StartCoroutine(DamageOverTime());
        
        //Todo: this should be moved to another file that handles if the character is in light or not
        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.CompareTag("Lights") && IsDecrementing == true) SetIsDecrementing(false);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Lights") && IsDecrementing == false) SetIsDecrementing(true);
        }

        IEnumerator DamageOverTime()
        {
            while (HitPoints > 0)
            {
                Debug.Log($"DamageOverTime ran and HP is {HitPoints}");
                if(isDecrementing)
                {
                    HitPoints -= HitPointDecrementValue;
                    float healthAsPercent = (float)HitPoints / (float)100;
                   GameEvents.onCharacterDamagedEvent?.Invoke(healthAsPercent);
                }
                yield return new WaitForSeconds(tickTime);
            }
        }

        void SetIsDecrementing(bool value) => IsDecrementing = value;
    }
}
