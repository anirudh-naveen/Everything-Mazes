using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    [SerializeField] public Slider slider;
    [SerializeField] private TMP_Text textField;

    private void Reset()
    {
        slider = GetComponent<Slider>();
        textField = GetComponent<TMP_Text>();
    }    

    public void Start() {
        slider.value = 10;
        slider.onValueChanged.AddListener(ChangedSliderValue);
    }

    public void ChangedSliderValue(float value) {
        textField.SetText(value.ToString("F0"));
    }

}
