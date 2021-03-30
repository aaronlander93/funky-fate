using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameChoices {
    NONE,
    ROCK,
    PAPER,
    SCISSORS
}

public class GameplayController : MonoBehaviour
{
    [SerializeField]
    private Sprite rock, paper, scissors;

    [SerializeField]
    private Image p1_choice_img, p2_choice_img;

    [SerializeField]
    private Text info;

    private GameChoices p1_choice = GameChoices.NONE, p2_choice = GameChoices.NONE;

    private AnimationController animationController;

    void Awake() {
        animationController = GetComponent<AnimationController>();
    }

    public void SetChoices(GameChoices gameChoices) {
        switch(gameChoices) {
            case GameChoices.ROCK:
                p1_choice_img.sprite = rock;
                p1_choice = GameChoices.ROCK;
                break;
            case GameChoices.PAPER:
                p1_choice_img.sprite = paper;
                p1_choice = GameChoices.PAPER;
                break;
            case GameChoices.SCISSORS:
                p1_choice_img.sprite = scissors;
                p1_choice = GameChoices.SCISSORS;
                break;
        }

        SetP2Choice();
        DetermineWinner();
    }

    void SetP2Choice() {
        int rnd = Random.Range(0,3);
        switch(rnd) {
            case 0:
                p2_choice_img.sprite = rock;
                p2_choice = GameChoices.ROCK;
                break;
            case 1:
                p2_choice_img.sprite = paper;
                p2_choice = GameChoices.PAPER;
                break;
            case 2:
                p2_choice_img.sprite = scissors;
                p2_choice = GameChoices.SCISSORS;
                break;

        }
    }

    void DetermineWinner() {
        if (p1_choice == p2_choice) {
            //draw
            info.text = "DRAW";
            StartCoroutine(DisplayWinnerAndRestart());
            return;
        }
        else if ((p1_choice == GameChoices.ROCK && p2_choice == GameChoices.SCISSORS) ||
            (p1_choice == GameChoices.PAPER && p2_choice == GameChoices.ROCK) ||
            (p1_choice == GameChoices.SCISSORS && p2_choice == GameChoices.PAPER)) {
            info.text = "You WIN!";
            StartCoroutine(DisplayWinnerAndRestart());
            return;
        }
        else {
            info.text = "You LOSE";
            StartCoroutine(DisplayWinnerAndRestart());
            return;
        }
    }

    IEnumerator DisplayWinnerAndRestart() {
        yield return new WaitForSeconds(2f);
        info.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        info.gameObject.SetActive(false);
        animationController.ResetAnimations();
    }
}
