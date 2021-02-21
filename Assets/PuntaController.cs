using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuntaController : MonoBehaviour
{

    public GameObject target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals(tag))
        {
            target?.SendMessage("connected",true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals(tag))
        {
            if (target.activeSelf)
            {
                //Debug.Log(target.activeSelf);
                //Debug.Log(gameObject.activeSelf);
                target?.SendMessage("connected", false);
            }
        }
    }
}
