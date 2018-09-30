using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.UI;

public class SliderControl : MonoBehaviour
{
    private Slider slider = null;
    // I'm a bit torn about this being private and auto-fetched,
    // as there is a dependency on an IUpdateUI type being attached to
    // gameObject as well.
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
