using UnityEngine;

public class AnimalCharacter : CharacterSystem
{
	public float DamageDirection = 0.5f;

	public float Force = 70f;

	private void Awake()
	{
		SetupAwake();
	}

	private void Start()
	{
		base.transform.position += Vector3.up;
	}

	private void Update()
	{
		UpdateFunction();
		Motor.inputJump = false;
		if (Motor.controller.velocity.y < -20f)
		{
			ApplyDamage(10000, Motor.controller.velocity, string.Empty, string.Empty);
		}
		if (Motor.inputJump)
		{
			Motor.inputJump = false;
		}
	}

	public void Leap()
	{
	}

	public void AfterAttack()
	{
	}

	public void DoAttack()
	{
		DoOverlapDamage(base.transform.position + DamageOffset, base.transform.forward * Force, Damage, DamageLength, DamageDirection, string.Empty, Team);
	}

	public override void PlayMoveAnimation(float magnitude)
	{
		if ((bool)animator)
		{
			if (magnitude > 0.4f)
			{
				animator.SetInteger("StateAnimation", 1);
			}
			else
			{
				animator.SetInteger("StateAnimation", 0);
			}
		}
		base.PlayMoveAnimation(magnitude);
	}

	public override void PlayAttackAnimation(bool attacking, int attacktype)
	{
		if ((bool)animator && attacking)
		{
			animator.SetTrigger("Attacking");
			spdMovAtkMult = 0f;
		}
		base.PlayAttackAnimation(attacking, attacktype);
	}
}
