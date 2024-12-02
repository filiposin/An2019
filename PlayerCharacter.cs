using UnityEngine;

[RequireComponent(typeof(CharacterDriver))]
[RequireComponent(typeof(CharacterLiving))]
[RequireComponent(typeof(CharacterInventory))]
[RequireComponent(typeof(FPSController))]
[RequireComponent(typeof(CharacterInventory))]
public class PlayerCharacter : CharacterSystem
{
    [HideInInspector]
    public bool ToggleFlashlight;

    public Quaternion CameraRotation;

    private void Awake()
    {
        SetupAwake();
    }

    private void Start()
    {
        if ((bool)animator)
        {
            animator.SetInteger("Shoot_Type", AttackType);
        }
    }

    private void Update()
    {
        if (!(animator == null))
        {
            animator.SetInteger("UpperState", 1);
            UpdateFunction();
            if (Motor.controller.velocity.y < -20f)
            {
                ApplyDamage(10000, Motor.controller.velocity, string.Empty, string.Empty);
            }
        }
    }

    public override void PlayMoveAnimation(float magnitude)
    {
        if ((bool)animator)
        {
            if (magnitude > 0.4f)
            {
                animator.SetInteger("LowerState", 1);
            }
            else
            {
                animator.SetInteger("LowerState", 0);
            }
        }
        base.PlayMoveAnimation(magnitude);
    }

    public override void PlayAttackAnimation(bool attacking, int attacktype)
    {
        if ((bool)animator)
        {
            if (attacking)
            {
                animator.SetTrigger("Shoot");
            }
            animator.SetInteger("Shoot_Type", attacktype);
        }
        base.PlayAttackAnimation(attacking, attacktype);
    }

    public void ToggleFlash()
    {
        // Multiplayer logic removed; this method now simply toggles the flashlight.
        ToggleFlashlight = !ToggleFlashlight;
    }

    public override void OnThisThingDead()
    {
        if ((bool)UnitZ.playerSave)
        {
            UnitZ.playerSave.DeleteSave(UserID, CharacterKey, UserName);
        }
        CharacterItemDroper component = GetComponent<CharacterItemDroper>();
        if ((bool)component)
        {
            component.DropItem();
        }
        base.OnThisThingDead();
    }
}
