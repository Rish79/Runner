
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


[RequireComponent (typeof(Player))]
public class PlayerInput : MonoBehaviour 
{
    public int playerId;
    private Player player;
    private PlayerController playerController;
    private float distance = 0.5f;

    void Start () 
	{
        player = GetComponent<Player>();
	}
	
	void Update () 
	{
		Vector2 directionalInput = new Vector2(Input.GetAxisRaw("MoveHorizontal" + playerId), Input.GetAxisRaw("MoveVertical" + playerId));
        player.SetDirectionalInput(directionalInput);

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump" + playerId))
        {
            player.OnJumpInputDown();
        }

        if(Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Jump" + playerId))
        {
            player.OnJumpInputUp();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position,transform.position + Vector3.right * transform.localScale.x * distance);
    }
}
