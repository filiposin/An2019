using UnityEngine;

public class GameClient : MonoBehaviour
{
    public float ConnectionTimeOut = 10f;

    [HideInInspector]
    public bool isConnecting;

    [HideInInspector]
    public float Delay;

    private float timeConnecting;

    private void Start()
    {
    }

    private void Update()
    {
        if (isConnecting && Time.time > timeConnecting + ConnectionTimeOut)
        {
            isConnecting = false;
        }
    }

    public void AttemptConnectToServer()
    {
        if (UnitZ.playersManager != null)
        {
            UnitZ.playersManager.ClearPlayers();
        }

        isConnecting = true;
        timeConnecting = Time.time;
        Debug.Log("Attempting to connect to the game...");
    }

    public void Disconnect()
    {
        isConnecting = false;
    }

    private void OnConnectedToServer()
    {
        Debug.Log("Connected to server! (Local functionality remains)");
        if (UnitZ.playersManager != null)
        {
            UnitZ.playersManager.UpdatePlayerInfo("0", 0, 0, UnitZ.gameManager.UserName, UnitZ.gameManager.Team, UnitZ.GameKeyVersion, true);
        }
        isConnecting = false;
    }

    public void OnDisconnectedFromServer()
    {
        UnitZ.gameManager.ClearNetworkGameObject();
        UnitZ.gameManager.IsPlaying = false;

        if (Application.loadedLevelName != PlayerPrefs.GetString("landingpage"))
        {
            Application.LoadLevel(PlayerPrefs.GetString("landingpage"));
            Object.Destroy(base.gameObject);
        }

        isConnecting = false;
        Debug.Log("Disconnected from server!");
    }
}
