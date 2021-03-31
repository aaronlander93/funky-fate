using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{
    [SerializeField]
    private Sprite rock, paper, scissors;

    [SerializeField]
    private Image p1_choice_img, p2_choice_img;

    [SerializeField]
    private Text info;

    [SerializeField]
    private Text p1_score_text, p2_score_text;

    private string p1_choice = "", p2_choice = "";

    private AnimationController animationController;

    private bool game_end;

    private int max_score;

    private int p1_id, p2_id;
    private int p1_score = 0, p2_score = 0;

    private NetworkManager networkManager;

    public Player[] Players = new Player[2];

    private int currentPlayer = 1;

    void Start()
    {
        // DontDestroyOnLoad(gameObject);
        networkManager = GameObject.Find("Network Manager").GetComponent<NetworkManager>();
        MessageQueue msgQueue = networkManager.GetComponent<MessageQueue>();
        msgQueue.AddCallback(Constants.SMSG_MOVE, OnResponseMove);
        msgQueue.AddCallback(Constants.SMSG_MAX, OnResponseMax);
        msgQueue.AddCallback(Constants.SMSG_SCORE, OnResponseScore);
        msgQueue.AddCallback(Constants.SMSG_RESULT, OnResponseResult);

        game_end = false;

        networkManager.SendMaxRequest();
    }

    public Player GetCurrentPlayer()
    {
        return Players[currentPlayer - 1];
    }

    void Awake()
    {
        animationController = GetComponent<AnimationController>();
    }

    public void Init(Player player1, Player player2)
    {
        Players[0] = player1;
        Players[1] = player2;
        currentPlayer = 1;
    }

    public void SetChoices(string gameChoices)
    {

        switch (gameChoices)
        {
            case "Rock":
                p1_choice_img.sprite = rock;
                p1_choice = "Rock";
                break;
            case "Paper":
                p1_choice_img.sprite = paper;
                p1_choice = "Paper";
                break;
            case "Scissors":
                p1_choice_img.sprite = scissors;
                p1_choice = "Scissors";
                break;
        }
        networkManager.SendMoveRequest(currentPlayer, p1_choice);

        print("p2's choice is: " + p2_choice);

        if (!string.IsNullOrEmpty(p2_choice))
        {
            setP2Choice();
            animationController.PlayerMadeChoice();
            networkManager.SendResultRequest(Constants.USER_ID, p1_choice, p2_choice);
        }
    }

    private void setP2Choice()
    {
        switch (p2_choice)
        {
            case "Rock":
                p2_choice_img.sprite = rock;
                p2_choice = "Rock";
                break;
            case "Paper":
                p2_choice_img.sprite = paper;
                p2_choice = "Paper";
                break;
            case "Scissors":
                p2_choice_img.sprite = scissors;
                p2_choice = "Scissors";
                break;
        }
    }

    IEnumerator DisplayWinnerAndMoveOn(int player)
    {
        yield return new WaitForSeconds(2f);
        info.gameObject.SetActive(true);
        if (player == 1)
        {
            p1_score++;
            p1_score_text.text = "Your score: " + p1_score;
        }
        else if (player == 2)
        {
            p2_score++;
            p2_score_text.text = "Their score: " + p2_score;
        }
        if (p1_score == max_score)
        {
            //player 1 wins
            info.text = "You are Victorious!";
        }
        else if (p2_score == max_score)
        {
            //player 2 wins
            info.text = "You are the Loser...";
        }
        else
        {
            yield return new WaitForSeconds(2f);
            info.gameObject.SetActive(false);
            animationController.ResetAnimations();
        }
    }

    public void OnResponseMove(ExtendedEventArgs eventArgs)
    {
        ResponseMoveEventArgs args = eventArgs as ResponseMoveEventArgs;
        if (args.user_id == Constants.OP_ID)
        {
            p2_choice = args.move;
            if (!string.IsNullOrEmpty(p1_choice))
            {
                setP2Choice();
                animationController.PlayerMadeChoice();
                networkManager.SendResultRequest(Constants.USER_ID, p1_choice, p2_choice);
            }
        }
        else if (args.user_id == Constants.USER_ID)
        {
            // Ignore
        }
    }

    public void OnResponseMax(ExtendedEventArgs eventArgs)
    {
        ResponseMaxEventArgs args = eventArgs as ResponseMaxEventArgs;

        max_score = args.max;
    }

    public void OnResponseScore(ExtendedEventArgs eventArgs)
    {
        ResponseScoreEventArgs args = eventArgs as ResponseScoreEventArgs;

        if (args.user_id == Constants.OP_ID)
        {
            p2_score = args.score;
            print("p2 score is: " + p2_score);
            p2_score_text.text = "Their score: " + p2_score;

            if(p2_score == max_score)
            {
                info.text = "You lose...";
                game_end = true;
            }
        }
        else if (args.user_id == Constants.USER_ID)
        {
            p1_score = args.score;
            p1_score_text.text = "Your score: " + p1_score;

            if(p1_score == max_score)
            {
                info.text = "You win!";
                game_end = true;
            }
        }


        StartCoroutine(resetAnimation());
    }

    public void OnResponseResult(ExtendedEventArgs eventArgs)
    {
        ResponseResultEventArgs args = eventArgs as ResponseResultEventArgs;
        handleResult(args.user_id, args.result);
    }

    public void handleResult(int id, int result)
    {
        if (id == Constants.USER_ID)
        {
            switch (result)
            {
                case 0:
                    info.text = "WIN!";
                    networkManager.SendScoreRequest(Constants.USER_ID, p1_score + 1);
                    break;
                case 1:
                    info.text = "LOSE...";
                    break;
                case 2:
                    info.text = "DRAW";
                    break;
            }
        }
    }

    IEnumerator resetAnimation()
    {
        yield return new WaitForSeconds(2f);
        info.gameObject.SetActive(true);
        
        if (!game_end)
        {
            yield return new WaitForSeconds(2f);
            info.gameObject.SetActive(false);

            animationController.ResetAnimations();

            p1_choice = "";
            p2_choice = "";
        }
    }
}