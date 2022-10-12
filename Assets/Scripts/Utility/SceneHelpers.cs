using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHelpers : MonoBehaviour
{
    public static void Load(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}