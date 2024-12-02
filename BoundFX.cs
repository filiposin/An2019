using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BoundFX : MonoBehaviour
{
	private AudioSource Audiosource;

	public AudioClip[] Sounds;

	private void Start()
	{
		Audiosource = GetComponent<AudioSource>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (Sounds.Length > 0)
		{
			Audiosource.PlayOneShot(Sounds[Random.Range(0, Sounds.Length)]);
		}
	}
}
