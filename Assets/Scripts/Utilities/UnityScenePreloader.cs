using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public static class UnityScenePreloader
    {
    
#if UNITY_EDITOR 
        public static int otherScene = -2;

        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitLoadingScene()
        {
            Debug.Log("InitLoadingScene()");
        
            var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        
            if (sceneIndex == 0) return;

            Debug.Log("Loading _preload scene");
        
            otherScene = sceneIndex;
            //_preload scene should be first in scene build list!!!
            SceneManager.LoadScene(0); 
        }
#endif
    
    }
}
