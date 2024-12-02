using UnityEngine;

public class krat : MonoBehaviour
{
	public int num;

	public int maxnum;

	public Collider zone;

	private void Start()
	{
		zone.enabled = false;
	}

	private void Update()
	{
		if (num > maxnum)
		{
			zone.enabled = true;
		}
	}

	public virtual void Pickup(CharacterSystem character)
	{
		num++;
	}
}
