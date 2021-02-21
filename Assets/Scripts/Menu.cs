using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour, IMenu
{
    public UILevel01Controller ui;

    public void show()
    {
        gameObject.SetActive(true);
    }

    public void close()
    {
        ui?.SendMessage("comprobarTareas");
        gameObject.SetActive(false);
    }
}
