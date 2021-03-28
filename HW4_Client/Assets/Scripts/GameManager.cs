using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	// public Player[] Players = new Player[2];

	// private int currentPlayer = 1;
	// private bool useNetwork;
	// private NetworkManager networkManager;

	// void Start()
	// {
	// 	DontDestroyOnLoad(gameObject);
	// 	networkManager = GameObject.Find("Network Manager").GetComponent<NetworkManager>();
	// 	MessageQueue msgQueue = networkManager.GetComponent<MessageQueue>();
	// 	msgQueue.AddCallback(Constants.SMSG_MOVE, OnResponseMove);
	// 	msgQueue.AddCallback(Constants.SMSG_INTERACT, OnResponseInteract);
	// }

	// public Player GetCurrentPlayer()
	// {
	// 	return Players[currentPlayer - 1];
	// }

	// public void Init(Player player1, Player player2)
	// {
	// 	Players[0] = player1;
	// 	Players[1] = player2;
	// 	currentPlayer = 1;
	// 	useNetwork = (!player1.isMouseControlled || !player2.isMouseControlled);
	// }

	// public void EndTurn()
	// {
	// 	currentPlayer = (currentPlayer == 1) ? 2 : 1;
	// }

	// public void ProcessClick(GameObject hitObject)
	// {
		
	// 	if (useNetwork)
	// 	{
	// 		networkManager.SendMoveRequest(hero.Index, x, y);
	// 	}
		
	// }

	// public void OnResponseMove(ExtendedEventArgs eventArgs)
	// {
	// 	ResponseMoveEventArgs args = eventArgs as ResponseMoveEventArgs;
	// 	if (args.user_id == Constants.OP_ID)
	// 	{
			
	// 	}
	// 	else if (args.user_id == Constants.USER_ID)
	// 	{
	// 		// Ignore
	// 	}
	// 	else
	// 	{
	// 		Debug.Log("ERROR: Invalid user_id in ResponseReady: " + args.user_id);
	// 	}
	// }
}
