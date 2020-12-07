using Nakama;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UiController : MonoBehaviour
{
    public InputField matchName;
    private List<GameObject> _list = new List<GameObject>();
    private List<PlayerController> _characters = new List<PlayerController>();
    public GameObject content;
    public Camera camera;
    public GameObject panel;
    public NakamaTesting nakamaTesting;

    public void setMatchName(string val) {
        matchName.text = val;
    }

    public void refreshJoinList(List<IUserPresence> list)
    {
        foreach(IUserPresence u in list)
        {
            GameObject g = _list.Find(l => l.name == u.SessionId);
            if (g != null){
                g.GetComponent<Text>().text = u.UserId + "/" + u.Status?.ToString() + "/" + u.Username + "/" +  u.SessionId;
            }
            else
            {
                g = new GameObject(u.SessionId);
                g.transform.parent = content.transform;
                Text t = g.AddComponent<Text>();
                t.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                t.text = u.UserId + "/" + u.Status?.ToString() + "/" + u.Username + "/" + u.SessionId;
                t.resizeTextForBestFit = true;
                _list.Add(g);
            }
        }
    }

    public void startGame(string name)
    {
        Vector3 vector3 = new Vector3(UnityEngine.Random.Range(0, 10f), 1.08f, UnityEngine.Random.Range(0, 10f));

        CreateCharacter(name, vector3);

        panel.SetActive(false);

    }

    public void CreateCharacter(string name, Vector3 origin)
    {
        GameObject player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        player.name = "Player_" + name;
        player.transform.position = origin;
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.camera = camera;
        playerController.speed = 2;
        playerController.isOnline = false;
        playerController.nakamaTesting = nakamaTesting;

        FollowToOther ffo = camera.gameObject.AddComponent<FollowToOther>();
        ffo.target = player.transform;
        ffo.modifyX = true;
        ffo.modifyZ = true;

        GameObject playerMarket = Instantiate(Resources.Load<GameObject>("Prefabs/MarkText"));
        playerMarket.name = "PlayerMark_" + name;
        playerMarket.transform.position = new Vector3(0,2,0);
        FollowToOther ffo2 = playerMarket.GetComponent<FollowToOther>();
        ffo2.target = player.transform;
        ffo2.modifyX = true;
        ffo2.modifyZ = true;
        ffo2.modifyY = true;

        _characters.Add(playerController);

        // nakamaTesting.updatePosition(origin, player.transform.rotation);
    }

    public void CreateCharacterWithoutController(string name, Vector3 origin, Quaternion rotation)
    {
        GameObject player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.isOnline = true;
        player.name = "Player_" + name;
        player.transform.position = origin;
        player.transform.rotation = rotation;
        playerController.speed = 2;
        playerController.nakamaTesting = nakamaTesting;
        
        GameObject playerMarket = Instantiate(Resources.Load<GameObject>("Prefabs/MarkText"));
        playerMarket.name = "PlayerMark_" + name;
        playerMarket.transform.position = new Vector3(0, 2, 0);
        FollowToOther ffo2 = playerMarket.GetComponent<FollowToOther>();
        ffo2.target = player.transform;
        ffo2.modifyX = true;
        ffo2.modifyZ = true;
        ffo2.modifyY = true;

        _characters.Add(playerController);
    }

    public void updateCharacterPosition(NakamaCommand state)
    {
        Vector3 position = state.position;
        Quaternion rotation = state.rotation;
        PlayerController g = _characters.Find(l => l.gameObject.name == "Player_" + state.sessionId);
        if (g != null)
        {
            //g.h = state.h;
            //g.v = state.v;
            //g.movPlayer = position;
            //g.move = true;
            g.transform.position = position;
            g.transform.rotation = rotation;
        }
        else
        {
            CreateCharacterWithoutController(state.sessionId,position, rotation);
        }
    }
}
