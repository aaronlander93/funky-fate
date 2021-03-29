using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseMoveEventArgs : ExtendedEventArgs
{
	public int user_id { get; set; } // The user_id of whom who sent the request
	public int moveIndex { get; set; }

	public ResponseMoveEventArgs()
	{
		event_id = Constants.SMSG_MOVE;
	}
}

public class ResponseMove : NetworkResponse
{
	private int user_id;
	private int moveIndex;

	public ResponseMove()
	{
	}

	public override void parse()
	{
		user_id = DataReader.ReadInt(dataStream);
		moveIndex = DataReader.ReadInt(dataStream);
	}

	public override ExtendedEventArgs process()
	{
		ResponseMoveEventArgs args = new ResponseMoveEventArgs
		{
			user_id = user_id,
			moveIndex = moveIndex
		};

		return args;
	}
}
