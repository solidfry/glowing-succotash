using System.Collections;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Slider hpSlider;
        [SerializeField] private float updateSpeedSeconds = 0.5f;

        [Header("Slider settings")] 
        [SerializeField] private Color sliderWarningColor = Color.red;

        [SerializeField] private float sliderWarningValue = 0.3f;

        private void Awake()
        {
            if(hpSlider == null)
                hpSlider = GetComponentInChildren<Slider>();
        }

        private void OnEnable()
        {
            GameEvents.onCharacterDamagedEvent += ChangeSlider;
        }
        
        private void OnDisable()
        {
            GameEvents.onCharacterDamagedEvent -= ChangeSlider;

        }

        private void ChangeSlider(float normalisedValue)
        {
            StartCoroutine(AnimateSlider(normalisedValue));
        }

        private IEnumerator AnimateSlider(float normalisedValue)
        {
            Debug.Log("Coroutine is running to animate slider");
            float preChangedPercent = hpSlider.value;
            float elapsed = 0f;
            while (elapsed < updateSpeedSeconds)
            {
                elapsed += Time.deltaTime;
                hpSlider.value = Mathf.Lerp(preChangedPercent, normalisedValue, elapsed / updateSpeedSeconds);
                if (hpSlider.value <= sliderWarningValue)
                    hpSlider.fillRect.gameObject.GetComponent<Image>().color = sliderWarningColor;
            
                yield return null;
            }
            hpSlider.value = normalisedValue;
        }
    }
}