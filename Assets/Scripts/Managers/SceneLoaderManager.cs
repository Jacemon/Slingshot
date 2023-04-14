using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    [RequireComponent(typeof(Animator))]
    public class SceneLoaderManager : MonoBehaviour
    {
        public float transitionTime;
        public AudioSource transitionSound;

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
            _transition.speed = 1 / transitionTime;

            _transition.SetBool(IsOpen, false);
            transitionSound.Play();

            yield return new WaitForSecondsRealtime(transitionTime);
            SceneManager.LoadSceneAsync(sceneName).completed +=
                _ =>
                {
                    _transition.SetBool(IsOpen, true);
                    transitionSound.Play();
                };
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
