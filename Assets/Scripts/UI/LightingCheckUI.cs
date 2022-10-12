using System;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LightingCheckUI : MonoBehaviour
    {
        [ReadOnly][SerializeField] private Image lightImage;
        [SerializeField] private Sprite litImage, unlitImage;
        private void Start()
        {
            lightImage = GetComponent<Image>();
        }

        private void OnEnable() => GameEvents.onLitEvent += CheckLighting;

        private void OnDisable() => GameEvents.onLitEvent -= CheckLighting;

        private void CheckLighting(bool lit)
        {
            print($"Lit value is {lit}");
            lightImage.sprite = lit ? litImage : unlitImage;
        }
    }
}