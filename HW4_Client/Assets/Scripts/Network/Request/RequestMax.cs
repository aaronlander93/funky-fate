using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestMax : NetworkRequest
{
	public RequestMax()
	{
		request_id = Constants.CMSG_MAX;
	}

	public void send()
	{
		packet = new GamePacket(request_id);
	}
}
