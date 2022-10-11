using Core;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float speed;
        [ReadOnly] [SerializeField] Vector3 mousePos;
        private Animator animator;
        private ThrowItem throwItem;
        [ReadOnly][SerializeField] private Vector2 dir;
        private static readonly int Direction = Animator.StringToHash("Direction");

        private void Start()
        {
            throwItem = GetComponent<ThrowItem>();
            animator = GetComponent<Animator>();
        }


        private void Update()
        {
            
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            mousePos.Normalize();
            
            dir = Vector2.zero;
            if (Input.GetKey(KeyCode.A))
            {
                dir.x = -1;
                animator.SetInteger(Direction, 3);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                dir.x = 1;
                animator.SetInteger(Direction, 2);
            }

            if (Input.GetKey(KeyCode.W))
            {
                dir.y = 1;
                animator.SetInteger(Direction, 1);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                dir.y = -1;
                animator.SetInteger(Direction, 0);
            }
            
            dir.Normalize();
            throwItem.Fire(mousePos);
            animator.SetBool("IsMoving", dir.magnitude > 0);

            GetComponent<Rigidbody2D>().velocity = speed * dir;
        }
    }
}
