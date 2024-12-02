using UnityEngine;

public class ColorUpd : MonoBehaviour
{
	public Renderer rend;

	public Color vet;

	public AudioClip AudioS;

	public bool block;

	private void Start()
	{
		rend = GetComponent<Renderer>();
	}

	private void Update()
	{
		if (!block)
		{
			Debug.Log("false");
			if (rend.material.color == vet)
			{
				Debug.Log("SoundRun");
				AudioSource.PlayClipAtPoint(AudioS, base.transform.position);
				block = true;
			}
		}
	}
}
