using UnityEngine;

public class ItemToggleEquipment : ItemEquipment
{
	private CharacterSystem character;

	public GameObject ItemToggle;

	public bool IsActive;

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
		if ((bool)ItemToggle)
		{
			ItemToggle.gameObject.SetActive(IsActive);
		}
		base.Action();
	}
}
