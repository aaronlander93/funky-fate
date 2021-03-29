using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TicTacToePlayer {
    public int userID { get; set; }
    public Image panel;
    public Text text;
}

[System.Serializable]
public class PlayerColor {
   public Color panelColor;
   public Color textColor;
}

public class TicTacToeController : MonoBehaviour {

    public Text[] buttonList;
    public Text gameOverText;
    public GameObject gameOverPanel;
    public GameObject restartButton;
    public TicTacToePlayer playerX;
    public TicTacToePlayer playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;

    private string playerSide;
    private int moveCount;
    private string sceneName;
    private bool isNetworkGame;
    private Scene m_Scene;
    private NetworkManager networkManager;
    private bool ready = false;
	private bool opReady = false;
    private bool opLeft = false;
    private GameObject messageBox;
    private GameObject readyMessageBox;
    private GameObject waitingForMoveMessage;
	private TMPro.TextMeshProUGUI messageBoxMsg;

    void Start() {
        m_Scene = SceneManager.GetActiveScene();
        sceneName = m_Scene.name;

        messageBox = GameObject.Find("Message Box");
        messageBoxMsg = messageBox.transform.Find("Message").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        messageBox.SetActive(false);

        SetGameControllerReferenceOnButtons();
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        playerSide = "X";
        moveCount = 0;
        playerX.userID = 1;
        playerO.userID = 2;
        SetPlayerColors(playerX, playerO);

        if(sceneName == "TTTNetwork") {
            Constants.USER_ID = -1;
            Constants.OP_ID = -1;

            isNetworkGame = true;
            networkManager = GameObject.Find("Network Manager").GetComponent<NetworkManager>();

            waitingForMoveMessage = GameObject.Find("Waiting Message");
            waitingForMoveMessage.SetActive(false);
            readyMessageBox = GameObject.Find("ReadyMessage");
            readyMessageBox.SetActive(false);
            
            // Prevent input before the connection is ready
            SetBoardInteractable(false);

            bool connected = networkManager.SendJoinRequest();
            if (!connected)
            {
                SetGameOverText("Unable to connect to server.", 40);
            }

            MessageQueue msgQueue = networkManager.GetComponent<MessageQueue>();

            msgQueue.AddCallback(Constants.SMSG_JOIN, OnResponseJoin);
		    msgQueue.AddCallback(Constants.SMSG_LEAVE, OnResponseLeave);
		    msgQueue.AddCallback(Constants.SMSG_READY, OnResponseReady);
            msgQueue.AddCallback(Constants.SMSG_MOVE, OnResponseMove);
        }
        else {
            isNetworkGame = false;
        }
    }

    void SetGameControllerReferenceOnButtons () {
        for(int i = 0; i < buttonList.Length; i++) {
            buttonList[i].GetComponentInParent<GridSpace>().SetControllerReference(this);
        }
    }

    public string GetPlayerSide () {
        return playerSide;
    }

    void ChangeSides () {
        playerSide = (playerSide == "X") ? "O" : "X";

        if (playerSide == "X") {
            SetPlayerColors(playerX, playerO);
        } 
        else {
            SetPlayerColors(playerO, playerX);
        }
    }

    public void EndTurn(int buttonIndex) {
        moveCount++;

        if(isNetworkGame) {
            networkManager.SendMoveRequest(buttonIndex);
            SetBoardInteractable(false);
        }
        if (CheckWin(playerSide)) {
            GameOver(playerSide);
        }
        else if(moveCount >= 9) {
            GameOver("draw");
        }
        else {
            ChangeSides();
            if (isNetworkGame) {
                waitingForMoveMessage.SetActive(true);
            }
        }
    }

    public bool CheckWin(string playerSide) {
        if (
            buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide
            || buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide
            || buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide
            || buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide
            || buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide
            || buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide
            || buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide
            || buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide
            ) {
            return true;
        } else {
            return false;
        }
    }

    void GameOver(string winner) {
        SetBoardInteractable(false);

        if (isNetworkGame) {
            ready = false;
            opReady = false;
            readyMessageBox.SetActive(true);
        } else {
            restartButton.SetActive(true);
        }

        if(winner == "draw") { 
            SetGameOverText("It's a Draw!"); 
        } 
        else { 
            SetGameOverText(winner + " Wins!"); 
        }
    }

    void SetGameOverText(string value, int size = 64) {
        gameOverPanel.SetActive(true); 
        gameOverText.text = value;
        gameOverText.fontSize = size;
    }

