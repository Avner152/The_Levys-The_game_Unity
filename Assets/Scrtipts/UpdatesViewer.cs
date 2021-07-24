using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UpdatesViewer : MonoBehaviour
{
    public TextMeshProUGUI messagesTextUI;
    [Range(3, 10)] public int maxLines = 10;
    public bool addTime = true;
    private int messagesCounter;
    private string[] messages;

    // Start is called before the first frame update
    void Start()
    {
        messagesCounter = 0;
        messages = new string[maxLines];
    }

    public void WriteUpdate(string message)
    {
        if (addTime)
        {
            message = "[" + GetTime() + "] " + message;
        }
        if (messagesCounter == maxLines)
        {
            messagesCounter -= 1;
            // Shift the messages to the left (the newesst message enter to the end of the array)
            Array.Copy(messages, 1, messages, 0, maxLines - 1);
        }
        messages[messagesCounter] = message;
        messagesCounter += 1;
        ShowMessages();
    }

    private void ShowMessages()
    {
        messagesTextUI.text = "";
        for (int i = 0; i < messagesCounter; i++)
        {
            messagesTextUI.text += messages[i];
            messagesTextUI.text += "\n";
        }
    }

    private string GetTime()
    {
        TimeSpan time = TimeSpan.FromSeconds(Time.time);

        //here backslash is must to tell that colon is
        //not the part of format, it just a character that we want in output
        string str = time.ToString(@"hh\:mm\:ss");
        return str;
    }
}