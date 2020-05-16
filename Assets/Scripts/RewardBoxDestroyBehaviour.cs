using UnityEngine;

public class RewardBoxDestroyBehaviour : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        GameObject.Find("BoxOpeningRewardHandler").GetComponent<RewardBoxOpenScript>().TossCoinsAnimation();
        GameObject.Destroy(animator.gameObject);
    }
}
