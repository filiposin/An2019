using UnityEngine;

public class OpenDraw : MonoBehaviour
{
	public Animator anim;

	public int isOpen;

	public string Open;

	public string Close;

	public Collider Drawer;

	private bool Drawernone;

	private Collider col;

	public AudioClip SoundPickup;

	private float sec;

	public float maxsec;

	public void Start()
	{
		col = GetComponent<Collider>();
		anim = GetComponent<Animator>();
		if (Drawer == null)
		{
			Drawernone = true;
		}
	}

	private void Update()
	{
		sec += Time.deltaTime;
	}

	public virtual void Pickup(CharacterSystem character)
	{
		if (sec > maxsec)
		{
			Pick();
		}
	}

	public void Pick()
	{
		if (isOpen == 0)
		{
			anim.Play(Open);
			isOpen = 1;
			sec = 0f;
			if ((bool)SoundPickup)
			{
				AudioSource.PlayClipAtPoint(SoundPickup, base.transform.position);
			}
			if (!Drawernone)
			{
				Drawer.enabled = false;
			}
		}
		else
		{
			anim.Play(Close);
			isOpen = 0;
			sec = 0f;
			if ((bool)SoundPickup)
			{
				AudioSource.PlayClipAtPoint(SoundPickup, base.transform.position);
			}
			if (!Drawernone)
			{
				Drawer.enabled = true;
			}
		}
	}
}
