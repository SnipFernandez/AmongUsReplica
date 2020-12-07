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

    public GameObject reporte;
    public Text reporteText;
    
    public GameObject pantallaReporte;

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
        pantallaReporte.SetActive(false);
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

    public void cambiarEstadoBTMatar(bool estado)
    {
        btKill.interactable = estado;
        if (!estado)
        {
            Killcontador.gameObject.SetActive(true);
            hacerContador = true;
        }
    }

    float time;
    float contador = 0;
    private void Update()
    {
        if (hacerContador)
        {
            time += Time.deltaTime;
            if(time >= 1)
            {
                time = 0;
                contador += 1 / 15;
                Killcontador.fillAmount += 1/15;
            }

            if(contador >= 1)
            {
                hacerContador = false;
                Killcontador.gameObject.SetActive(false);
                Killcontador.fillAmount = 0;
                cambiarEstadoBTMatar(true);
            }
        }
    }

    public void mostrarNotificacion(string texto) {
        reporteText.text = texto;
        reporte.SetActive(true);
    }

    public void mostrarPantallaVotaciones()
    {
        pantallaReporte.SetActive(true);
    }
}
