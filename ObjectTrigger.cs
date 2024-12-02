using UnityEngine;

public class ObjectTrigger : MonoBehaviour
{
	public string ActiveText = string.Empty;

	protected bool ShowInfo;

	public float DistanceLimit = 2f;

	public Vector3 Offset;

	protected CharacterSystem characterTemp;

	private void Start()
	{
	}

	private void Update()
	{
		UpdateFunction();
	}

	protected void UpdateFunction()
	{
		if ((bool)characterTemp)
		{
			if (Vector3.Distance(base.transform.position, characterTemp.transform.position + Offset) > DistanceLimit)
			{
				OnExit();
			}
			else
			{
				OnStay();
			}
		}
	}

	public virtual void OnStay()
	{
	}

	public virtual void OnExit()
	{
		characterTemp = null;
		ShowInfo = false;
	}

	public virtual void GetInfo()
	{
		ShowInfo = true;
	}

	public virtual void Pickup(CharacterSystem character)
	{
		characterTemp = character;
	}

	public void FixedUpdate()
	{
		ShowInfo = false;
	}

	private void OnGUI()
	{
		if (ShowInfo)
		{
			Vector3 vector = Camera.main.WorldToScreenPoint(base.gameObject.transform.position + Offset);
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.Label(new Rect(vector.x, (float)Screen.height - vector.y, 200f, 60f), ActiveText);
		}
	}
}
