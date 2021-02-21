using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinijuegoGasolinaController : MonoBehaviour
{
    public Image liquido;
    public bool isPress;

    // Start is called before the first frame update
    void Start()
    {
        liquido.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPress)
        {
            liquido.fillAmount = Mathf.SmoothStep(liquido.fillAmount, 1, 5f * Time.deltaTime);
        }
    }

    public void OnButtonDown(BaseEventData baseEventData)
    {
        isPress = true;
    }
    public void OnButtonUp(BaseEventData baseEventData)
    {
        isPress = false;
    }
}
