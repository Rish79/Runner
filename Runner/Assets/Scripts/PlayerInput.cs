
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


[RequireComponent (typeof(Player))]
public class PlayerInput : MonoBehaviour 
{

     public bool m_isInput = true;
    [HideInInspector] public bool m_isShop = false;
     public bool m_isPlacing = false;

    [SerializeField] private GameObject m_manager;

    public int playerId;
    private Player player;
    private PlayerShoping m_playerShopScript;
    private PlayerController playerController;
    private Manager m_managerScript;
    private float distance = 0.5f;

    private Animator m_playerAnim;


    void Start () 
	{
        player = GetComponent<Player>();
        m_playerShopScript = GetComponent<PlayerShoping>();
        m_managerScript = m_manager.GetComponent<Manager>();
        m_playerAnim = gameObject.GetComponent<Animator>();

    }

    void Update () 
	{


        if (m_isInput)
        {
            Vector2 directionalInput = new Vector2(Input.GetAxisRaw("MoveHorizontal" + playerId), Input.GetAxisRaw("MoveVertical" + playerId));

            if(directionalInput.x > 0.1f || directionalInput.x < -0.1f)
            {
                m_playerAnim.SetBool("isWalking", true);
            }
            else
            {
                m_playerAnim.SetBool("isWalking", false);

            }
            player.SetDirectionalInput(directionalInput);

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump" + playerId))
            {
                player.OnJumpInputDown();
            }

            if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Jump" + playerId))
            {
                player.OnJumpInputUp();
            }
        }
        else if(m_isShop)
        {
            Vector2 directionalInput = new Vector2(Input.GetAxisRaw("MoveHorizontal" + playerId), Input.GetAxisRaw("MoveVertical" + playerId));
            m_playerShopScript.SetDirectionalInput(directionalInput);

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump" + playerId))
            {
                m_playerShopScript.SelectItem();
            }
        }
        else if(m_isPlacing)
        {
            Vector2 directionalInput = new Vector2(Input.GetAxisRaw("MoveHorizontal" + playerId), Input.GetAxisRaw("MoveVertical" + playerId));
            m_playerShopScript.SetDirectionalInput(directionalInput);

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump" + playerId))
            {
                m_playerShopScript.PlaceItem();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position,transform.position + Vector3.right * transform.localScale.x * distance);
    }

    public void SetNoMovment()
    {
        if(Input.GetAxisRaw("MoveHorizontal" + playerId) != 0)
        {
            float horz = Input.GetAxisRaw("MoveHorizontal" + playerId);
            horz = 0f;
        }
        if (Input.GetAxisRaw("MoveVertical" + playerId) != 0)
        {
            float vert = Input.GetAxisRaw("MoveVertical" + playerId);
            vert = 0f;
        }
    }
}
