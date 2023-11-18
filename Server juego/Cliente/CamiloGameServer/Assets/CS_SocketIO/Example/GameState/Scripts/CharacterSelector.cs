using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    public Sprite[] characters;
    public Image image;
    public NetworkController connection;
    private int index;
    private Dictionary<string, int> playerIndices = new Dictionary<string, int>();

    public int playersInGame;
    public DataStructureUI[] gamePanels;

    public static CharacterSelector Instance;

    private void Start()
    {
        Instance = this;
        index = 1;
        connection.CharacterNum = index;
        playersInGame = 0;
        ChangeCharacter(0);
    }

    public void ChangeCharacter(int i)
    {
        index = Mathf.Clamp(index + i, 1, 4);
        image.sprite = characters[index - 1];
        connection.CharacterNum = index;
    }

    public void SetUpNewPlayer(Player pl)
    {
        if (playerIndices.ContainsKey(pl.Id)) return;

        playerIndices.Add(pl.Id, playersInGame);
        gamePanels[playersInGame].parent.SetActive(true);
        playersInGame++;
    }

    public void UpdateScore(Player pl)
    {
        gamePanels[playerIndices[pl.Id]].image.sprite = characters[pl.CharacterNum - 1];
        gamePanels[playerIndices[pl.Id]].username.text = pl.Username;
        gamePanels[playerIndices[pl.Id]].scale.text = pl.Radius.ToString();
    }

    public void UpdateHighestScores(List<Player> players)
    {
        var sortedPlayers = players.OrderByDescending(pl => pl.Radius.ToString()).ToList();

        for (int i = 0; i < Mathf.Min(playersInGame, 5); i++)
        {
            gamePanels[i].image.sprite = characters[sortedPlayers[i].CharacterNum - 1];
            gamePanels[i].username.text = sortedPlayers[i].Username;
            gamePanels[i].scale.text = sortedPlayers[i].Radius.ToString();
        }
    }
}

[Serializable]
public class DataStructureUI
{
    public GameObject parent;
    public Image image;
    public TextMeshProUGUI username;
    public TextMeshProUGUI scale;
}
