using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseMaxEventArgs : ExtendedEventArgs
{
    public int max { get; set; } 

    public ResponseMaxEventArgs()
    {
        event_id = Constants.SMSG_MAX;
    }
}

public class ResponseMax : NetworkResponse
{
    private int max;

    public ResponseMax()
    {
    }

    public override void parse()
    {
        max = DataReader.ReadInt(dataStream);
    }

    public override ExtendedEventArgs process()
    {
        ResponseMaxEventArgs args = new ResponseMaxEventArgs
        {
            max = max
        };

        return args;
    }
}
