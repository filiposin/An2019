using System.Collections.Generic;
using UnityEngine;

public class FPSRayActive : MonoBehaviour
{
	public bool Sorting;

	public string[] IgnoreTag = new string[1] { "Player" };

	public string[] DestroyerTag = new string[1] { "scene" };

	private void Start()
	{
	}

	public void ShootRayOnce(Vector3 origin, Vector3 direction, string id, string team)
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(origin, direction, out hitInfo, 100f) && hitInfo.collider.gameObject != base.gameObject)
		{
			DamagePackage damagePackage = default(DamagePackage);
			damagePackage.Damage = 50;
			damagePackage.Normal = hitInfo.normal;
			damagePackage.Direction = direction;
			damagePackage.Position = hitInfo.point;
			damagePackage.ID = id;
			damagePackage.Team = team;
			hitInfo.collider.SendMessage("OnHit", damagePackage, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void CheckingRay(Vector3 origin, Vector3 direction)
	{
		float maxDistance = 3f;
		RaycastHit[] array = Physics.RaycastAll(origin, direction, maxDistance);
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i].collider)
			{
				RaycastHit raycastHit = array[i];
				raycastHit.collider.SendMessage("GetInfo", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public void ActiveRay(Vector3 origin, Vector3 direction)
	{
		float maxDistance = 3f;
		RaycastHit[] array = Physics.RaycastAll(origin, direction, maxDistance);
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i].collider)
			{
				array[i].collider.SendMessage("Pickup", GetComponent<CharacterSystem>(), SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public bool ShootRay(Vector3 origin, Vector3[] direction, int damage, float size, int hitmax, string id, string team)
	{
		bool result = false;
		DamagePackage damagePackage = default(DamagePackage);
		for (int i = 0; i < direction.Length; i++)
		{
			int num = damage;
			int num2 = 0;
			int num3 = 0;
			List<casthit> list = new List<casthit>();
			RaycastHit[] array = Physics.RaycastAll(origin, direction[i], size);
			for (int j = 0; j < array.Length; j++)
			{
				if ((bool)array[j].collider && tagCheck(array[j].collider.gameObject) && array[j].collider.gameObject != base.gameObject && (((bool)array[j].collider.transform.root && array[j].collider.transform.root != base.gameObject.transform.root && array[j].collider.transform.root.gameObject != base.gameObject) || array[j].collider.transform.root == null))
				{
					num3++;
					casthit item = default(casthit);
					item.distance = Vector3.Distance(origin, array[j].point);
					item.index = j;
					item.name = array[j].collider.name;
					list.Add(item);
				}
			}
			RaycastHit[] array2 = new RaycastHit[num3];
			list.Sort((casthit x, casthit y) => x.distance.CompareTo(y.distance));
			for (int k = 0; k < list.Count; k++)
			{
				array2[k] = array[list[k].index];
			}
			for (int l = 0; l < array2.Length; l++)
			{
				if (num2 >= hitmax)
				{
					break;
				}
				RaycastHit raycastHit = array2[l];
				damagePackage.Damage = damage;
				damagePackage.Normal = raycastHit.normal;
				damagePackage.Direction = direction[i];
				damagePackage.Position = raycastHit.point;
				damagePackage.ID = id;
				damagePackage.Team = team;
				raycastHit.collider.SendMessage("OnHit", damagePackage, SendMessageOptions.DontRequireReceiver);
				result = true;
				num2++;
				if (num2 >= hitmax || tagDestroyerCheck(raycastHit.collider.gameObject))
				{
					break;
				}
				int num4 = (int)((float)num * 0.75f);
				num = num4;
			}
		}
		return result;
	}

	public bool ShootRayTest(Vector3 origin, Vector3[] direction, int damage, float size, int hitmax, string id, string team)
	{
		bool result = false;
		for (int i = 0; i < direction.Length; i++)
		{
			int num = 0;
			int num2 = 0;
			List<casthit> list = new List<casthit>();
			RaycastHit[] array = Physics.RaycastAll(origin, direction[i], size);
			for (int j = 0; j < array.Length; j++)
			{
				if ((bool)array[j].collider && tagCheck(array[j].collider.gameObject) && array[j].collider.gameObject != base.gameObject && (((bool)array[j].collider.transform.root && array[j].collider.transform.root.gameObject != base.gameObject) || array[j].collider.transform.root == null))
				{
					num2++;
					casthit item = default(casthit);
					item.distance = Vector3.Distance(origin, array[j].point);
					item.index = j;
					item.name = array[j].collider.name;
					list.Add(item);
				}
			}
			RaycastHit[] array2 = new RaycastHit[num2];
			list.Sort((casthit x, casthit y) => x.distance.CompareTo(y.distance));
			for (int k = 0; k < list.Count; k++)
			{
				array2[k] = array[list[k].index];
			}
			for (int l = 0; l < array2.Length; l++)
			{
				if (num >= hitmax)
				{
					break;
				}
				RaycastHit raycastHit = array2[l];
				result = true;
				num++;
				if (num >= hitmax || tagDestroyerCheck(raycastHit.collider.gameObject))
				{
					break;
				}
			}
		}
		return result;
	}

	public bool Overlap(Vector3 origin, Vector3 forward, int damage, float size, float dot, string id, string team)
	{
		bool result = false;
		Collider[] array = Physics.OverlapSphere(origin, size);
		Collider[] array2 = array;
		DamagePackage damagePackage = default(DamagePackage);
		foreach (Collider collider in array2)
		{
			if ((bool)collider && collider.gameObject != base.gameObject && collider.gameObject.transform.root != base.gameObject.transform)
			{
				Debug.Log(collider.gameObject.transform.root.name);
				Vector3 normalized = (collider.transform.position - origin).normalized;
				float num = Vector3.Dot(normalized, forward);
				if (num >= dot)
				{
					damagePackage.Damage = damage;
					damagePackage.Normal = normalized;
					damagePackage.Direction = forward;
					damagePackage.Position = collider.gameObject.transform.position;
					damagePackage.ID = id;
					damagePackage.Team = team;
					collider.GetComponent<Collider>().SendMessage("OnHit", damagePackage, SendMessageOptions.DontRequireReceiver);
					result = true;
				}
			}
		}
		return result;
	}

	public bool OverlapTest(Vector3 origin, Vector3 forward, int damage, float size, float dot, string id, string team)
	{
		bool result = false;
		Collider[] array = Physics.OverlapSphere(origin, size);
		Collider[] array2 = array;
		foreach (Collider collider in array2)
		{
			if ((bool)collider && collider.gameObject != base.gameObject && collider.gameObject.transform.root != base.gameObject)
			{
				Vector3 normalized = (collider.transform.position - origin).normalized;
				float num = Vector3.Dot(normalized, forward);
				if (num >= dot)
				{
					result = true;
				}
			}
		}
		return result;
	}

	private bool tagDestroyerCheck(GameObject obj)
	{
		for (int i = 0; i < DestroyerTag.Length; i++)
		{
			if (obj.CompareTag(DestroyerTag[i]))
			{
				return true;
			}
		}
		return false;
	}

	private bool tagCheck(GameObject obj)
	{
		for (int i = 0; i < IgnoreTag.Length; i++)
		{
			if (obj.CompareTag(IgnoreTag[i]))
			{
				return false;
			}
		}
		return true;
	}
}
