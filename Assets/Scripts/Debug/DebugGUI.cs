using System;
using UnityEngine;

[RequireComponent(typeof(DebugTools))]
public class DebugGUI : MonoBehaviour
{
    private DebugTools tools;

    private bool isGUIEnabled;
    private string currencyAmount = "0";

    private GUIStyle textFieldStyle = null;
    private GUIStyle buttonStyle = null;

    void Awake()
    {
        tools = GetComponent<DebugTools>();
    }

    void OnEnable()
    {
        tools.DebugToolsEnabled += Enable;
        tools.DebugToolsDisabled += Disable;
    }

    void OnDisable()
    {
        tools.DebugToolsEnabled -= Enable;
        tools.DebugToolsDisabled -= Disable;
    }

    private void Enable()
    {
        isGUIEnabled = true;
    }

    private void Disable()
    {
        isGUIEnabled = false;
    }

    void OnGUI()
    {
        if (!isGUIEnabled) return;
        
        textFieldStyle ??= new(GUI.skin.textField) { fontSize = 24 };
        buttonStyle ??= new(GUI.skin.button) { fontSize = 24 };

        char chr = Event.current.character;
        if ( !(chr >= '0' && chr <= '9') )
            Event.current.character = '\0';
            
        currencyAmount = GUI.TextField(new Rect(0,0,100,35), currencyAmount, textFieldStyle);
        bool increasing = GUI.Button(new Rect(0, 40, 125, 35), "Increase", buttonStyle);
        bool decreasing = GUI.Button(new Rect(0, 85, 125, 35), "Decrease", buttonStyle);

        if (increasing)
            tools.IncreaseAmountOfPlayerCurrency(int.Parse(currencyAmount));
        if (decreasing)
            tools.DecreaseAmountOfPlayerCurrency(int.Parse(currencyAmount));
    }

}