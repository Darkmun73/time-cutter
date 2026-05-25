using System;
using UnityEngine;

[RequireComponent(typeof(DebugTools))]
public class DebugGUI : MonoBehaviour
{
    private DebugTools tools;

    private bool isGUIEnabled;
    private string currencyAmount = "0";
    private string launchForce = "0";

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

        launchForce = GUI.TextField(new Rect(130, 0, 100, 35), launchForce, textFieldStyle);
        bool launch = GUI.Button(new Rect(130, 40, 125, 35), "Launch up", buttonStyle);

        bool spawnPlayer = GUI.Button(new Rect(260, 0, 160, 35), "Spawn player", buttonStyle);
        bool killPlayer = GUI.Button(new Rect(425, 0, 160, 35), "Kill player", buttonStyle);
        bool revivePlayer = GUI.Button(new Rect(425, 40, 160, 35), "Revive player", buttonStyle);

        if (increasing)
            tools.IncreaseAmountOfPlayerCurrency(int.Parse(currencyAmount));
        if (decreasing)
            tools.DecreaseAmountOfPlayerCurrency(int.Parse(currencyAmount));

        if (launch)
            tools.LaunchPlayerUp(int.Parse(launchForce));

        if (spawnPlayer)
            tools.SpawnPlayer();
        if (killPlayer)
            tools.KillPlayer();
        if (revivePlayer)
            tools.RevivePlayer();
    }

}