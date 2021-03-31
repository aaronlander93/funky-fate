using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseScoreEventArgs : ExtendedEventArgs
{
    public int user_id { get; set; } // The user_id of whom who sent the request
    public int score { get; set; }

    public ResponseScoreEventArgs()
    {
        event_id = Constants.SMSG_SCORE;
    }
}

public class ResponseScore : NetworkResponse
{
    private int user_id;
    private int score;

    public ResponseScore()
    {
    }

    public override void parse()
    {
        user_id = DataReader.ReadInt(dataStream);
        score = DataReader.ReadInt(dataStream);
    }

    public override ExtendedEventArgs process()
    {
        ResponseScoreEventArgs args = new ResponseScoreEventArgs
        {
            user_id = user_id,
            score = score
        };

        return args;
    }
}
