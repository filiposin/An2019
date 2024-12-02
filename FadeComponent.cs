using UnityEngine;

public class FadeComponent : MonoBehaviour
{
	public Texture2D BG;

	public Texture2D LoadingBG;

	public float Alpha;

	private float alphaTarget;

	public float Delay = 0.5f;

	public GameObject loader;

	private void Start()
	{
		alphaTarget = Alpha;
	}

	private void OnGUI()
	{
		if (!(BG == null))
		{
			GUI.depth = 1;
			if (Alpha > 0.01f)
			{
				GUI.color = new Color(1f, 1f, 1f, Alpha);
				GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), BG);
			}
			if (Application.isLoadingLevel)
			{
				loader.SetActive(true);
			}
			GUI.depth = 0;
		}
	}

	public void Fade(float start, float end, float delay)
	{
		Alpha = start;
		Delay = delay;
		alphaTarget = end;
	}

	public void Fade(float start, float end)
	{
		Alpha = start;
		Delay = 1f;
		alphaTarget = end;
	}

	private void Update()
	{
		Alpha = Mathf.Lerp(Alpha, alphaTarget, Delay * Time.deltaTime);
	}
}
