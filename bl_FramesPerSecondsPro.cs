using UnityEngine;
using UnityEngine.UI;

public class bl_FramesPerSecondsPro : MonoBehaviour
{
	[SerializeField]
	private Text FPSText;

	private float deltaTime;

	private void Update()
	{
		if (!(FPSText == null))
		{
			deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
			float num = deltaTime * 1000f;
			float num2 = 1f / deltaTime;
			string text = string.Format("{1:0.} FPS", num, num2);
			FPSText.text = text.ToUpper();
		}
	}
}
