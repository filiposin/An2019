using UnityEngine;

public class GameMenuCanvas : MonoBehaviour
{
	private void Start()
	{
	}

	public void Resume()
	{
		MouseLock.MouseLocked = true;
	}

	public void Disconnect()
	{
		if ((bool)UnitZ.gameManager)
		{
			UnitZ.gameManager.QuitGame();
		}
		MouseLock.MouseLocked = false;
	}
}
