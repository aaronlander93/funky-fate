using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestScore : NetworkRequest
{
	public RequestScore()
	{
		request_id = Constants.CMSG_SCORE;
	}

	public void send(int id, int score)
	{
		packet = new GamePacket(request_id);
        packet.addInt32(id);
        packet.addInt32(score);
	}
}
