using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour {
    
    public Button button;
    public Text buttonText;

    private TicTacToeController controller;

    public void SetControllerReference(TicTacToeController controller) {
        this.controller = controller;
    }
    
    public void SetSpace() {
        buttonText.text = controller.GetPlayerSide();
        button.interactable = false;
        controller.EndTurn();
    }
}
