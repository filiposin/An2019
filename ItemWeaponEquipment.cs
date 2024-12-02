using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ItemWeaponEquipment : ItemEquipment
{
	private CharacterSystem character;

	public GameObject MuzzleFX;

	private AudioSource audioSource;

	public AudioClip SoundFire;

	public AudioClip[] DamageSound;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		if ((bool)base.transform.root)
		{
			character = base.transform.root.GetComponent<CharacterSystem>();
		}
		else
		{
			character = base.transform.GetComponent<CharacterSystem>();
		}
		if ((bool)character)
		{
			character.DamageSound = DamageSound;
		}
	}

	public override void Action()
	{
		if ((bool)audioSource && (bool)SoundFire && audioSource.enabled)
		{
			audioSource.PlayOneShot(SoundFire);
		}
		if ((bool)MuzzleFX)
		{
			GameObject gameObject = Object.Instantiate(MuzzleFX, base.transform.position, base.transform.rotation);
			gameObject.transform.parent = base.transform;
			Object.Destroy(gameObject, 2f);
		}
		base.Action();
	}
}
