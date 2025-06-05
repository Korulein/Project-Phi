using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    public Animator animator;

    public void TriggerActive()
    {
        animator.SetTrigger("Activate");
    }
}

