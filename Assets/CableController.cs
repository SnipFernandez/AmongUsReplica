using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CableController : MonoBehaviour
{
    public Vector3 target;
    public GameObject giro;
    public GameObject line;
    public Image indicador;
    public BoxCollider2D collider2D;

    public bool isDragging;
    public bool isConnected;
    public bool refresh;
    // public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        refresh = false;
        isConnected = false;
        isDragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            refresh = true;
            if (target == null)
            {
                target = giro.transform.position;
            }
            //giro.transform.right = target - giro.transform.position;

            //giro.transform.LookAt(target);

            Vector3 dir = target - giro.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            giro.transform.rotation = Quaternion.AngleAxis(Mathf.Clamp(angle, -40, 50), Vector3.forward);

            Vector2.Distance(target, giro.transform.position);
            ((RectTransform)line.transform).sizeDelta = new Vector2(Vector2.Distance(target, giro.transform.position), 50);
            //Debug.Log(giro.transform.position);
            //Debug.Log(target);
            //Debug.Log($"------1: {Vector2.Angle(giro.transform.position, target)}");
            //Vector2 v = giro.transform.position;
            //Debug.Log($"------2: {Vector2.Angle(giro.transform.position, v)}");
            //giro.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(giro.transform.position, target));
            // giro.transform.Rotate(giro.transform.position, Vector2.Angle(giro.transform.position, target.transform.position));
        }
        else
        {
            if (refresh)
            {
                refresh = false;
                if (isConnected)
                {
                    indicador.color = Color.green;
                    collider2D.enabled = false;
                }
                else
                {
                    target = giro.transform.position;
                    giro.transform.rotation = Quaternion.AngleAxis(Mathf.Clamp(0, -40, 50), Vector3.forward);
                    ((RectTransform)line.transform).sizeDelta = new Vector2(50, 50);
                }

            }
        }
    }

    public void beginDrag(Vector3 vector)
    {
        isDragging = true;
        target = vector;
    }

    public void endDrag()
    {
        isDragging = false;
    }

    public void move(Vector3 vector)
    {
        target = vector;
    }
    public void connected(bool val)
    {
        isConnected = val;
    }
}
