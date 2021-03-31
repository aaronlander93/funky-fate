using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestResult : NetworkRequest
{
    public RequestResult()
    {
        request_id = Constants.CMSG_RESULT;
    }

    public void send(string m1, string m2)
    {
        packet = new GamePacket(request_id);
        packet.addString(m1);
        packet.addString(m2);
    }
}
