
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


[RequireComponent (typeof(Player))]
public class PlayerInput : MonoBehaviour 
{
    [HideInInspector] public bool m_isInput = true;
    [HideInInspector] public bool m_isShop = false;
    public int playerId;
    private Player player;
    private PlayerShoping m_playerShopScript;
    private PlayerController playerController;
    private float distance = 0.5f;

    void Start () 
	{
        player = GetComponent<Player>();
        m_playerShopScript = GetComponent<PlayerShoping>();
	}
	
	void Update () 
	{
        if (m_isInput)
        {
            Vector2 directionalInput = new Vector2(Input.GetAxisRaw("MoveHorizontal" + playerId), Input.GetAxisRaw("MoveVertical" + playerId));
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

            Debug.Log(Input.GetAxisRaw("MoveHorizontal" + playerId));

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump" + playerId))
            {
                m_playerShopScript.SelectItem();
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
        Debug.Log("called Func");

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
