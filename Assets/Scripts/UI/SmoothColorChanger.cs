using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class SmoothColorChanger : MonoBehaviour
    {
        [SerializeField] private float _colorTransitionDuration = 0.5f;
        [SerializeField] private float _colorChangeInterval = 1.0f; 

        private TextMeshProUGUI textMeshPro; 

        private Color currentColor;
        private Color targetColor;
        
        private float timeSinceLastChange = 0.0f;
        
        private void Start()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();

            StartCoroutine(ChangeColorSmoothly());
        }

        private IEnumerator ChangeColorSmoothly()
        {
            while (true)
            {
                targetColor = new Color(Random.value, Random.value, Random.value, 1.0f);

                currentColor = textMeshPro.color;

                float step = 0.0f;
                float t = 0.0f;

                while (step < 1.0f)
                {
                    t += Time.deltaTime;

                    step = Mathf.Clamp01(t / _colorTransitionDuration);

                    Color lerpedColor = Color.Lerp(currentColor, targetColor, step);

                    textMeshPro.color = lerpedColor;

                    yield return null;
                }

                yield return new WaitForSeconds(_colorChangeInterval);
            }
        }
    }
}