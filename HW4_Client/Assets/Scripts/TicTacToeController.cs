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

    public TicTacToePlayer(int userID, Image panel, Text text) {
        this.userID = userID;
        this.panel = panel;
        this.text = text;
    }
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
    private GameObject messageBox;
    private GameObject readyMessageBox;
	private TMPro.TextMeshProUGUI messageBoxMsg;

    void Start() {
        m_Scene = SceneManager.GetActiveScene();
        sceneName = m_Scene.name;

        messageBox = GameObject.Find("Message Box");
        messageBoxMsg = messageBox.transform.Find("Message").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        messageBox.SetActive(false);

        readyMessageBox = GameObject.Find("ReadyMessage");
        readyMessageBox.SetActive(false);

        SetGameControllerReferenceOnButtons();
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        playerSide = "X";
        moveCount = 0;
        SetPlayerColors(playerX, playerO);

        if(sceneName == "TTTNetwork") {
            isNetworkGame = true;
            networkManager = GameObject.Find("Network Manager").GetComponent<NetworkManager>();
            
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

    public void EndTurn () {
        moveCount++;

        if (
            buttonList [0].text == playerSide && buttonList [1].text == playerSide && buttonList [2].text == playerSide
            || buttonList [3].text == playerSide && buttonList [4].text == playerSide && buttonList [5].text == playerSide
            || buttonList [6].text == playerSide && buttonList [7].text == playerSide && buttonList [8].text == playerSide
            || buttonList [0].text == playerSide && buttonList [3].text == playerSide && buttonList [6].text == playerSide
            || buttonList [1].text == playerSide && buttonList [4].text == playerSide && buttonList [7].text == playerSide
            || buttonList [2].text == playerSide && buttonList [5].text == playerSide && buttonList [8].text == playerSide
            || buttonList [0].text == playerSide && buttonList [4].text == playerSide && buttonList [8].text == playerSide
            || buttonList [2].text == playerSide && buttonList [4].text == playerSide && buttonList [6].text == playerSide
            ) {
            GameOver(playerSide);
        }
        else if(moveCount >= 9) {
            GameOver("draw");
        }
        else {
            ChangeSides();
        }
    }

    void GameOver(string winner) {
        SetBoardInteractable(false);
        restartButton.SetActive(true);

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

    }

    public void OnResponseJoin(ExtendedEventArgs eventArgs)
	{
		ResponseJoinEventArgs args = eventArgs as ResponseJoinEventArgs;
		if (args.status == 0)
		{
            // If you're already in the server, but recieve a join response
			// i.e. the other player has joined
			if(Constants.USER_ID != -1) {
				// remove "waiting for opponent" message
				gameOverPanel.SetActive(false);
			}
			if(args.user_id != 1 && args.user_id != 2)
			{
				Debug.Log("ERROR: Invalid user_id in ResponseJoin: " + args.user_id);
				messageBoxMsg.text = "Error joining game. Network returned invalid response.";
				messageBox.SetActive(true);
				return;
			}

            // Not so constant, huh?
			Constants.USER_ID = args.user_id;
			Constants.OP_ID = 3 - args.user_id;

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
		if (args.user_id != Constants.USER_ID)
		{
			SetGameOverText("Waiting for opponent...", 40);
			opReady = false;
            RestartGame();
		}
	}

    // TODO: add ready message box & button
	public void OnResponseReady(ExtendedEventArgs eventArgs)
	{
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
			//StartNetworkGame();
            if(Constants.USER_ID == 1) {
                SetBoardInteractable(true);
            }
            else {

            }
		}
	}

	public void OnMessageButtonClick() {
		messageBox.SetActive(false);
	}

    public void OnReadyClick() {
        readyMessageBox.SetActive(false);
        Debug.Log("Send ReadyReq");
		networkManager.SendReadyRequest();
    }

    public void QuitToMenuOnClick() {
        SceneManager.LoadScene("Main Menu");
    }
}
