using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExchangeHtml_Test : MonoBehaviour
{
    public InputField Message;

    public void SendToHtml()
    {
        Application.ExternalCall("ReceiveFromUnity", Message.text);
    }

    public void ReceiveFromHtml(string message)
    {
        Message.text = message;
    }
}
