using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MommaPower : MonoBehaviour
{	
	public Slider PupSlider;
	public Slider MommaSlider;

	private float pupStartingHealth;
	private float pupCurrentHealth;
	private float mommaStartingHealth;
	private float mommaCurrentHealth;

	// Use this for initialization
	void Start () {
		mommaCurrentHealth = MommaSlider.value;
		pupCurrentHealth = PupSlider.value;

		if (mommaCurrentHealth > 0)
		{
            StartCoroutine(poisonHealth()); //Starts the loss of health process.
		}
	}

    public void UpdatePupCurrentHealth(float value)
    {
        pupCurrentHealth += value;
        PupSlider.value = pupCurrentHealth;
    }

    public void UpdateMommaCurrentHealth(float value)
    {
        mommaCurrentHealth += value;
        MommaSlider.value = mommaCurrentHealth;

        pupCurrentHealth -= value;
        PupSlider.value = pupCurrentHealth;
    }


	IEnumerator poisonHealth()
     {
         while(mommaCurrentHealth > 0)
		 {
             mommaCurrentHealth -= 3;
			 MommaSlider.value = mommaCurrentHealth;
             yield return new WaitForSeconds(1);
		 }
             
	 }
}
