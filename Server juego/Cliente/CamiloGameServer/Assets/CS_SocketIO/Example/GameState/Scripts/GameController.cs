using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject GameContainer;
    [SerializeField]
    private Transform PlayersContainer;
    [SerializeField]
    private Transform CoinsContainer;

    [SerializeField]
    private GameObject PlayerPrefab;
    [SerializeField]
    private GameObject CoinPrefab;

    private GameState State;
    private Dictionary<string, Transform> PlayersToRender;
    private Dictionary<string, Transform> CoinsToRender;
    internal void StartGame(GameState state)
    {
        PlayersToRender = new Dictionary<string, Transform>();
        CoinsToRender = new Dictionary<string, Transform>();

        GameObject.Find("PanelConnect").SetActive(false);
        GameContainer.SetActive(true);


        foreach (Player player in state.Players)
        {
            InstantiatePlayer(player);
        }

        var Socket = NetworkController._Instance.Socket;

        InputController._Instance.onAxisChange += (axis) => { Socket.Emit("move", axis); };

        State = state;
        Socket.On("updateState", UpdateState);
    }

    private void InstantiatePlayer(Player player)
    {
        GameObject playerGameObject = Instantiate(PlayerPrefab, PlayersContainer);
        if (player.Username == NetworkController._Instance.Username) InputController._Instance.set_Player(playerGameObject);
        playerGameObject.transform.position = new Vector2(player.x, player.y);
        playerGameObject.GetComponent<GamePlayer>().Id = player.Id;
        playerGameObject.GetComponent<GamePlayer>().Username = player.Username;

        playerGameObject.GetComponent<GamePlayer>().SetTxt();
        playerGameObject.GetComponent<GamePlayer>().SetUp(player.CharacterNum);
        Debug.Log(player.CharacterNum);

        CharacterSelector.Instance.SetUpNewPlayer(player);

        PlayersToRender[player.Id] = playerGameObject.transform;
    }

    private void UpdateState(string json)
    {
        GameStateData jsonData = JsonUtility.FromJson<GameStateData>(json);
        State = jsonData.State;

    }

    internal void NewPlayer(string id, string username)
    {
        InstantiatePlayer(new Player { Id = id, Username = username });
    }

    void Update()
    {
        if (State != null)
        {
            foreach (Player player in State.Players)
            {
                if (PlayersToRender.ContainsKey(player.Id))
                {
                    if (player.isDead)
                    {
                        PlayersToRender[player.Id].gameObject.SetActive(false);
                    }
                    else
                    {
                        PlayersToRender[player.Id].gameObject.SetActive(true);
                        PlayersToRender[player.Id].position = new Vector2(player.x, player.y);
                        PlayersToRender[player.Id].localScale = new Vector2(player.Radius, player.Radius);
                        CharacterSelector.Instance.UpdateScore(player);
                    }
                }
                else
                {
                    InstantiatePlayer(player);
                }
            }

            CharacterSelector.Instance.UpdateHighestScores(State.Players.ToList());

            var playersToDelete = PlayersToRender.Where(item => !State.Players.Any(player => player.Id == item.Key)).ToList();
            foreach (var playerItem in playersToDelete)
            {
                Destroy(playerItem.Value.gameObject);
                PlayersToRender.Remove(playerItem.Key);
            }

            foreach (Coin coin in State.Coins)
            {
                if (CoinsToRender.ContainsKey(coin.Id))
                {
                    CoinsToRender[coin.Id].position = new Vector2(coin.x, coin.y);
                }
                else
                {
                    InstantiateCoin(coin);
                }
            }

            var coinsToDelete = CoinsToRender.Where(item => !State.Coins.Any(coin => coin.Id == item.Key)).ToList();

            foreach (var coinItem in coinsToDelete)
            {
                Destroy(coinItem.Value.gameObject);
                CoinsToRender.Remove(coinItem.Key);
            }
        }
    }

    private void InstantiateCoin(Coin coin)
    {
        GameObject coinGameObject = Instantiate(CoinPrefab, CoinsContainer);
        coinGameObject.transform.position = new Vector2(coin.x, coin.y);
        coinGameObject.GetComponent<GameCoin>().Id = coin.Id;

        CoinsToRender[coin.Id] = coinGameObject.transform;
    }
}

[Serializable]
public class GameStateData
{
    public GameState State;
}
