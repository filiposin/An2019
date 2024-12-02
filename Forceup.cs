using UnityEngine;

public class Forceup : MonoBehaviour
{
	public Renderer Marker;

	public Color Active;

	public bool ActiveMesh;

	public MeshRenderer mesh;

	public Collider collis;

	private void Start()
	{
		collis = GetComponent<Collider>();
		mesh = GetComponent<MeshRenderer>();
		mesh.enabled = false;
		collis.enabled = false;
	}

	private void Update()
	{
		if (Marker == null && (bool)GameObject.Find("Cone"))
		{
			Marker = GameObject.Find("Cone").GetComponent<Renderer>();
		}
		if (!(Marker != null))
		{
			return;
		}
		if (Marker.material.color == Active)
		{
			if (ActiveMesh)
			{
				mesh.enabled = true;
			}
			collis.enabled = true;
		}
		else
		{
			mesh.enabled = false;
			collis.enabled = false;
		}
	}
}
