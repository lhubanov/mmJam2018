using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public void AnimateLifeDrain()
    {
        GetComponentInParent<Animator>().Play("drainLife");
    }
}
