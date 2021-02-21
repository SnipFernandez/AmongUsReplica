using System;
using UnityEngine;

[Serializable]
public class NakamaCommand
{
    public string command;
    public Vector3 position;
    public string sessionId;
    public Quaternion rotation;
    public float h;
    public float v;
    public float speed;
    public string skin;
    public bool isALive;
}