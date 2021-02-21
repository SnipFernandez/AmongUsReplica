using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TripulanteOrImpostor : MonoBehaviour
{
    public GameObject target;
    public Text texto;
    public Image image;
    public UnityEngine.UI.Outline outline;
    bool refresh = true;

    private void Awake()
    {
        if(target != null)
        {
            refresh = false;
            if (target.tag.Equals("Impostor"))
            {
                texto.text = target.tag;
                texto.color = Color.white;
                image.color = Color.red;
                outline.effectColor = Color.white;

            }
            else
            {
                texto.text = "Tripulante";
                texto.color = Color.HSVToRGB(.2f, .2f, .2f);
                image.color = Color.white;
                outline.effectColor = Color.HSVToRGB(.2f, .2f, .2f);
            }
        }
    }

    private void Update()
    {
        if(refresh && target != null)
        {
            refresh = false;
            if (target.tag.Equals("Impostor"))
            {
                texto.text = target.tag;
                texto.color = Color.white;
                image.color = Color.red;
                outline.effectColor = Color.white;

            }
            else
            {
                texto.text = "Tripulante";
                texto.color = Color.HSVToRGB(.2f, .2f, .2f);
                image.color = Color.white;
                outline.effectColor = Color.HSVToRGB(.2f, .2f, .2f);
            }
        }
    }

}
