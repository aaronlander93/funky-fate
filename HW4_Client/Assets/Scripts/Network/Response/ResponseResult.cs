using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseResultEventArgs : ExtendedEventArgs
{
    public int user_id { get; set; }
    public int result { get; set; } 

    public ResponseResultEventArgs()
    {
        event_id = Constants.SMSG_RESULT;
    }
}

public class ResponseResult : NetworkResponse
{
    private int user_id;
    private int result;

    public ResponseResult()
    {
    }

    public override void parse()
    {
        user_id = DataReader.ReadInt(dataStream);
        result = DataReader.ReadInt(dataStream);
    }

    public override ExtendedEventArgs process()
    {
        ResponseResultEventArgs args = new ResponseResultEventArgs
        {
            user_id = user_id,
            result = result
        };

        return args;
    }
}
