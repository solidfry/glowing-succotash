using System;
using System.Collections;
using Events;
using UnityEngine;

namespace Core
{
    public class Health : MonoBehaviour
    {
        public int hitPoints;
        public int tickTime;
        [SerializeField] private bool isDecrementing;

        public int HitPoints {get => hitPoints; set => hitPoints = value; }

        public bool IsDecrementing
        {
            get => isDecrementing;
            set => isDecrementing = value;
        }
        
        private void Start() => StartCoroutine(DamageOverTime());

        private void OnEnable()
        {
            GameEvents.OnInLightEvent += SetIsDecrementing;
            GameEvents.OnInDarkEvent += SetIsDecrementing;
        }

        private void OnDisable()
        {
            GameEvents.OnInLightEvent -= SetIsDecrementing;
            GameEvents.OnInDarkEvent -= SetIsDecrementing;
        }

        IEnumerator DamageOverTime()
        {
            while (HitPoints > 0)
            {
                Debug.Log($"DamageOverTime ran and HP is {HitPoints}");
                if(isDecrementing) HitPoints--;
                yield return new WaitForSeconds(tickTime);
            }
        }

        void SetIsDecrementing(bool value) => IsDecrementing = value;
    }
}
