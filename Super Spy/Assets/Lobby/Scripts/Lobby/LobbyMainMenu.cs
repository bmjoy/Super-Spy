using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Prototype.NetworkLobby
{
    //Main menu, mainly only a bunch of callback called by the UI (setup throught the Inspector)
    public class LobbyMainMenu : MonoBehaviour 
    {
        public LobbyManager lobbyManager;
        public RectTransform lobbyPanel;
        public InputField roomInput;
		public Button joinRoom;

        public void OnEnable()
        {
            lobbyManager.topPanel.ToggleVisibility(true);
			roomInput.onEndEdit.RemoveAllListeners();
			roomInput.onEndEdit.AddListener(onEndEditRoomName);
			roomInput.onValueChanged.RemoveAllListeners();
			roomInput.onValueChanged.AddListener(onEditRoomName);
        }

        public void OnClickHost()
        {
            lobbyManager.StartHost();
        }

        public void OnClickJoin()
        {
			lobbyManager.ChangeTo(lobbyPanel);
			lobbyManager.DisplayIsConnecting();
			lobbyManager.networkAddress = roomInput.text;
			lobbyManager.StartClient();
			lobbyManager.backDelegate = lobbyManager.StopClientClbk;
        }

		void onEditRoomName(string text) {
			joinRoom.interactable = text.Length != 0;
		}

		void onEndEditRoomName(string text)
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				OnClickJoin();
			}
		}
    }
}
