using UnityEngine;

public class SleepUp : MonoBehaviour
{
	private GameObject player;

	private Collider Picture;

	private void Start()
	{
		player = GameObject.Find("Character_Salary(Clone)");
		Object.Destroy(player);
		Picture = GameObject.Find("PictureClose").GetComponent<Collider>();
		Picture.enabled = true;
		Object.Destroy(base.gameObject);
	}
}
