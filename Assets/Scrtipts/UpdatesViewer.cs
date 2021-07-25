using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

// Responsible for viewing the updates on a UI text
// Controls the number of messages that appears in the UI
public class UpdatesViewer : MonoBehaviour
{
    public TextMeshProUGUI messagesTextUI;
    [Range(3, 10)] public int maxLines = 10;
    public bool addTime = true; // If we want to show the game time with the messages
    private int messagesCounter;
    private string[] messages;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables
        messagesCounter = 0; // At the begining we have 0 messages
        messages = new string[maxLines]; // Save all messages in an array, so we can print them later on
    }

    public void WriteUpdate(string message)
    {
        // If we need to add time stamp
        if (addTime)
        {
            // Append the time stamp at the begining of the message
            message = "[" + GetTime() + "] " + message;
        }
        // If we reached the maximum number of lines
        if (messagesCounter == maxLines)
        {
            // Point to the end of the array (length - 1)
            messagesCounter -= 1;
            // Shift all messages to the left (the newesst message enter at the end of the array)
            Array.Copy(messages, 1, messages, 0, maxLines - 1);
        }
        // Add the message to the array
        messages[messagesCounter] = message;
        messagesCounter += 1;
        // Display the updated messages array
        ShowMessages();
    }

    private void ShowMessages()
    {
        // Delete all previous text in the text UI
        messagesTextUI.text = "";
        // Append every message in the array to the Text UI
        for (int i = 0; i < messagesCounter; i++)
        {
            messagesTextUI.text += messages[i];
            messagesTextUI.text += "\n";
        }
    }

    private string GetTime()
    {
        // Time.time returns the time passed from the begining of the game (in seconds)
        // We use TimeSpan to easily convert the seconds into hours (hh), minutes (mm) and seconds (ss)
        TimeSpan time = TimeSpan.FromSeconds(Time.time);

        //here backslash is must to tell that colon is
        //not the part of format, it just a character that we want in output
        string str = time.ToString(@"hh\:mm\:ss");
        return str;
    }
}