using StaticScripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VCG_Library;
using VCG_Objects;
using WebSocketSharp;

namespace EventScripts
{
    public class GameSystem : MonoBehaviour
    {
        public GameObject unloadingScreen;

        public GameObject RoomItem;
        public GameObject NoRoomItem;
        public GameObject RoomList;

        public GameObject PlayerItem;
        public GameObject PlayerList;

        public GameObject RoomListPanel;
        public GameObject LobbyPanel;
        public GameObject GamePanel;

        public Animator RoomListTitleAnimator;
        public Animator LobbyTitleAnimator;
        public Animator GameTitleAnimator;

        public Animator PanelController;

        public bool canListRooms
        {
            get
            {
                return CanListRooms;
            }
            set
            {
                CanListRooms = value;
            }
        }

        public bool CanListRooms = false;

        public dynamic[] listRoomsArgs;

        public bool canListPlayers = false;
        public dynamic[] listPlayersArgs;

        public bool JoinRoomAction = false;
        public bool StartRoomAction = false;

        public GameObject cardPrefab;
        public Image deckPanel;
        [SerializeField]
        private List<Card> Deck = new List<Card>();
        public List<Card> deck { get => Deck; set { this.Deck = value; if (NetManager.GameStarted) { RedrawCards(); } } }

        public IEnumerator UnloadScene()
        {
            Scene scene = SceneManager.GetSceneByName("MainMenu");
            while (!scene.isLoaded)
            {
                yield return new WaitForEndOfFrame();
            }
            unloadingScreen.SetActive(false);
            SceneManager.UnloadSceneAsync("GameScene");
            yield return null;
        }

        public void StartJoinedRoom()
        {
            NetManager.SendDataToRoom("StartRoom<<");
        }

        public void UseCard(int cardIndex)
        {
            NetManager.SendDataToRoom("UseCard<<" + cardIndex);
        }
        
        public void RemoveCard(int cardIndex)
        {
            Destroy(deckPanel.transform.GetChild(cardIndex).gameObject);
        }

        public void QuitJoinedRoom()
        {
            NetManager.SendDataToRoom("Quit<<");
            OpenRooms();
        }

        public void StartMultiplayerGame()
        {
            unloadingScreen.SetActive(true);
            SceneManager.LoadSceneAsync("MainMenu");
            StartCoroutine(UnloadScene());
        }

        public GameObject InstantiateUI(GameObject original, Transform parent)
        {
            GameObject obj = Instantiate(original, parent);
            obj.transform.localScale = Vector3.one;
            return obj;
        }

        public void JoinRoom(string roomKey)
        {
            NetManager.SendData("JoinRoom", roomKey);
        }

        public void KickPlayer(string playerName)
        {
            NetManager.SendDataToRoom("KickPlayer", playerName);
        }

        public void CreateRoom(string roomName)
        {
            NetManager.SendData("CreateRoom", new string[] { roomName, "true", "7" });
            RelistRooms();
        }

        public void RelistRooms()
        {
            NetManager.SendData("ListRooms", 20);
        }

        void OnDataSent(object sender, MessageEventArgs e)
        {

        }

        public void StartEnumerator(IEnumerator enumerator)
        {
            StartCoroutine(enumerator);
        }

        public void RedrawCards()
        {
            for (int i = 0; i < deckPanel.transform.childCount; i++)
            {
                Destroy(deckPanel.transform.GetChild(i).gameObject);
            }
            foreach (Card card in deck)
            {
                var obj = InstantiateUI(cardPrefab, deckPanel.transform);
                obj.GetComponent<CardUI>().card = card;
                obj.GetComponent<CardUI>().selectAction = (card) => { UseCard(deck.IndexOf(card)); };
            }
        }

        void Start()
        {
            RoomListPanel.SetActive(false);
            LobbyPanel.SetActive(false);
            GamePanel.SetActive(false);
            NetManager.Create(this, "ws://127.0.0.1:99", 88, "Ömer Faruk Nehir");
            NetManager.StartConnection(OnDataSent);
            RoomListTitleAnimator.SetBool("Exit", false);
            RoomListPanel.SetActive(true);
        }

