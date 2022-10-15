using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Utility
{
    public class SceneHelpers : MonoBehaviour
    {
        public static void Load(string sceneToLoad)
        {
            SceneManager.LoadScene(sceneToLoad);
        }

        
    }
}