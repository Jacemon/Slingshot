using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    [RequireComponent(typeof(Animator))]
    public class SceneLoaderManager : MonoBehaviour
    {
        public float transitionTime;
        
        private Animator _transition;
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        private void Awake()
        {
            _transition = GetComponent<Animator>();
        }

        public void LoadSceneWithTransition(string sceneName)
        {
            StartCoroutine(LoadSceneWithTransitionCoroutine(sceneName));
        }

        private IEnumerator LoadSceneWithTransitionCoroutine(string sceneName)
        {

            _transition.SetBool(IsOpen, false);
            _transition.speed = 1 / transitionTime;

            yield return new WaitForSecondsRealtime(transitionTime);
            SceneManager.LoadSceneAsync(sceneName).completed +=
                _ =>
                {
                    _transition.SetBool(IsOpen, true);
                };
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
