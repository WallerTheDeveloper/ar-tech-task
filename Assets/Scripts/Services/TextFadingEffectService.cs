using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Services
{
    public class TextFadingEffectService : ITextFader
    {
        private readonly MonoBehaviour _controllerInstance;
        private readonly TextMeshProUGUI _textToFade;
        private readonly Queue<string> _textsContainer;
        private readonly string[] _displayedTexts;
        
        private List<Coroutine> _fadeCoroutinesList = new();

        private int _fadeEffectDuration;

        public TextFadingEffectService(
            MonoBehaviour controllerInstance,
            Queue<string> textsContainer, 
            string[] displayedTexts, 
            int fadeEffectDuration,
            TextMeshProUGUI textToFade)
        {
            _controllerInstance = controllerInstance;
            _textsContainer = textsContainer;
            _displayedTexts = displayedTexts;
            _fadeEffectDuration = fadeEffectDuration;
            _textToFade = textToFade;
        }
        public void StartShowing()
        {
            foreach (string text in _displayedTexts)
            {
                _textsContainer.Enqueue(text);
            }

            ShowNextText();
        }
        private IEnumerator PerformFadeEffectFor(int seconds, TextMeshProUGUI textToFade, string text)
        {
            textToFade.SetText(text);
            for (float i = 0; i <= seconds; i += Time.deltaTime)
            {
                textToFade.color = new Color(1, 1, 1, i);
                yield return null;
            }
            for (float i = seconds; i >= 0; i -= Time.deltaTime)
            {
                textToFade.color = new Color(1, 1, 1, i);
                yield return null;
            }
            ShowNextText();
        }
        private void ShowNextText()
        {
            if (_textsContainer.Count > 0)
            {
                StartFadeEffect();
            }
            else
            {
                StopFadeEffect();
            }
        }
        private void StartFadeEffect()
        {
            string nextText = _textsContainer.Dequeue();
            Coroutine currentFade =
                _controllerInstance.StartCoroutine(PerformFadeEffectFor(_fadeEffectDuration, _textToFade, nextText));
            _fadeCoroutinesList.Add(currentFade);
        }
        private void StopFadeEffect()
        {
            foreach (Coroutine fadeCoroutine in _fadeCoroutinesList)
            {
                _controllerInstance.StopCoroutine(fadeCoroutine);
            }

            Debug.Log("Text display coroutines stopped");
        }
    }

    public interface ITextFader
    {
        void StartShowing();
    }
}