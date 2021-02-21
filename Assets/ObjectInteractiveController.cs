using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractiveController : MonoBehaviour
{

    Outline outline;
    [SerializeField, SerializeReference]
    public Menu windowsOpen;
    public bool isHatch;
    public GameObject ui;
    public List<string> targets = new List<string>();

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.OutlineMode = Outline.Mode.None;
        ui.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (targets.Contains(other.tag))
        {
            outline.OutlineMode = Outline.Mode.OutlineVisible;
            other.SendMessage("NearAnInteractiveObject",new NearObject(true, windowsOpen, isHatch, gameObject));
            ui.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (targets.Contains(other.tag))
        {
            outline.OutlineMode = Outline.Mode.None;
            other.SendMessage("NearAnInteractiveObject", new NearObject(false, windowsOpen));
            ui.SetActive(false);
        }
    }
}
