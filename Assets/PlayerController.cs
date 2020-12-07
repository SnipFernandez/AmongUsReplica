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

    public Camera camera;
    public Vector3 camForward;
    public Vector3 camRight;

    public string controlNameHorizontal = "Horizontal";
    public string controlNameVertical = "Vertical";
    public string controlNameJump = "Jump";
    public NakamaTesting nakamaTesting;

    public string skin;

    //public ControladorDeUI cdUI;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOnline)
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

            Debug.Log(character.velocity.magnitude);
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

        Debug.Log("Righ: " + camera.transform.right);
        Debug.Log("forward: " + camera.transform.forward);

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }
}
