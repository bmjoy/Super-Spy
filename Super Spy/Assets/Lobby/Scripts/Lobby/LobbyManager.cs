using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections;
using System.Collections.Generic;

namespace Prototype.NetworkLobby
{
    public class LobbyManager : NetworkLobbyManager 
    {
        static short MsgKicked = MsgType.Highest + 1;

        static public LobbyManager s_Singleton;
		Dictionary<NetworkConnection, GameObject> gamePlayerOfConnection;

        [Header("Unity UI Lobby")]
        [Tooltip("Time in second between all players ready & match start")]
        public float DayTime = 120;
		public float NightTime = 30;

        [Space]
        [Header("UI Reference")]
        public LobbyTopPanel topPanel;
		public Slider timeSlider;

        public RectTransform mainMenuPanel;
        public RectTransform lobbyPanel;

        public LobbyInfoPanel infoPanel;
        public LobbyCountdownPanel countdownPanel;
        //public GameObject addPlayerButton;

        protected RectTransform currentPanel;

        public Button backButton, startGameButton;

        public Text hostInfo;

        //Client numPlayers from NetworkManager is always 0, so we count (throught connect/destroy in LobbyPlayer) the number
        //of players, so that even client know how many player there is.
        [HideInInspector]
        public int _playerNumber = 0;

        void Start()
        {
            s_Singleton = this;
            currentPanel = mainMenuPanel;
			timeSlider.gameObject.SetActive (false);
			startGameButton.gameObject.SetActive (false);
            backButton.gameObject.SetActive(false);
            GetComponent<Canvas>().enabled = true;

            DontDestroyOnLoad(gameObject);

            SetServerInfo("无");
        }
			
		public override GameObject OnLobbyServerCreateGamePlayer (NetworkConnection conn, short playerControllerId)
		{
			gamePlayerPrefab = gamePlayerOfConnection[conn];
			return base.OnLobbyServerCreateGamePlayer (conn, playerControllerId);
		}
			
        public override void OnLobbyClientSceneChanged(NetworkConnection conn)
        {
            if (SceneManager.GetSceneAt(0).name == lobbyScene)
            {
                if (topPanel.isInGame)
                {
                    ChangeTo(lobbyPanel);
					if (conn.playerControllers[0].unetView.isClient)
					{
						backDelegate = StopHostClbk;
					}
					else
					{
						backDelegate = StopClientClbk;
					}
                }
                else
                {
                    ChangeTo(mainMenuPanel);
                }

                topPanel.ToggleVisibility(true);
                topPanel.isInGame = false;
            }
            else
            {
                ChangeTo(null);

                Destroy(GameObject.Find("MainMenuUI(Clone)"));

                //backDelegate = StopGameClbk;
                topPanel.isInGame = true;
                topPanel.ToggleVisibility(false);
				timeSlider.gameObject.SetActive (true);
				StartCoroutine (ServerCountdownCoroutine());
            }
        }

        public void ChangeTo(RectTransform newPanel)
        {
            if (currentPanel != null)
            {
                currentPanel.gameObject.SetActive(false);
            }

            if (newPanel != null)
            {
                newPanel.gameObject.SetActive(true);
            }

            currentPanel = newPanel;

            if (currentPanel != mainMenuPanel)
            {
                backButton.gameObject.SetActive(true);
            }
            else
            {
                backButton.gameObject.SetActive(false);
                SetServerInfo("无");
            }
        }

        public void DisplayIsConnecting()
        {
            var _this = this;
            infoPanel.Display("正在连接...", "取消", () => { _this.backDelegate(); });
        }

        public void SetServerInfo(string host)
        {
            hostInfo.text = host;
        }


        public delegate void BackButtonDelegate();
        public BackButtonDelegate backDelegate;
        public void GoBackButton()
        {
            backDelegate();
			topPanel.isInGame = false;
        }

        // ----------------- Server management

        public void AddLocalPlayer()
        {
            TryToAddPlayer();
        }

        public void RemovePlayer(LobbyPlayer player)
        {
            player.RemovePlayer();
        }

        public void SimpleBackClbk()
        {
            ChangeTo(mainMenuPanel);
        }
                 
        public void StopHostClbk()
        {
			StopHost();
            ChangeTo(mainMenuPanel);
        }

        public void StopClientClbk()
        {
            StopClient();
            ChangeTo(mainMenuPanel);
        }

        public void StopServerClbk()
        {
            StopServer();
            ChangeTo(mainMenuPanel);
        }

        class KickMsg : MessageBase { }
        public void KickPlayer(NetworkConnection conn)
        {
            conn.Send(MsgKicked, new KickMsg());
        }

        public void KickedMessageHandler(NetworkMessage netMsg)
        {
            infoPanel.Display("您被踢出了房间！", "关闭", null);
            netMsg.conn.Disconnect();
        }

