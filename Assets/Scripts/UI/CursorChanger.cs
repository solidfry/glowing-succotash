using UnityEngine;

namespace UI
{
    public class CursorChanger : MonoBehaviour
    {
        public static CursorChanger instance = null;
        [SerializeField] private Texture2D image;

        void Start()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            } else if (instance != null)
            {
                Destroy(this.gameObject);
            }
            
            
            Cursor.SetCursor(image, Vector2.zero , CursorMode.ForceSoftware); 
        }
    }
}
