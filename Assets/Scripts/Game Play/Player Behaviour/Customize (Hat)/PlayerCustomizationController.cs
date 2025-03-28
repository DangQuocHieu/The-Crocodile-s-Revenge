using UnityEngine;

public class PlayerCustomizationController : MonoBehaviour
{
    private Animator animator;
    AnimatorOverrideController animatorOverrideController;
    [SerializeField] HatCustomizeData[] datas;

    void Awake()
    {
        animator = GetComponent<Animator>();    
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            
            Debug.Log("SWAP");
            var data = datas[Random.Range(0, datas.Length)];
            animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = animatorOverrideController;    
            animatorOverrideController["Hat Run"] = data.runClip;
            animatorOverrideController["Hat Jump"] = data.jumpClip;
            animatorOverrideController["Hat Die"] = data.dieClip;
        }
    }

}
