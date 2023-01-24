﻿using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        // ---------------------------------------------------------------------------------
        // Custom Methods
        // ---------------------------------------------------------------------------------


        // ---- Private ----
        private IEnumerator FadeOutIn()
        {
            yield return FadeOut(3f);
            print("Faded out");
            yield return FadeIn(2f);
            print("Faded in");

        }

        // ---- Public ----

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha < 1) // alpha is not 1
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null; // run on the next frame
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0) // alpha is not 1
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null; // run on the next frame
            }
        }
    }
}
