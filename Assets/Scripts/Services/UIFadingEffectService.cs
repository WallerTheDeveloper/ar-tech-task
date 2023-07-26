using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Services
{
    public class UIFadingEffectService
    {
        private readonly MonoBehaviour _userInstance;
        private readonly TextMeshProUGUI _userMissionText;
        private readonly Queue<string> _instructionTextContainer;
        
        private List<Coroutine> _fadeCoroutines = new();

        private int _fadeEffectDuration;
        private readonly string[] _textsToDisplay;

        public UIFadingEffectService(
            MonoBehaviour userInstance,
            Queue<string> instructionTextContainer, 
            string[] textsToDisplay, 
            int fadeEffectDuration,
            TextMeshProUGUI userMissionText)
        {
            _userInstance = userInstance;
            _instructionTextContainer = instructionTextContainer;
            _textsToDisplay = textsToDisplay;
            _fadeEffectDuration = fadeEffectDuration;
            _userMissionText = userMissionText;
        }
        public void AddTextToQueue()
        {
            foreach (string text in _textsToDisplay)
            {
                _instructionTextContainer.Enqueue(text);
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
            if (_instructionTextContainer.Count > 0)
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
            string nextText = _instructionTextContainer.Dequeue();
            Coroutine currentFade =
                _userInstance.StartCoroutine(PerformFadeEffectFor(_fadeEffectDuration, _userMissionText, nextText));
            _fadeCoroutines.Add(currentFade);
        }
        private void StopFadeEffect()
        {
            foreach (Coroutine fadeCoroutine in _fadeCoroutines)
            {
                _userInstance.StopCoroutine(fadeCoroutine);
            }

            MonoBehaviour.print("Text display coroutines stopped");
        }
    }
}