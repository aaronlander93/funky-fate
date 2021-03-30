using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private AnimationController animationController;
    private GameplayController gameplayController;

    private string playerChoice;

    void Awake() {
        animationController = GetComponent<AnimationController>();
        gameplayController = GetComponent<GameplayController>();
    }

    public void GetChoice() {
        string choice = UnityEngine.EventSystems.
            EventSystem.current.currentSelectedGameObject.name;

        string selectedChoice = "";

        switch(choice) {
            case "Rock":
                selectedChoice = "Rock";
                break;
            case "Paper":
                selectedChoice = "Paper";
                break;
            case "Scissors":
                selectedChoice = "Scissors";
                break;
        }

        gameplayController.SetChoices(selectedChoice);
        animationController.PlayerMadeChoice();
    }
} // class
