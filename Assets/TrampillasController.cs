using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampillasController : MonoBehaviour
{
    public List<GameObject> trampillas = new List<GameObject>();
    public Camera camara;
    public Vector3 initialPosition;
    internal GameObject initialTrampilla;
    internal int index = 0;
    public GameObject ligth;
    public GameObject Player;
    public GameObject TextPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Player != null)
        {

            if (Input.GetKeyUp(KeyCode.A))
            {
                if (index + 1 < trampillas.Count)
                {
                    index++;
                }
                else
                {
                    index = 0;
                }
                camara.transform.position = new Vector3(trampillas[index].transform.position.x, camara.transform.position.y, trampillas[index].transform.position.z);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                if (index - 1 > 0)
                {
                    index--;
                }
                else
                {
                    index = trampillas.Count - 1;
                }
                camara.transform.position = new Vector3(trampillas[index].transform.position.x, camara.transform.position.y, trampillas[index].transform.position.z);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                TextPlayer.SetActive(true);
                Player.transform.position = new Vector3(trampillas[index].transform.position.x, Player.transform.position.y, trampillas[index].transform.position.z);
                camara.transform.rotation = Quaternion.Euler(52, 180, 0);
                camara.gameObject.GetComponent<FollowToOther>().modifyX = true;
                camara.gameObject.GetComponent<FollowToOther>().modifyY = true;
                camara.gameObject.GetComponent<FollowToOther>().modifyZ = true;
                ligth.SetActive(false);
                Player.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        TextPlayer.SetActive(false);
        ligth.SetActive(true);
        camara.transform.position = new Vector3(initialPosition.x, camara.transform.position.y, initialPosition.z);
        camara.transform.rotation = Quaternion.Euler(90, 180, 0);
        index = trampillas.IndexOf(initialTrampilla);
    }
}
