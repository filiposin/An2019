using UnityEngine;

public class DoorOpen : MonoBehaviour
{
	public bool Key1;

	public bool Key2;

	public bool Key3;

	public bool Key4;

	public bool Key5;

	private void OnCollisionEnter(Collision col)
	{
		if (Key1 && col.gameObject.name == "DoorKey1")
		{
			col.transform.GetComponent<DoorOpener>().KeyEnter();
			Debug.Log("dd");
		}
		if (Key2 && col.gameObject.name == "DoorKey2")
		{
			col.transform.GetComponent<DoorOpener>().KeyEnter();
		}
		if (Key3 && col.gameObject.name == "DoorKey3")
		{
			col.transform.GetComponent<DoorOpener>().KeyEnter();
		}
		if (Key4 && col.gameObject.name == "DoorKey4")
		{
			col.transform.GetComponent<DoorOpener>().KeyEnter();
		}
		if (Key5 && col.gameObject.name == "DoorKey5")
		{
			col.transform.GetComponent<DoorOpener>().KeyEnter();
		}
		Object.Destroy(base.gameObject);
	}
}
