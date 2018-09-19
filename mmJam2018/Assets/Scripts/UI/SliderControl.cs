using UnityEngine;
using UnityEngine.UI;

public class SliderControl : MonoBehaviour
{
    private Slider slider = null;
    public StateMachine World;

    void Start ()
    {
        slider = GetComponent<Slider>();
	}
	
	void Update ()
    {
		if(gameObject.tag == "Player") {
            slider.value = World.HeldEnergy;
        } else {
            slider.value = World.MomHealth;
        }
	}
}
