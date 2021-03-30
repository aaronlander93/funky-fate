using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestMove : NetworkRequest
{
	public RequestMove()
	{
		request_id = Constants.CMSG_MOVE;
	}

	public void send(int id, string move)
	{
		packet = new GamePacket(request_id);
        packet.addInt32(id);
		packet.addString(move);
	}
}