        public void OpenRooms()
        {
            GameTitleAnimator.SetBool("Exit", true);
            LobbyTitleAnimator.SetBool("Exit", true);

            StartCoroutine(SetActiveTimeOut(GamePanel, false, 1));
            StartCoroutine(SetActiveTimeOut(LobbyPanel, false, 1));
            StartCoroutine(SetActiveTimeOut(RoomListPanel, true, 1));

            RelistRooms();

            RoomListTitleAnimator.SetBool("Exit", false);
        }

        public void OpenMenu()
        {
            SceneManager.LoadScene(0);
            SceneManager.UnloadSceneAsync(1);
        }

        IEnumerator SetActiveTimeOut(GameObject obj, bool tf, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            obj.SetActive(tf);
        }

        public void AddCard(Card card)
        {

        }

        void Update()
        {
            if (JoinRoomAction)
            {
                GameTitleAnimator.SetBool("Exit", true);
                RoomListTitleAnimator.SetBool("Exit", true);

                StartCoroutine(SetActiveTimeOut(GamePanel, false, 1));
                StartCoroutine(SetActiveTimeOut(RoomListPanel, false, 1));
                StartCoroutine(SetActiveTimeOut(LobbyPanel, true, 1));

                LobbyTitleAnimator.SetBool("Exit", false);

                JoinRoomAction = false;
                Debug.Log("Joined Room");
            }

            if (StartRoomAction)
            {
                RoomListTitleAnimator.SetBool("Exit", true);
                LobbyTitleAnimator.SetBool("Exit", true);

                StartCoroutine(SetActiveTimeOut(GamePanel, true, 1));
                StartCoroutine(SetActiveTimeOut(RoomListPanel, false, 1));
                StartCoroutine(SetActiveTimeOut(LobbyPanel, false, 1));

                GameTitleAnimator.SetBool("Exit", false);

                StartRoomAction = false;
                Debug.Log("Game Started");
            }

            if (canListRooms && !NetManager.JoinedRoom)
            {
                for (int c = 0; c < RoomList.transform.childCount; c++)
                {
                    Destroy(RoomList.transform.GetChild(c).gameObject);
                }

                int i = 0;
                foreach (string roomText in listRoomsArgs)
                {
                    string[] roomData = roomText.Split(":");
                    string roomKey = roomData[0];
                    string roomName = roomData[1];
                    string roomPlayerNum = roomData[2];
                    string roomMaxNum = roomData[3];
                    GameObject roomItem = InstantiateUI(RoomItem, RoomList.transform);

                    roomItem.GetComponent<Button>().onClick.AddListener(() => JoinRoom(roomKey));
                    TextMeshProUGUI[] tmpItems = roomItem.GetComponentsInChildren<TextMeshProUGUI>();
                    tmpItems[0].text = roomName;
                    tmpItems[1].text = roomPlayerNum + "/" + roomMaxNum;

                    i++;
                }

                if (i == 0)
                {
                    GameObject noRoomItem = InstantiateUI(NoRoomItem, RoomList.transform);
                }

                canListRooms = false;
            }

            if (canListPlayers && NetManager.JoinedRoom)
            {
                Debug.Log("Warn");
                for (int c = 0; c < PlayerList.transform.childCount; c++)
                {
                    Destroy(PlayerList.transform.GetChild(c).gameObject);
                }

                int i = 0;
                foreach (string playerName in listPlayersArgs)
                {
                    GameObject playerItem = InstantiateUI(PlayerItem, PlayerList.transform);

                    playerItem.GetComponent<Button>().onClick.AddListener(() => KickPlayer(playerName));

                    Debug.Log(playerName + ", " + NetManager.PlayerRoomName);
                    if (playerName == NetManager.PlayerRoomName)
                    {
                        ColorBlock ncb = new ColorBlock();
                        ncb.colorMultiplier = playerItem.GetComponent<Button>().colors.colorMultiplier;
                        ncb.disabledColor = new Color32(100, 100, 100, 100);
                        playerItem.GetComponent<Button>().colors = ncb;
                        playerItem.GetComponent<Button>().interactable = false;
                    }

                    TextMeshProUGUI textE = playerItem.GetComponentInChildren<TextMeshProUGUI>();
                    textE.text = playerName;

                    i++;
                }

                canListPlayers = false;
            }
        }

        void OnApplicationQuit()
        {
            NetManager.CloseConnection();
        }
    }
}