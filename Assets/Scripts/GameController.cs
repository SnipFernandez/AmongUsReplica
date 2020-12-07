using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public CommandPanelController commandPanel;
    public GameObject characterTemplate;
    public GameObject characterTextIndicatorTemplate;
    public NakamaTesting nakama;
    string initialCommand = "";
    private List<PlayerController> _characters = new List<PlayerController>();
    public GameObject UI;
    public GameObject amongUsLobby;

    private void Awake()
    {
        nakama.gamecontroller = gameObject;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        DontDestroyOnLoad(this.gameObject);        
    }

    private void SceneManager_sceneLoaded(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.LoadSceneMode arg1)
    {
        Debug.Log("Escena Cargada" + arg0.name);
        switch (arg0.name)
        {
            case "Lobby":
                GameObject gCommandPanel = GameObject.Find("CommandPanel");
                GameObject gCopyIdGame = GameObject.Find("CopyIDGame");
                GameObject camera = GameObject.Find("Main Camera");
                amongUsLobby = GameObject.Find("amongUsLobby"); 
                UI = GameObject.Find("UI");

                if (gCommandPanel != null)
                {
                    commandPanel = gCommandPanel.GetComponent<CommandPanelController>();
                    if (commandPanel != null)
                    {
                        nakama.ui = commandPanel;
                        commandPanel.WriteCommand(initialCommand);
                    }
                }

                if (gCopyIdGame != null)
                {
                    Button bCopyIDGame = gCopyIdGame.GetComponent<Button>();
                    if (bCopyIDGame != null)
                    {
                        bCopyIDGame.onClick.AddListener(CopyToClipboard);
                    }
                }
                if(UI != null)
                {
                    UI.SendMessage("incrementPlayer");
                }
                createCharacter(camera);
                break;
        }
    }

    float[] positions = new float[] { 2.74f, 1.89f, 1.22f, -1.38f, -2f, -2.72f };
    string[] skins = new string[] { "Materials/MACharacterRed",
        "Materials/MACharacterLigthBlue",
        "Materials/MACharacterPink",
        "Materials/MACharacterGreen",
        "Materials/MACharacterBlack"
    };

    private void createCharacter(GameObject camera)
    {
        GameObject player = Instantiate(characterTemplate);
        GameObject playerIndicator = Instantiate(characterTextIndicatorTemplate);

        float posX = positions[UnityEngine.Random.Range(0, positions.Length-1)];

        player.transform.position = new Vector3(posX, 1.20f, -5f);
        playerIndicator.transform.position = new Vector3(posX, 2.3f, -5f);
        playerIndicator.transform.rotation = Quaternion.Euler(0, -180, 0);

        //player.transform.parent = amongUsLobby.transform;

        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.camera = camera.GetComponent<Camera>();
        playerController.nakamaTesting = nakama;

        string skin = skins[UnityEngine.Random.Range(0, skins.Length-1)];

        playerController.skin = skin;
        Material[] mats = playerController.Mesh.materials;
        mats[0] = Resources.Load<Material>(skin);
        playerController.Mesh.materials = mats;

        FollowToOther follow = playerIndicator.GetComponent<FollowToOther>();
        follow.target = player.transform;
        follow.modifyX = true;
        follow.modifyY = true;
        follow.modifyZ = true;

        playerController.nakamaTesting.updatePosition(playerController.transform.position, 
            playerController.transform.rotation,
            playerController.character.velocity.magnitude,
            skin);
    }

    public void CreateCharacterWithoutController(string name, Vector3 origin, Quaternion rotation, string skin)
    {
        GameObject player = Instantiate(characterTemplate);
        GameObject playerIndicator = Instantiate(characterTextIndicatorTemplate);

        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.isOnline = true;
        player.name = "Player_" + name;
        player.transform.position = origin;
        player.transform.rotation = rotation;
        playerController.speed = 2;
        playerController.nakamaTesting = nakama;

        playerIndicator.transform.position = new Vector3(origin.x, 2.3f, origin.z);
        playerIndicator.transform.rotation = Quaternion.Euler(0, -180, 0);
        
        //player.transform.parent = amongUsLobby.transform;

        Material[] mats = playerController.Mesh.materials;
        mats[0] = Resources.Load<Material>(skin);
        playerController.Mesh.materials = mats;

        FollowToOther follow = playerIndicator.GetComponent<FollowToOther>();
        follow.target = player.transform;
        follow.modifyX = true;
        follow.modifyY = true;
        follow.modifyZ = true;

        _characters.Add(playerController);

        if (UI != null)
        {
            UI.SendMessage("incrementPlayer");
        }
    }


    public void updateCharacterPosition(NakamaCommand state)
    {
        Vector3 position = state.position;
        Quaternion rotation = state.rotation;
        PlayerController g = _characters.Find(l => l.gameObject.name == "Player_" + state.sessionId);
        if (g != null)
        {
            g.transform.position = position;
            g.transform.rotation = rotation;
            g.animator.SetFloat("speed", state.speed);
        }
        else
        {
            CreateCharacterWithoutController(state.sessionId, position, rotation, state.skin);
        }
    }


    public async void StartGame()
    {
        await nakama.createMatch();
        initialCommand = $"<b>ID PARTIDA</b> {nakama.getMatchID()}";
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Lobby");
    }

    public async void JoinGame(InputField idMatch)
    {
        // Debug.Log($"[{idMatch.text}]");
        if (!string.IsNullOrEmpty(idMatch.text))
        {
            await nakama.joinMatch(idMatch.text);
            initialCommand = $"<b>ID PARTIDA</b> {nakama.getMatchID()}";
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Lobby");
        }
    }

    public void CopyToClipboard()
    {
        GUIUtility.systemCopyBuffer = nakama.getMatchID();
    }
}