    void SetBoardInteractable(bool toggle) {
        for (int i = 0; i < buttonList.Length; i++) {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }
    
    public void RestartGame() {
        playerSide = "X";
        moveCount = 0;
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        SetPlayerColors(playerX, playerO);

        SetBoardInteractable(true);
        for (int i = 0; i < buttonList.Length; i++) {
            buttonList[i].text = "";
        }
    }

    void SetPlayerColors(TicTacToePlayer newPlayer, TicTacToePlayer oldPlayer) {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;

        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    public void OnResponseMove(ExtendedEventArgs eventArgs) {
        ResponseMoveEventArgs args = eventArgs as ResponseMoveEventArgs;

        //Debug.Log("args.user_id: " + args.user_id);
        //Debug.Log("Constants.USER_ID: " + Constants.USER_ID);
        if (args.user_id != Constants.USER_ID) {
            if(args.user_id != 1) {
            buttonList[args.moveIndex].text = "O";
            }
            else {
                buttonList[args.moveIndex].text = "X";
            }

            waitingForMoveMessage.SetActive(false);

            if (CheckWin(playerSide)) {
                GameOver(playerSide);
            } else {
                for (int i = 0; i < buttonList.Length; i++)
                {
                    if (buttonList[i].text == "")
                    {
                        buttonList[i].GetComponentInParent<Button>().interactable = true;
                    }
                }
                ChangeSides();
            }
        }
    }

    public void OnResponseJoin(ExtendedEventArgs eventArgs)
	{
        Debug.Log("RECEIVED JOIN RESPONSE");
		ResponseJoinEventArgs args = eventArgs as ResponseJoinEventArgs;
		if (args.status == 0)
		{
            if(args.user_id != 1 && args.user_id != 2)
			{
				Debug.Log("ERROR: Invalid user_id in ResponseJoin: " + args.user_id);
				messageBoxMsg.text = "Error joining game. Network returned invalid response.";
				messageBox.SetActive(true);
				return;
			}

            // if already in server and recieve join response
            // i.e. another player has connected
            if(Constants.USER_ID != -1) {
                Debug.Log("RECEIVED SECOND JOIN RESPONSE");
                // hide "waiting for opponent" message
                gameOverPanel.SetActive(false);
            }
            else {
            // Not so constant, huh?
			Constants.USER_ID = args.user_id;
			Constants.OP_ID = 3 - args.user_id;

            // if we are playing as O
            if(args.user_id == 2) {
                // move the "waiting for opponent's move" message so it's under the X icon
                RectTransform waitingMessageTransform = waitingForMoveMessage.GetComponent<RectTransform>();
                var pos = waitingMessageTransform.anchoredPosition;
                waitingMessageTransform.anchoredPosition = new Vector2(pos.x * -1, pos.y);
            }

			if (args.op_id > 0)
			{
				if (args.op_id == Constants.OP_ID)
				{
					opReady = args.op_ready;
				}
				else
				{
					Debug.Log("ERROR: Invalid op_id in ResponseJoin: " + args.op_id);
					messageBoxMsg.text = "Error joining game. Network returned invalid response.";
					messageBox.SetActive(true);
					return;
				}
			}
			else
			{
				SetGameOverText("Waiting for opponent...", 40);
			}

            readyMessageBox.SetActive(true);
            }
		}
		else
		{
			SetGameOverText("Server is full.", 40);
		}
	}
    
	public void OnLeave()
	{
		Debug.Log("Send LeaveReq");
		networkManager.SendLeaveRequest();
		ready = false;
	}

	public void OnResponseLeave(ExtendedEventArgs eventArgs)
	{
		ResponseLeaveEventArgs args = eventArgs as ResponseLeaveEventArgs;
        Debug.Log("args.user_id: " + args.user_id + " Constants.USER_ID: " + Constants.USER_ID);
        if (args.user_id != Constants.USER_ID) {
            SetBoardInteractable(false);
            messageBoxMsg.text = "Opponent has left the game.";
            messageBox.SetActive(true);
            waitingForMoveMessage.SetActive(false);
            opLeft = true;
		} else {
            networkManager.CloseNetworkSocket();
            SceneManager.LoadScene("Main Menu");
        }
	}

	public void OnResponseReady(ExtendedEventArgs eventArgs)
	{
        Debug.Log("RECEIVED READY RESPONSE");
		ResponseReadyEventArgs args = eventArgs as ResponseReadyEventArgs;
		if (Constants.USER_ID == -1) // Haven't joined, but got ready message
		{
			opReady = true;
		}
		else
		{
			if (args.user_id == Constants.OP_ID)
			{
				opReady = true;
			}
			else if (args.user_id == Constants.USER_ID)
			{
				ready = true;
			}
			else
			{
				Debug.Log("ERROR: Invalid user_id in ResponseReady: " + args.user_id);
				messageBoxMsg.text = "Error starting game. Network returned invalid response.";
				messageBox.SetActive(true);
				return;
			}
		}

		if (ready && opReady)
		{
            playerSide = "X";
            moveCount = 0;
            gameOverPanel.SetActive(false);
            for (int i = 0; i < buttonList.Length; i++)
            {
                buttonList[i].text = "";
            }

            if (Constants.USER_ID == 1) {
                SetBoardInteractable(true);
            }
            else {
                waitingForMoveMessage.SetActive(true);
            }
		}
	}

	public void OnMessageButtonClick() {
		messageBox.SetActive(false);
        if (opLeft) {
            networkManager.SendLeaveRequest();
        }
	}

    public void OnReadyClick() {
        readyMessageBox.SetActive(false);
        Debug.Log("Send ReadyReq");
		networkManager.SendReadyRequest();
    }

    public void OnQuitToMenuClick() {
        if (isNetworkGame) {
            networkManager.SendLeaveRequest();
        } else {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
