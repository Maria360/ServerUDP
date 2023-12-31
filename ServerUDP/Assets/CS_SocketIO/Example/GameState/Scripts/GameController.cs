using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine.UIElements;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject GameContainer;
    [SerializeField]
    private Transform PlayersContainer;
    [SerializeField]
    private Transform CoinsContainer;
    [SerializeField]
    private TextMeshProUGUI textAxis;

    [SerializeField]
    private GameObject PlayerPrefab;
    [SerializeField]
    private GameObject CoinPrefab;

    [SerializeField] Sprite[] playerSprites;

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

        RectTransform playerRectTransform = playerGameObject.GetComponent<RectTransform>();

        playerGameObject.transform.position = new Vector2(player.x, 200);
        playerGameObject.GetComponent<GamePlayer>().Id = player.Id;
        playerGameObject.GetComponent<GamePlayer>().Username = player.Id;
        playerGameObject.GetComponent<GamePlayer>().usertext.text = player.Username;

        //playerGameObject.GetComponent<GamePlayer>().Score = player.Score;
        //playerGameObject.GetComponent<GamePlayer>().scoreText.text = player.Score.ToString();



        int randomIndex = Random.Range(0, playerSprites.Length);
        playerGameObject.GetComponent<SpriteRenderer>().sprite = playerSprites[randomIndex];

        



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
                    PlayersToRender[player.Id].position = new Vector2(player.x, player.y);
                    float scale = player.Radius / 1f;
                    PlayersToRender[player.Id].localScale = new Vector2(scale, scale);//here

                    PlayersToRender[player.Id].GetComponent<GamePlayer>().scoreText.text = player.Score.ToString();
                    


                }
                else
                {
                    InstantiatePlayer(player);
                    
                    
                }
              
            }
            var plarersToDelete = PlayersToRender.Where(item => !State.Players.Any(player => player.Id == item.Key)).ToList();
            foreach (var playerItem in plarersToDelete)
            {
                Destroy(playerItem.Value.gameObject);
                PlayersToRender.Remove(playerItem.Key);
            }
            foreach (Coin coin in State.Coins)
            {
                if (CoinsToRender.ContainsKey(coin.Id))
                {
                    CoinsToRender[coin.Id].position = new Vector2(coin.x, coin.y);
                    
                    CoinsToRender[coin.Id].rotation = Quaternion.Euler(0,0,coin.angle);
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
        if (Input.GetKeyDown("w"))
        {
            textAxis.text = "W";
        }
        if (Input.GetKeyDown("a"))
        {
            textAxis.text = "A";
        }
        if (Input.GetKeyDown("s"))
        {
            textAxis.text = "S";
        }
        if (Input.GetKeyDown("d"))
        {
            textAxis.text = "D";
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
