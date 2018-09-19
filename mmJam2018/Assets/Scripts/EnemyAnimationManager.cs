using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    public void PlayDeathAnimation()
    {
        StartCoroutine(DisappearIntoTheVoid(0.75f));
    }

    private IEnumerator DisappearIntoTheVoid(float delay)
    {
        GetComponent<Animator>().Play("enemyDie");
        yield return new WaitForSeconds(delay);
        this.gameObject.SetActive(false);
        Object.Destroy(this.transform.parent.gameObject);
    }
}
