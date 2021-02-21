using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevel01Controller : MonoBehaviour
{
    float NiveltareaActual = 0;
    float MaxNivelTareas = 1;
    public Image task;
    public Button btKill;
    public Image Killcontador;
    bool hacerContador = false;
    public Button btReport;
    public MiniJuegoCablesController miniJuegoCables;
    public MinijuegoGasolinaController miniJuegoGasolina;
    [SerializeField, SerializeReference]
    public Menu windowsOpen;

    public GameObject reporte;
    public Text reporteText;
    public GameObject player;
    
    private void Awake()
    {
        task.fillAmount = 0;
        Killcontador.fillAmount = 0;
        hacerContador = false;
        Killcontador.gameObject.SetActive(false);
        btReport.interactable = false;
        btKill.interactable = false;
        reporteText.text = "";
        reporte.SetActive(false);
    }     

    public void comprobarTareas()
    {
        if(miniJuegoCables?.cablesConectados.Count == 4)
        {
            aumentarTareas(.5f);
        }

        if(miniJuegoGasolina.liquido.fillAmount >= .88f)
        {
            aumentarTareas(.5f);
        }
    }

    public void aumentarTareas(float cantidad)
    {
        NiveltareaActual += cantidad;
        task.fillAmount = NiveltareaActual;
    }

    public void cambiarEstadoBTReportar(bool estado)
    {
        btReport.interactable = estado;
    }

    public void cambiarEstadoBTReportar2()
    {
        windowsOpen.show();
        player.SendMessage("moveToResPawn");
    }

    PlayerController target;
    public void cambiarEstadoBTMatar(bool estado)
    {
        Debug.Log("cambiarEstadoBTMatar");
        if (target != null)
        {
            Debug.Log("aaaaaaaaaaaaa");
            target.isALive = false;
            btKill.interactable = estado;
            if (!estado)
            {
                Killcontador.gameObject.SetActive(true);
                hacerContador = true;
            }
        }
    }

    public void cambiarEstadoBTMatar2(bool estado, PlayerController outTarget)
    {
        if (!hacerContador)
        {
            btKill.interactable = estado;
        }
        target = outTarget;
    }

    public float time;
    public float contador = 0;
    private void Update()
    {
        if (hacerContador)
        {
            time += Time.deltaTime;
            if(time >= 1)
            {
                time = 0;
                contador += 1f/15f;
                Killcontador.fillAmount += 1f/15f;
            }

            if(contador >= 1)
            {
                hacerContador = false;
                time = 0;
                contador = 0;
                Killcontador.gameObject.SetActive(false);
                Killcontador.fillAmount = 0;
                cambiarEstadoBTMatar(true);
                if (target != null)
                {
                    btKill.interactable = true;
                }
                else {
                    btKill.interactable = false;
                }
            }
        }
    }

    public void mostrarNotificacion(string texto) {
        reporteText.text = texto;
        reporte.SetActive(true);
    }    

    internal void mostrarPantalla(Menu openWindow)
    {
        openWindow?.show();
        if (openWindow is UIScreenVoteController)
        {
            Debug.Log("bbbbbbbb");
            player.SendMessage("moveToResPawn");
        }
    }
}
