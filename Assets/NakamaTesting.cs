using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama;
using System;
using System.Threading.Tasks;
using UnityEngine.UI;

public class NakamaTesting : MonoBehaviour
{
    private const string PrefKeyName = "nakama.session";
    private IClient _client;
    private ISession _session;
    private ISocket _socket;
    private IMatch _match;
    private List<IUserPresence> _joinUsers = new List<IUserPresence>();
    public CommandPanelController ui;
    public bool refreshJoinList = false;
    private Queue<SendMessageObject> _queue = new Queue<SendMessageObject>();
    public GameObject gamecontroller;

    private async void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _client = new Client("http", "127.0.0.1", 7350, "defaultkey");
        var authtoken = PlayerPrefs.GetString(PrefKeyName);
        if (!string.IsNullOrEmpty(authtoken))
        {
            var session = Session.Restore(authtoken);
            if (!session.IsExpired)
            {
                _session = session;
                Debug.Log(_session);
                return;
            }
        }
        var deviceid = SystemInfo.deviceUniqueIdentifier;
        _session = await _client.AuthenticateDeviceAsync(deviceid,null,true);
        PlayerPrefs.SetString(PrefKeyName, _session.AuthToken);
        Debug.Log(_session);
    }

    public async Task<bool> createMatch()
    {
        if(_socket == null)
        {
            await createSocket();
        }

        if (_socket.IsConnected){
            _match = await _socket.CreateMatchAsync();
            return true;
            // ui.SendMessage("setMatchName", _match.Id);
        }
        else
        {
            await _socket.ConnectAsync(_session);
            createMatch();
        }
        return false;
    }

    public string getMatchID()
    {
        return _match.Id;
    }

    public async Task<bool> joinMatch(string matchId)
    {
        if (_socket == null)
        {
            await createSocket();
        }

        if (_socket != null)
        {
            if (_socket.IsConnected)
            {
                _match = await _socket.JoinMatchAsync(matchId);
                return true;
            }
            else
            {
                await _socket.ConnectAsync(_session);
                joinMatch(matchId);
            }
        }
        return false;
    }

    public async void joinMatch(InputField matchId)
    {
        joinMatch(matchId.text);
    }

    private async Task<bool> createSocket()
    {
        _socket = _client.NewSocket();

        _socket.ReceivedMatchPresence += _socket_ReceivedMatchPresence;

        _socket.ReceivedMatchState += _socket_ReceivedMatchState;
        
        _socket.Closed += _socket_Closed;

        await _socket.ConnectAsync(_session);

        return true;
    }

    private async void _socket_Closed()
    {
        if(_socket == null)
        {
            await createSocket();
        }
        else
        {
            await _socket.ConnectAsync(_session);
        }

        if (_match != null)
        {
            await joinMatch(_match.Id);
        }
    }

    public async Task<bool> startMatch()
    {
        if (_socket == null)
        {
            await createSocket();
        }

        if (_socket.IsConnected)
        {

            NakamaCommand nakamaCommand = new NakamaCommand();

            nakamaCommand.command = "start_game";

            var ns = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(nakamaCommand));

            Debug.Log(JsonUtility.ToJson(nakamaCommand));
            Debug.Log(ns);

            await _socket.SendMatchStateAsync(_match.Id, 1, ns);

            // ui.SendMessage("startGame", "Local");
            return true;
        }
        else
        {
            await _socket.ConnectAsync(_session);
            startMatch();
        }
        return false;
    }

    public async void updatePosition(float h, float v)
    {
        if (_socket == null)
        {
            await createSocket();
        }

        if (_socket.IsConnected)
        {

            NakamaCommand nakamaCommand = new NakamaCommand();

            nakamaCommand.command = "move_character";
            nakamaCommand.h = h;
            nakamaCommand.v = v;

            var ns = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(nakamaCommand));

            Debug.Log(JsonUtility.ToJson(nakamaCommand));
            Debug.Log(ns);

            await _socket.SendMatchStateAsync(_match.Id, 1, ns);
        }
        else
        {
            await _socket.ConnectAsync(_session);
            updatePosition(h,v);
        }
    }

    public async void updatePosition(Vector3 position, Quaternion rotation, float speed, string skin)
    {
        if (_socket == null)
        {
            await createSocket();
        }

        if (_socket.IsConnected)
        {

            NakamaCommand nakamaCommand = new NakamaCommand();

            nakamaCommand.command = "move_character";
            nakamaCommand.position = position;
            nakamaCommand.rotation = rotation;
            nakamaCommand.speed = speed;
            nakamaCommand.skin = skin;

             var ns = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(nakamaCommand));

            //Debug.Log(JsonUtility.ToJson(nakamaCommand));
            //Debug.Log(ns);

            await _socket.SendMatchStateAsync(_match.Id, 1, ns);
        }
        else
        {
            await _socket.ConnectAsync(_session);
            updatePosition(position, rotation, speed, skin);
        }
    }

    private void _socket_ReceivedMatchState(IMatchState obj)
    {
        var state = JsonUtility.FromJson<NakamaCommand>(System.Text.Encoding.UTF8.GetString(obj.State));

        switch (state.command)
        {
            case "start_game":
                _queue.Enqueue(new SendMessageObject()
                {
                    messajeName = "startGame",
                    messageParam = "Local"
                });
                //ui.SendMessage("startGame", obj.UserPresence.SessionId);
                break;
            case "move_character":
                state.sessionId = obj.UserPresence.SessionId;
                _queue.Enqueue(new SendMessageObject()
                {
                    messajeName = "updateCharacterPosition",
                    messageParam = state
                });
                //ui.SendMessage("updateCharacterPosition", state);
                break;
        }
    }

    List<string> commands = new List<string>();

    private void _socket_ReceivedMatchPresence(IMatchPresenceEvent presenceEvent)
    {
        foreach(IUserPresence presence in presenceEvent.Leaves)
        {
            _joinUsers.Remove(presence);
            commands.Add($"{presence.UserId} - <b><color=#ff0000ff>desconectado</color></b>");
        }

        foreach (IUserPresence presence in presenceEvent.Joins)
        {
            commands.Add($"{presence.UserId} - <b>conectado</b>");
        }

        _joinUsers.AddRange(presenceEvent.Joins);
        refreshJoinList = true;
    }

    private void Update()
    {
        if(ui != null)
        {
            if (refreshJoinList)
            {
                refreshJoinList = false;
                foreach (string command in commands)
                {
                    ui.SendMessage("WriteCommand", command);
                }
            }

        }
        if (_queue.Count > 0)
        {
            SendMessageObject messageObject = _queue.Dequeue();
            gamecontroller.SendMessage(messageObject.messajeName, messageObject.messageParam);
        }
    }
}
