using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandPanelController : MonoBehaviour
{
    public GameObject container;
    public GameObject textCommandTemplate;
    
    public void WriteCommand(string command)
    {
        GameObject newTextCommand = Instantiate<GameObject>(textCommandTemplate);
        newTextCommand.transform.parent = container.transform;

        newTextCommand.GetComponent<Text>().text = $"_> {command}.";
    }
}

