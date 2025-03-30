using UnityEngine;

public class PlayerCustomizationController : Singleton<PlayerCustomizationController>
{
    [SerializeField] private RuntimeAnimatorController playerAnimatorController;
    private AnimatorOverrideController animatorOverrideController;
    public RuntimeAnimatorController PlayerAnimatorController => playerAnimatorController;

    protected override void Awake()
    {
        Observer.AddObserver(GameEvent.OnPlayerEquipHatItem, OnPlayerEquipHatItem);
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnPlayerEquipHatItem, OnPlayerEquipHatItem);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            
            Debug.Log("SWAP");
           
        }
    }

    void OnPlayerEquipHatItem(object[] datas)
    {
        HatCustomizeData data = (HatCustomizeData)datas[0];
        SetAnimationClip(data);
    }

    public void SetAnimationClip(HatCustomizeData data)
    {
        animatorOverrideController = new AnimatorOverrideController(playerAnimatorController);
        playerAnimatorController = animatorOverrideController;    
        animatorOverrideController["Hat Run"] = data.runClip == null ? null : data.runClip;
        animatorOverrideController["Hat Jump"] = data.jumpClip == null ? null : data.jumpClip;
        animatorOverrideController["Hat Die"] = data.dieClip == null ? null : data.dieClip;
    }

}
