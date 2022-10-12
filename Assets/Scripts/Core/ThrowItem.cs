using Events;
using UnityEngine;

namespace Core
{
    public class ThrowItem : MonoBehaviour
    {
        [SerializeField] private int charges = 3;
        [SerializeField] private int maxCharges = 3;
        [SerializeField] GameObject prefabToThrow;
        [SerializeField] private float force;
        [SerializeField] private float torque = 10f;

        public int Charges
        {
            get => charges;
            set
            {
                charges = value;
                GameEvents.onChargesChangedEvent?.Invoke(value);
                Debug.Log(value);
            }
        }

        private void Start() => GameEvents.onSetChargesCountEvent?.Invoke(maxCharges);
        
        public void Fire(Vector2 direction)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (Charges > 0) 
                {
                    var item = Instantiate(prefabToThrow, transform.position, Quaternion.identity);
                    var itemRb = item.GetComponent<Rigidbody2D>();

                    itemRb.AddForce(direction * force);
                    itemRb.AddTorque(torque);
                    Charges--;
                }
                else
                {
                    Debug.Log("Out of Flares!");
                }
            }
        }
    }
}
