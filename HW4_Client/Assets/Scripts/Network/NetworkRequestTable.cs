using UnityEngine;

using System;
using System.Collections.Generic;

public class NetworkRequestTable {

	public static Dictionary<short, Type> requestTable { get; set; }
	
	public static void init() {
		requestTable = new Dictionary<short, Type>();
		add(Constants.CMSG_JOIN, "RequestJoin"); 			//101
		add(Constants.CMSG_LEAVE, "RequestLeave"); 			//102
		add(Constants.CMSG_SETNAME, "RequestSetName"); 		//103
		add(Constants.CMSG_READY, "RequestReady"); 			//104
		add(Constants.CMSG_MOVE, "RequestMove"); 			//105
		add(Constants.CMSG_INTERACT, "RequestInteract");	//106
		add(Constants.CMSG_SCORE, "RequestScore");			//107
		add(Constants.CMSG_MAX, "RequestMax");				//108
		add(Constants.CMSG_RESULT, "RequestResult");		//109
	}
	
	public static void add(short request_id, string name) {
		requestTable.Add(request_id, Type.GetType(name));
	}
	
	public static NetworkRequest get(short request_id) {
		NetworkRequest request = null;
		
		if (requestTable.ContainsKey(request_id)) {
			request = (NetworkRequest) Activator.CreateInstance(requestTable[request_id]);
			request.request_id = request_id;
		} else {
			Debug.Log("Request [" + request_id + "] Not Found");
		}
		
		return request;
	}
}
