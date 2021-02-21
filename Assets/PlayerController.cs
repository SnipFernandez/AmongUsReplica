using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float h;
    public float v;
    public float speedAnt;

    public CharacterController character;
    public float speed;
    public Vector3 playerInput;
    public Vector3 movPlayer;
    public Vector3 posAnt;
    public float gravity = 9.8f;
    public float fallVelocity;
    public float jumpForce;
    public bool isOnline;
    public Animator animator;
    public SkinnedMeshRenderer Mesh;
    public MeshRenderer MeshDeadth;

    public Camera camera;
    public Vector3 camForward;
    public Vector3 camRight;

    public string controlNameHorizontal = "Horizontal";
    public string controlNameVertical = "Vertical";
    public string controlNameJump = "Jump";
    public NakamaTesting nakamaTesting;

    public string skin;
    private UILevel01Controller Ui;
    private Menu openWindow;
    //public ControladorDeUI cdUI;

    public TrampillasController trampillasController;

    NearObject state;
    public PlayerController target;
    bool _isAlive = true;
    public bool isALive {
        get { return _isAlive; }
        set
        {
            if (!value)
            {
                BodyDeath?.SetActive(true);
                BodyAlive?.SetActive(false);
                enabled = false;
                _isAlive = value;
                /// Enviar el cambio online
                nakamaTesting.updateState(false, gameObject.name);
            }
        }
    }

    public GameObject BodyAlive;
    public GameObject BodyDeath;

    public Transform respawn;
    //public bool isNearAnInteractiveObject = false;

    public void  NearAnInteractiveObject(NearObject _state)
    {
        // Debug.Log($"Hola {state}");
        //isNearAnInteractiveObject = state.isNear;
        //openWindow = state.obj;
        state = _state;
    }

    void Awake()
    {
        character = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        BodyDeath?.SetActive(false);
        character = GetComponent<CharacterController>();
        GameObject uiAux =  GameObject.Find("UI");
        if(uiAux != null)
        {
            Ui = uiAux.GetComponent<UILevel01Controller>(); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOnline && !locked)
        {
            h = Input.GetAxis(controlNameHorizontal);
            v = Input.GetAxis(controlNameVertical);

            playerInput = new Vector3(h, 0, v);
            playerInput = playerInput.normalized;

            if (camera != null)
            {
                CamDirection();

                movPlayer = playerInput.x * camRight + playerInput.z * camForward;
            }
            else
            {
                movPlayer = playerInput;
            }

            movPlayer = movPlayer * speed;

            character.transform.LookAt(character.transform.position + movPlayer);

            SetGravity();

            PlayerSkill();

            character.Move(movPlayer * Time.deltaTime);

            if (!isOnline)
            {
                //if (h != 0 || v != 0)
                //{
                if (posAnt != transform.position
                    || speedAnt != character.velocity.magnitude)
                {
                    posAnt = transform.position;
                    speedAnt = character.velocity.magnitude;
                    if(nakamaTesting != null)
                    {
                        nakamaTesting.updatePosition(posAnt, transform.rotation, character.velocity.magnitude, skin);
                    }
                }
                //nakamaTesting.updatePosition(transform.position, transform.rotation);
                //}

            }
            animator.SetFloat("speed", character.velocity.normalized.magnitude);

            //Debug.Log(character.velocity.magnitude);

            if (state!= null)
            {
                if (state.isNear)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (!state.isHatch)
                        {
                            Ui.mostrarPantalla(state.obj);
                        }
                        else
                        {
                            camera.gameObject.GetComponent<FollowToOther>().modifyX = false;
                            camera.gameObject.GetComponent<FollowToOther>().modifyY = false;
                            camera.gameObject.GetComponent<FollowToOther>().modifyZ = false;
                            trampillasController.camara = camera;
                            trampillasController.initialPosition = state.hatch.transform.position;
                            trampillasController.initialTrampilla = state.hatch;
                            trampillasController.gameObject.SetActive(true);
                            trampillasController.Player = gameObject;
                            gameObject.SetActive(false);
                        }
                    }
                }

            }
        }
    }

    private void PlayerSkill()
    {
        if (character.isGrounded && Input.GetButtonDown(controlNameJump))
        {
            fallVelocity = jumpForce;
        }

        if (fallVelocity < -1)
        {
            fallVelocity = 0;
            movPlayer.y = 1;
        }

        movPlayer.y = fallVelocity;
    }

    private void SetGravity()
    {
        if (character.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;
        }

        if(fallVelocity < -1)
        {
            fallVelocity = 0;
            movPlayer.y = 1;
        }

        movPlayer.y = fallVelocity;
    }

    private void CamDirection()
    {
        camForward = camera.transform.forward;
        camRight = camera.transform.right;

        //Debug.Log("Righ: " + camera.transform.right);
        //Debug.Log("forward: " + camera.transform.forward);

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Character") && other.gameObject != gameObject && gameObject.tag.Equals("Impostor") )
        {
            target = other.GetComponent<PlayerController>();
            Debug.Log("OnTriggerEnter" + target);
            if (target.isALive)
            {
                Ui.cambiarEstadoBTMatar2(true, target);
            }
            else
            {
                Ui.cambiarEstadoBTReportar(true);
            }
        }  
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Character"))
        {
            if (target != null && target.isALive)
            {
                Ui.cambiarEstadoBTMatar2(false, null);
            }
            else
            {
                Ui.cambiarEstadoBTReportar(false);
            }
            target = null;
        }

    }
    bool locked = false;

    public void moveToResPawn()
    {
        locked = true;
        transform.position = respawn.position;
    }
    public void unlock()
    {
        locked = false;
    }
    
}
