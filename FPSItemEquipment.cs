using UnityEngine;

public class FPSItemEquipment : MonoBehaviour
{
	public string ItemID = string.Empty;

	public ItemCollector CollectorSlot;

	[HideInInspector]
	public bool OnFire1;

	[HideInInspector]
	public bool OnFire2;

	public virtual void Trigger()
	{
		OnFire1 = true;
	}

	public virtual void Trigger2()
	{
		OnFire2 = true;
	}

	public virtual void OnTriggerRelease()
	{
		OnFire1 = false;
	}

	public virtual void OnTrigger2Release()
	{
		OnFire2 = false;
	}

	public virtual void Reload()
	{
	}

	public virtual void ReloadComplete()
	{
	}

	public virtual void OnAction()
	{
	}

	public void Hide(bool visible)
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			renderer.enabled = visible;
		}
	}

	public void SetItemID(string id)
	{
		ItemID = id;
	}
}
