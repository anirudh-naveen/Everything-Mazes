using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValueManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text textField;
    [SerializeField] private float initialValue = 100;

    private void Reset()
    {
        slider = GetComponent<Slider>();
        textField = GetComponent<TMP_Text>();
    }    

    public void Start() {
        slider.value = initialValue;
        slider.onValueChanged.AddListener(ChangedSliderValue);
    }


    public void ChangedSliderValue(float value) {
        textField.SetText(value.ToString("F0"));
    }

}