        //===================

        public override void OnStartHost()
        {
			base.OnStartHost ();
            ChangeTo(lobbyPanel);
            backDelegate = StopHostClbk;
			startGameButton.gameObject.SetActive (true);
			startGameButton.interactable = false;
			SetServerInfo (Network.player.ipAddress);
        }

        //allow to handle the (+) button to add/remove player
        public void OnPlayersNumberModified(int count)
        {
            _playerNumber += count;
        }

        // ----------------- Server callbacks ------------------

        //we want to disable the button JOIN if we don't have enough player
        //But OnLobbyClientConnect isn't called on hosting player. So we override the lobbyPlayer creation
        public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
        {
            GameObject obj = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;

            LobbyPlayer newPlayer = obj.GetComponent<LobbyPlayer>();
            newPlayer.ToggleJoinButton(numPlayers + 1 >= minPlayers);


            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                LobbyPlayer p = lobbySlots[i] as LobbyPlayer;

                if (p != null)
                {
                    p.RpcUpdateRemoveButton();
                    p.ToggleJoinButton(numPlayers + 1 >= minPlayers);
                }
            }

            return obj;
        }

        public override void OnLobbyServerPlayerRemoved(NetworkConnection conn, short playerControllerId)
        {
            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                LobbyPlayer p = lobbySlots[i] as LobbyPlayer;

                if (p != null)
                {
                    p.RpcUpdateRemoveButton();
                    p.ToggleJoinButton(numPlayers + 1 >= minPlayers);
                }
            }
        }

        public override void OnLobbyServerDisconnect(NetworkConnection conn)
        {
            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                LobbyPlayer p = lobbySlots[i] as LobbyPlayer;

                if (p != null)
                {
                    p.RpcUpdateRemoveButton();
                    p.ToggleJoinButton(numPlayers >= minPlayers);
                }
            }

        }

        // --- Countdown management

        public override void OnLobbyServerPlayersReady()
        {
			bool allready = true;
			for(int i = 0; i < lobbySlots.Length; ++i)
			{
				if(lobbySlots[i] != null)
					allready &= lobbySlots[i].readyToBegin;
			}

			if(allready)
				startGameButton.interactable = true;
        }

		public void StartToLoad() {
			gamePlayerOfConnection = new Dictionary<NetworkConnection, GameObject> ();
			for (int i = 0; i < lobbySlots.Length; ++i)
			{
				if (lobbySlots[i] != null)
				{
					LobbyPlayer player = (lobbySlots [i] as LobbyPlayer);
					int index = 0;
					if (player.playerColor == Color.blue) {
						index = 1;
					}
					gamePlayerOfConnection[player.connectionToClient] = (spawnPrefabs [index]);
				}
			}
			ServerChangeScene(playScene);
			startGameButton.interactable = false;
		}

        public IEnumerator ServerCountdownCoroutine()
        {
			int isDay = 0;
			FogOfWarEffect fow = null;
			timeSlider.onValueChanged.AddListener(delegate(float arg0) {
				if (arg0 <= 0) {
					timeSlider.maxValue = NightTime;
					isDay = -1;
					fow = Camera.main.gameObject.AddComponent<FogOfWarEffect>();
				}
				else if (arg0 >= timeSlider.maxValue) {
					timeSlider.value = timeSlider.maxValue = DayTime;
					isDay = 1;
					if (fow) {
						Destroy(fow);
					}
				}
			});

			if (NetworkServer.active) {
				timeSlider.value = timeSlider.maxValue;
				while (true)
				{
					yield return null;

					timeSlider.value -= Time.deltaTime * isDay;
					for (int i = 0; i < lobbySlots.Length; ++i)
					{
						if (lobbySlots[i] != null)
						{//there is maxPlayer slots, so some could be == null, need to test it before accessing!
							LobbyPlayer p = (lobbySlots[i] as LobbyPlayer);
							p.RpcUpdateCountdown(timeSlider.value, timeSlider.maxValue);
						}
					}
				}
			}
        }

        // ----------------- Client callbacks ------------------

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);

            infoPanel.gameObject.SetActive(false);

            conn.RegisterHandler(MsgKicked, KickedMessageHandler);

            if (!NetworkServer.active)
            {//only to do on pure client (not self hosting client)
                ChangeTo(lobbyPanel);
                backDelegate = StopClientClbk;
				SetServerInfo(networkAddress);
            }
        }


        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);
            ChangeTo(mainMenuPanel);
        }

        public override void OnClientError(NetworkConnection conn, int errorCode)
        {
            ChangeTo(mainMenuPanel);
            infoPanel.Display(errorCode == 6 ? "连接超时" : errorCode.ToString(), "关闭", null);
        }
    }
}
