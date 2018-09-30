using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayDeathAnimation()
    {
        StartCoroutine(DisappearIntoTheVoid(0.75f));
    }

    private IEnumerator DisappearIntoTheVoid(float delay)
    {
        animator.Play("enemyDie");
        yield return new WaitForSeconds(delay);
        this.gameObject.SetActive(false);
        Object.Destroy(this.transform.parent.gameObject);
    }
}
