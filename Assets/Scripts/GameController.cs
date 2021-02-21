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
        GameObject camera;
        switch (arg0.name)
        {
            case "Lobby":
                GameObject gCommandPanel = GameObject.Find("CommandPanel");
                GameObject gCopyIdGame = GameObject.Find("CopyIDGame");
                GameObject gStartButton = GameObject.Find("StartButton");
                camera = GameObject.Find("Main Camera");
                amongUsLobby = GameObject.Find("amongUsLobby");
                List<Transform> positionsAux = new List<Transform>();
                GameObject.Find("PointsRespawn").gameObject.GetComponentsInChildren<Transform>(positionsAux);
                positionsAux.RemoveAt(0);
                positions = positionsAux.ToArray();
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

                if (gStartButton != null)
                {
                    Button bStartButton = gStartButton.GetComponent<Button>();
                    if (bStartButton != null)
                    {
                        bStartButton.onClick.AddListener(StartMatch);
                    }
                }

                if (UI != null)
                {
                    UI.SendMessage("incrementPlayer");
                }
                createCharacter(camera);
                break;
            case "Level01":
                _characters.Clear();
                Debug.Log("Level01");
                camera = GameObject.Find("Main Camera");
                GameObject PointsResPawn = GameObject.Find("PointsResPawn");
                createCharacter(camera, PointsResPawn);
                break;
        }
    }

    // float[] positions = new float[] { 2.74f, 1.89f, 1.22f, -1.38f, -2f, -2.72f };
    public Transform[] positions;
    string[] skins = new string[] { "Materials/MACharacterRed",
        "Materials/MACharacterLigthBlue",
        "Materials/MACharacterPink",
        "Materials/MACharacterGreen",
        "Materials/MACharacterBlack"
    };

    PlayerData playerData; 

    private void createCharacter(GameObject camera, GameObject grespawn = null)
    {
        if (playerData == null)
        {
            playerData = new PlayerData();
            playerData.skin = skins[UnityEngine.Random.Range(0, skins.Length - 1)];
        }

        GameObject player = Instantiate(characterTemplate);
        GameObject playerIndicator = Instantiate(characterTextIndicatorTemplate);
        player.name = "Player_" + nakama.GetSessionId();

        float posX = 0;
        PlayerController playerController = player.GetComponent<PlayerController>();

        if (grespawn == null)
        {
            posX = positions[UnityEngine.Random.Range(0, positions.Length - 1)].position.x;
            player.transform.position = new Vector3(posX, 1.20f, -5f);
            playerIndicator.transform.position = new Vector3(posX, 2.3f, -5f);
        }
        else
        {
            Transform respawn = grespawn.transform.GetChild(UnityEngine.Random.Range(0, grespawn.transform.childCount - 1));
            posX = respawn.position.x;
            player.transform.position = new Vector3(respawn.position.x, 0.38f, respawn.position.z);
            playerIndicator.transform.position = new Vector3(respawn.position.x, 1.5f, respawn.position.z);
            camera.transform.position = new Vector3(respawn.position.x, 5.37f, respawn.position.z+3);

            // ESto es una mierda pero no se me ocurre por ahora nada, quiero terminarlo ya....
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            GameObject gControllerTrampillas = null;
            foreach (GameObject go in allObjects)
            {
                if (go.name.Equals("ControllerTrampillas"))
                {
                    gControllerTrampillas = go;
                    break;
                }
            }

            TrampillasController tp2 = gControllerTrampillas.GetComponent<TrampillasController>();
            tp2.Player = player;
            tp2.TextPlayer = playerIndicator;
            playerController.trampillasController = tp2;

            player.tag = (UnityEngine.Random.Range(0,2) == 0 ) ? "CharacterPlayer" : "Impostor";

            FollowToOther follow2 = camera.GetComponent<FollowToOther>();
            follow2.target = player.transform;
            follow2.modifyX = true;
            follow2.modifyY = true;
            follow2.modifyZ = true;
            playerController.respawn = respawn;

            TripulanteOrImpostor tripulanteOrImpostor = playerIndicator.GetComponent<TripulanteOrImpostor>();
            tripulanteOrImpostor.target = player;
        }        
        playerIndicator.transform.rotation = Quaternion.Euler(0, -180, 0);
        //player.transform.parent = amongUsLobby.transform;

        playerController.camera = camera.GetComponent<Camera>();
        playerController.nakamaTesting = nakama;

        playerController.skin = playerData.skin;
        Material[] mats = playerController.Mesh.materials;
        mats[0] = Resources.Load<Material>(playerData.skin);
        playerController.Mesh.materials = mats;
        playerController.MeshDeadth.materials = mats;

        FollowToOther follow = playerIndicator.GetComponent<FollowToOther>();
        follow.target = player.transform;
        follow.modifyX = true;
        follow.modifyY = true;
        follow.modifyZ = true;

        playerController.nakamaTesting.updatePosition(playerController.transform.position, 
            playerController.transform.rotation,
            playerController.character.velocity.magnitude,
            playerData.skin);
    }

    public void CreateCharacterWithoutController(string name, Vector3 origin, Quaternion rotation, string skin)
    {
        GameObject player = Instantiate(characterTemplate);
        GameObject playerIndicator = Instantiate(characterTextIndicatorTemplate);

        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.isOnline = true;
        player.name = "Player_" + name;
        player.tag = "Character";
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
        playerController.MeshDeadth.materials = mats;

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
    public void updateCharacterState(NakamaCommand state)
    {
        PlayerController g = _characters.Find(l => l.gameObject.name == "Player_" + state.sessionId);
        if (g != null)
        {
            g.isALive = state.isALive;
        }
    }


    public async void StartGame()
    {
        await nakama.createMatch();
        initialCommand = $"<b>ID PARTIDA</b> {nakama.getMatchID()}";
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Lobby");
    }

    public async void StartMatch()
    {
        await nakama.startMatch();
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Level01");
    }

    public void StartMatchWithoutNakama()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Level01");
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
