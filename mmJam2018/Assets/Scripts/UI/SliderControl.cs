using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.UI;

public class SliderControl : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private IUpdateUIFromStateMachine uiInstance;

    void Start ()
    {
        slider = GetComponent<Slider>();
        uiInstance = GetComponent<IUpdateUIFromStateMachine>();
	}
	
	void Update ()
    {
        slider.value = uiInstance.GetValueFromStateMachine();
	}
}
