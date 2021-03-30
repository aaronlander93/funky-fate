using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
	public int UserID { get; set; }
	public string Name { get; set; }

	public Player(int userID, string name)
	{
		UserID = userID;
		Name = name;
	}

}
