using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterMotor))]
public class CharacterFootStep : MonoBehaviour
{
	private AudioSource audios;

	private CharacterMotor motor;

	private float delay;

	public AudioClip[] FoodSteps;

	public float Delay = 3f;

	private void Start()
	{
		motor = base.gameObject.GetComponent<CharacterMotor>();
		audios = base.gameObject.GetComponent<AudioSource>();
	}

	private void PlaySound()
	{
		if (FoodSteps.Length > 0)
		{
			audios.PlayOneShot(FoodSteps[Random.Range(0, FoodSteps.Length)]);
		}
	}

	private void Update()
	{
		if ((bool)motor)
		{
			float magnitude = motor.movement.velocity.magnitude;
			if (motor.grounded && motor.IsActive && delay >= Delay)
			{
				PlaySound();
				delay = 0f;
			}
			if (delay < Delay)
			{
				delay += magnitude * Time.deltaTime;
			}
		}
	}
}
