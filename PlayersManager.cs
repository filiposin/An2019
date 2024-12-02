using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    public float VersionCheckDelay = 5f;

    public List<PlayerData> PlayerList = new List<PlayerData>();

    public List<string> TeamList = new List<string>();

    private void Start()
    {
        ClearPlayers();
    }

    public void ClearPlayers()
    {
        PlayerList.Clear();
        TeamList.Clear();
    }

    public PlayerData GetPlayerData(string ID)
    {
        foreach (PlayerData player in PlayerList)
        {
            if (player.ID == ID)
            {
                return player;
            }
        }
        return null;
    }

    private void Update()
    {
        // Ћогика обновлени€ игроков, если необходимо
        // ¬ одиночном режиме можно оставить это место дл€ обновлени€ состо€ни€ игроков
    }

    public void AddPlayer(string id)
    {
        PlayerData playerData = new PlayerData();
        playerData.Dead = 0;
        playerData.ID = id;
        playerData.Name = string.Empty;
        playerData.Team = string.Empty;
        playerData.IsConnected = true;
        PlayerList.Add(playerData);
    }

    private void AddTeam(string team)
    {
        foreach (string team2 in TeamList)
        {
            if (team == team2)
            {
                return;
            }
        }
        TeamList.Add(team);
    }

    public void UpdateMyInfo(string id, string name, string team, bool isconnected)
    {
        foreach (PlayerData player in PlayerList)
        {
            if (id == player.ID)
            {
                player.Name = name;
                player.Team = team;
                player.IsConnected = isconnected;
                AddTeam(team);
                break;
            }
        }
    }

    public void UpdatePlayerInfo(string id, int score, int dead, string name, string team, string gameKey, bool isconnected)
    {
        bool flag = false;
        foreach (PlayerData player in PlayerList)
        {
            if (player.ID == id)
            {
                flag = true;
            }
        }
        if (!flag)
        {
            AddPlayer(id);
        }
        foreach (PlayerData player2 in PlayerList)
        {
            if (id == player2.ID)
            {
                player2.Score = score;
                player2.Dead = dead;
                player2.Name = name;
                player2.Team = team;
                player2.GameKey = gameKey;
                player2.ConnectedTime = Time.time;
                player2.IsConnected = isconnected;
                AddTeam(team);
                break;
            }
        }
    }
}