using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class GameServer : MonoBehaviour
{
    public string ServerName = "UZ_coop";

    public int Port = 25000;

    public int MaxPlayer = 32;

    public string IPServer = "127.0.0.1";

    public string FacilitatorIP = string.Empty;

    public string MasterServerIP = string.Empty;

    public int MasterServerPort = 23466;

    public int FacilitatorPort = 50005;

    public bool LanOnly;

    public bool UseNat;

    [HideInInspector]
    public bool isActive;

    private void Start()
    {
        // Удалены все элементы, связанные с NetworkView, MasterServer и NAT Facilitator
    }

    public void StartServer()
    {
        isActive = false;
        Debug.Log("Server started locally on port: " + Port);
    }

    public void KillServer()
    {
        isActive = false;
        Debug.Log("Kill Server");
    }

    private void OnPlayerConnected()
    {
        Debug.Log("Player connected (Single-player mode, no network interaction).");
    }

    private void OnPlayerDisconnected()
    {
        Debug.Log("Player disconnected (Single-player mode, no network interaction).");
    }

    private void OnServerInitialized()
    {
        Debug.Log("Server initialized (Single-player mode).");
    }

    public string GetLocalIPAddress()
    {
        string result = string.Empty;
        IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress[] addressList = hostEntry.AddressList;
        foreach (IPAddress iPAddress in addressList)
        {
            if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                result = iPAddress.ToString();
            }
        }
        return result;
    }
}
