using UnityEngine;
using UnityEngine.UI;

public class SensMaster : MonoBehaviour
{
	public float sensx;

	public Slider slider;

	public Slider slider2;

	public FPSController player;

	public Text textshow;

	public Text textshow2;

	public bool mode;

	public Text textfps;

	private void Start()
	{
		if (PlayerPrefs.HasKey("sensdata"))
		{
			sensx = PlayerPrefs.GetFloat("sensdata");
			slider.value = sensx;
			slider2.value = sensx;
		}
		else
		{
			sensx = 3f;
			Debug.Log("нету ключа");
		}
		if (PlayerPrefs.HasKey("showfps"))
		{
			mode = true;
		}
		else
		{
			mode = false;
		}
	}

	private void Update()
	{
		if (mode)
		{
			textfps.enabled = true;
			if (textshow != null)
			{
				textshow.text = "ON";
			}
			textshow2.text = "ON";
		}
		else
		{
			textfps.enabled = false;
			if (textshow != null)
			{
				textshow.text = "OFF";
			}
			textshow2.text = "OFF";
		}
		if (player == null)
		{
			player = Object.FindObjectOfType<FPSController>();
		}
		else if (sensx == 1f)
		{
			player.sensitivityX = 0.15f;
			player.sensitivityY = 0.15f;
		}
		else if (sensx == 2f)
		{
			player.sensitivityX = 0.25f;
			player.sensitivityY = 0.25f;
		}
		else if (sensx == 3f)
		{
			player.sensitivityX = 0.4f;
			player.sensitivityY = 0.4f;
		}
		else if (sensx == 4f)
		{
			player.sensitivityX = 0.5f;
			player.sensitivityY = 0.5f;
		}
		else if (sensx == 5f)
		{
			player.sensitivityX = 0.6f;
			player.sensitivityY = 0.6f;
		}
	}

	public void Sensitivity(float levelsens)
	{
		slider2.value = slider.value;
		sensx = levelsens;
		PlayerPrefs.SetFloat("sensdata", sensx);
	}

	public void Sensitivity2(float levelsens)
	{
		sensx = levelsens;
		PlayerPrefs.SetFloat("sensdata", sensx);
	}

	public void DeletePrefs()
	{
		PlayerPrefs.DeleteAll();
	}

	public void Showfps()
	{
		if (!mode)
		{
			PlayerPrefs.SetFloat("showfps", 1f);
		}
		else
		{
			PlayerPrefs.DeleteKey("showfps");
		}
		mode = !mode;
	}
}
