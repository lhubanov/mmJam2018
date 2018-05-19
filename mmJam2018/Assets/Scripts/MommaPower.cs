using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MommaPower : MonoBehaviour {

	
	
	public float mommaStartingHealth = 100;
	public float mommaCurrentHealth;
	public Slider MommaFulfilmentBar;



	// Use this for initialization
	void Start () {
		mommaCurrentHealth = MommaFulfilmentBar.value;
	}

	// Update is called once per frame
	void Update () {

		if (mommaCurrentHealth > 0)
		{
        StartCoroutine(poisonHealth());//Starts the loss of health process.
		}

		if (Input.GetKeyDown(KeyCode.A))
		{
			mommaCurrentHealth += 10;
			MommaFulfilmentBar.value = mommaCurrentHealth;
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			mommaCurrentHealth -= 10;
			MommaFulfilmentBar.value = mommaCurrentHealth;
		}
	}

	IEnumerator poisonHealth ()//Lose health momma
     {
         while(mommaCurrentHealth > 0)//While momma ain't ded
		 {
             mommaCurrentHealth -= 1;    //lose this health per step.
			 MommaFulfilmentBar.value = mommaCurrentHealth;
             yield return new WaitForSeconds(100);//sets the time to lose health over.
		 }
             
	 }
}
