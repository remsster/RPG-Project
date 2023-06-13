using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup canvasGroup;
        private Coroutine currentlyActiveFade;

        // ---------------------------------------------------------------------------------
        // Unity Engine Methods
        // ---------------------------------------------------------------------------------

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        // ---------------------------------------------------------------------------------
        // Custom Methods
        // ---------------------------------------------------------------------------------

        private IEnumerator FadeOutIn()
        {
            yield return FadeOut(3f);
            yield return FadeIn(2f);
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        public Coroutine Fade(float target, float time)
        {
            if (currentlyActiveFade != null)
            {
                StopCoroutine(currentlyActiveFade);
            }
            currentlyActiveFade = StartCoroutine(FadeRoutine(target, time));
            return currentlyActiveFade;
        }

        public Coroutine FadeOut(float time) => Fade(1, time);
        public Coroutine FadeIn(float time) => Fade(0, time);
    }
}
