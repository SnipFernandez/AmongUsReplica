using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiniJuegoCablesController : MonoBehaviour
{
    bool isDrag = false;
    bool isDragSendMessage = true;
    bool isEndDragSendMessage = true;

    public List<CableController> cables = new List<CableController>();
    public List<GameObject> cablesConectados = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log($"Mouse position: " + Input.mousePosition);
        if (target != null && isDrag)
        {
            if (isDrag && !isDragSendMessage)
            {
                isDragSendMessage = true;
                target.SendMessage("beginDrag", InitialPosition);
            }
            // Debug.Log($"Input.mousePosition--> " + Input.mousePosition);
            target.SendMessage("move", Input.mousePosition);
        }
        else
        {
            if (!isDrag && !isEndDragSendMessage)
            {
                isDragSendMessage = true;
                target?.SendMessage("endDrag");
                if (cables.Find(c => c.gameObject == target && c.isConnected)) {
                    if (!cablesConectados.Contains(target))
                    {
                        cablesConectados.Add(target);
                    }
                }
                target = null;
            }
        }
    }
    GameObject target;
    Vector3 InitialPosition;
    public void OnBeginDrag(BaseEventData baseEventData)
    {
        target = ((PointerEventData)baseEventData).pointerDrag;
        if (target != null && !target.GetComponent<CableController>().isConnected)
        {            
            InitialPosition = ((PointerEventData)baseEventData).position;
            isDrag = true;
            isDragSendMessage = false;
            isEndDragSendMessage = false;
        }
        else
        {
            target = null;
        }

        // Debug.Log(InitialPosition);
    }

    public void OnEndDrag(BaseEventData baseEventData)
    {
        if(((PointerEventData)baseEventData).pointerDrag == target)
        {
            isDragSendMessage = false;
            isEndDragSendMessage = false;
            isDrag = false;
            InitialPosition = Vector3.zero;
        }
    }
}
