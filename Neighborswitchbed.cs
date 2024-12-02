using UnityEngine;

public class Neighborswitchbed : MonoBehaviour
{
	public Renderer Marker;

	public Color Active;

	public bool signal;

	public string zonaname;

	public string activeskin;

	public string activeskin2;

	public string activeskin3;

	public string activeskin4;

	public string activeskin5;

	public string activeskin6;

	public GameObject modelactive;

	public SkinnedMeshRenderer modelactive2;

	public SkinnedMeshRenderer modelactive3;

	public SkinnedMeshRenderer modelactive4;

	public SkinnedMeshRenderer modelactive5;

	public SkinnedMeshRenderer modelactive6;

	public SkinnedMeshRenderer modelactive7;

	public SkinnedMeshRenderer skin1;

	public SkinnedMeshRenderer skin2;

	public SkinnedMeshRenderer skin3;

	public SkinnedMeshRenderer skin4;

	public SkinnedMeshRenderer skin5;

	public SkinnedMeshRenderer skin6;

	public SkinnedMeshRenderer skin7;

	public SkinnedMeshRenderer skin8;

	public SkinnedMeshRenderer skin9;

	public SkinnedMeshRenderer skin10;

	public string soundname;

	public AudioSource soundmaster;

	private void Start()
	{
		modelactive = GameObject.Find(activeskin);
		modelactive2 = modelactive.gameObject.GetComponent<SkinnedMeshRenderer>();
		modelactive = GameObject.Find(activeskin2);
		modelactive3 = modelactive.gameObject.GetComponent<SkinnedMeshRenderer>();
		modelactive = GameObject.Find(activeskin3);
		modelactive4 = modelactive.gameObject.GetComponent<SkinnedMeshRenderer>();
		modelactive = GameObject.Find(activeskin4);
		modelactive5 = modelactive.gameObject.GetComponent<SkinnedMeshRenderer>();
		modelactive = GameObject.Find(activeskin5);
		modelactive6 = modelactive.gameObject.GetComponent<SkinnedMeshRenderer>();
		modelactive = GameObject.Find(activeskin6);
		modelactive7 = modelactive.gameObject.GetComponent<SkinnedMeshRenderer>();
		if (soundname != null)
		{
			soundmaster = GameObject.Find(soundname).GetComponent<AudioSource>();
		}
	}

	private void Update()
	{
	}

	private void OnTriggerStay(Collider other)
	{
		if (!(Marker.material.color == Active) && other.name == zonaname)
		{
			skin1.enabled = false;
			skin2.enabled = false;
			skin3.enabled = false;
			skin4.enabled = false;
			skin5.enabled = false;
			skin6.enabled = false;
			skin7.enabled = false;
			skin8.enabled = false;
			skin9.enabled = false;
			skin10.enabled = false;
			modelactive2.enabled = true;
			modelactive3.enabled = true;
			modelactive4.enabled = true;
			modelactive5.enabled = true;
			modelactive6.enabled = true;
			modelactive7.enabled = true;
			soundmaster.mute = false;
			signal = true;
		}
	}

	private void FixedUpdate()
	{
		if (!signal)
		{
			skin1.enabled = true;
			skin2.enabled = true;
			skin3.enabled = true;
			skin4.enabled = true;
			skin5.enabled = true;
			skin6.enabled = true;
			skin7.enabled = true;
			skin8.enabled = true;
			skin9.enabled = true;
			skin10.enabled = true;
			modelactive2.enabled = false;
			modelactive3.enabled = false;
			modelactive4.enabled = false;
			modelactive5.enabled = false;
			modelactive6.enabled = false;
			modelactive7.enabled = false;
			soundmaster.mute = true;
		}
		signal = false;
	}
}
