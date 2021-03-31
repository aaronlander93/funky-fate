using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseResultEventArgs : ExtendedEventArgs
{
    public int result { get; set; } 

    public ResponseResultEventArgs()
    {
        event_id = Constants.SMSG_RESULT;
    }
}

public class ResponseResult : NetworkResponse
{
    private int result;

    public ResponseResult()
    {
    }

    public override void parse()
    {
        result = DataReader.ReadInt(dataStream);
    }

    public override ExtendedEventArgs process()
    {
        ResponseResultEventArgs args = new ResponseResultEventArgs
        {
            result = result
        };

        return args;
    }
}
