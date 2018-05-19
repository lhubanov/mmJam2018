using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MommaPower : MonoBehaviour {

	
	
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
        StartCoroutine(poisonHealth());//Starts the loss of health process.
		}
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.W))
		{
			pupCurrentHealth += 10;
			PupSlider.value = pupCurrentHealth;
		}

		if (Input.GetKeyDown(KeyCode.A))
		{
			mommaCurrentHealth += 10;
			MommaSlider.value = mommaCurrentHealth;
			
			pupCurrentHealth -= 10;
			PupSlider.value = pupCurrentHealth;
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			mommaCurrentHealth -= 10;
			MommaSlider.value = mommaCurrentHealth;
		}
	}

	IEnumerator poisonHealth ()//Lose health momma
     {
         while(mommaCurrentHealth > 0)//While momma ain't ded
		 {
             mommaCurrentHealth -= 5;    //lose this health per step.
			 MommaSlider.value = mommaCurrentHealth;
             yield return new WaitForSeconds(1);//sets the time to lose health over.
		 }
             
	 }
}
