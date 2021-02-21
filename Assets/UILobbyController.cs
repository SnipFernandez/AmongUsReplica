using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyController : MonoBehaviour
{
    private int numPlayers = 0;
    public Text tNumPlayer;
    public Button bStartGame;

    public void incrementPlayer()
    {
        // Esto quitar
        numPlayers = 6;


        numPlayers++;
        tNumPlayer.text = $"{numPlayers}/6";

        if(numPlayers >= 6)
        {
            bStartGame.interactable = true;
        }
        else
        {
            bStartGame.interactable = false;
        }
    }
}
