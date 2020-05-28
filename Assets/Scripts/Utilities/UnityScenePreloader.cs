using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public static class UnityScenePreloader
    {
#if UNITY_EDITOR
        //comment to test only one scene?
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitLoadingScene()
        {
            Debug.Log("InitLoadingScene()");
            var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        
            if (sceneIndex == 0) return;
            Debug.Log("Loading _preload scene");
            SceneManager.LoadScene(0); //_preload scene should be first in scene build list!!!
        }
#endif
    }
}
