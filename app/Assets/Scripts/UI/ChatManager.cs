using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ChatManager : MonoBehaviour
{
    public int maxMessages = 8;
    public GameObject chatCanvas, chatPanel, textObject;
    public InputField chatBox;
    public PhotonView pv;
    public Movement2D mv;

    private bool chatShowing = false;

    [SerializeField]
    List<Message> messageList = new List<Message>();

    void Start()
    {
        chatCanvas.SetActive(chatShowing);
    }

    void Update()
    {
        if (pv.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Return) && !chatShowing)
            {
                chatShowing = true;
                chatCanvas.SetActive(chatShowing);

                chatBox.ActivateInputField();
                chatBox.Select();

                // Disable movement while chatting
                mv.enabled = false;
            }
            else if (Input.GetKeyDown(KeyCode.Return) && chatBox.text != "")
            {
                SendMessage(pv.Owner.NickName, chatBox.text);
                chatBox.Select();
                chatBox.text = "";
                chatBox.ActivateInputField();
            }
            else if (Input.GetKeyDown(KeyCode.Return) && chatBox.text == "")
            {
                chatShowing = false;
                chatCanvas.SetActive(chatShowing);

                mv.enabled = true;
            }
        }
        
    }

    public void SendMessage(string text)
    {
        if (pv.IsMine)
        {
            pv.RPC("ReceiveMessage", RpcTarget.All, text);
        }
    }

    public void SendMessage(string senderName, string text)
    {
        string message = string.Format("[{0}] {1}", senderName, text);

        pv.RPC("ReceiveMessage", RpcTarget.All, message);
    }

    public void AddMessage(string text)
    {
        // Craft new message
        Message newMessage = new Message();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();

        newMessage.textObject.text = newMessage.text;

        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        // Activate chat panel
        chatShowing = true;
        chatCanvas.SetActive(chatShowing);

        messageList.Add(newMessage);

        StartCoroutine(HideChatAfterMessage());
    }

    [PunRPC]
    void ReceiveMessage(string text)
    {
        // Craft new message
        Message newMessage = new Message();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();

        newMessage.textObject.text = newMessage.text;

        // If this is not my photon view, find my photon view and add the new message to the list
        if (!pv.IsMine)
        {
            // Find my photon view
            var photonViews = GameObject.FindObjectsOfType<PhotonView>();

            ChatManager cm = null;
            foreach (var photonView in photonViews)
            {
                if (photonView.IsMine && photonView.gameObject.tag == "Player")
                {
                    cm = photonView.gameObject.GetComponent<ChatManager>();
                    cm.AddMessage(text);

                    break;
                }
            }
        }
        else
        {
            // This is my photon view
            if (messageList.Count >= maxMessages)
            {
                Destroy(messageList[0].textObject.gameObject);
                messageList.Remove(messageList[0]);
            }

            // Activate chat panel
            chatShowing = true;
            chatCanvas.SetActive(chatShowing);

            messageList.Add(newMessage);

            StartCoroutine(HideChatAfterMessage());
        }
    }

    // Hides the chat window after 4 seconds
    IEnumerator HideChatAfterMessage()
    {
        yield return new WaitForSeconds(4);

        if (!chatBox.isFocused)
        {
            chatShowing = false;
            chatCanvas.SetActive(chatShowing);
        }
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
}
