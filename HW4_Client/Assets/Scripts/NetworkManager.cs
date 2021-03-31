using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
	private ConnectionManager cManager;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);

		gameObject.AddComponent<MessageQueue>();
		gameObject.AddComponent<ConnectionManager>();

		NetworkRequestTable.init();
		NetworkResponseTable.init();
	}

	// Start is called before the first frame update
	void Start()
    {
		cManager = GetComponent<ConnectionManager>();

		if (cManager)
		{
			cManager.setupSocket();

			StartCoroutine(RequestHeartbeat(0.1f));
		}
	}

	public bool SendJoinRequest()
	{
		if (cManager && cManager.IsConnected())
		{
			RequestJoin request = new RequestJoin();
			request.send();
			cManager.send(request);
			return true;
		}
		return false;
	}

	public bool SendLeaveRequest()
	{
		if (cManager && cManager.IsConnected())
		{
			RequestLeave request = new RequestLeave();
			request.send();
			cManager.send(request);
			return true;
		}
		return false;
	}

	public bool SendSetNameRequest(string Name)
	{
		if (cManager && cManager.IsConnected())
		{
			RequestSetName request = new RequestSetName();
			request.send(Name);
			cManager.send(request);
			return true;
		}
		return false;
	}

	public bool SendReadyRequest()
	{
		if (cManager && cManager.IsConnected())
		{
			RequestReady request = new RequestReady();
			request.send();
			cManager.send(request);
			return true;
		}
		return false;
	}

    public bool SendMaxRequest()
    {
        if (cManager && cManager.IsConnected())
        {
            RequestMax request = new RequestMax();
            request.send();
            cManager.send(request);
            return true;
        }
        return false;
    }

	public bool SendMoveRequest(int id, String move)
	{
		if (cManager && cManager.IsConnected())
		{
			RequestMove request = new RequestMove();
			request.send(id, move);
			cManager.send(request);
			return true;
		}
		return false;
	}

    public bool SendResultRequest(int id, string m1, string m2)
    {
        if (cManager && cManager.IsConnected())
        {
            RequestResult request = new RequestResult();
            request.send(id, m1, m2);
            cManager.send(request);
            return true;
        }
        return false;
    }

    public bool SendScoreRequest(int id, int score)
    {
        if (cManager && cManager.IsConnected())
        {
            RequestScore request = new RequestScore();
            request.send(id, score);
            cManager.send(request);
            return true;
        }
        return false;
    }

    public IEnumerator RequestHeartbeat(float time)
	{
		yield return new WaitForSeconds(time);

		if (cManager)
		{
			RequestHeartbeat request = new RequestHeartbeat();
			request.send();
			cManager.send(request);
		}

		StartCoroutine(RequestHeartbeat(time));
	}
}
