using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoping : MonoBehaviour {

    private Vector2 directionalInput;
    private Vector3 velocity;

    private float moveSpeed = 5.0f;

    public GameObject m_aimSprite;
    public bool m_activateAimSprite = false;

    private PlayerResource m_playerResourceScript;


    private void Awake()
    {
        m_playerResourceScript = GetComponent<PlayerResource>();
    }

    // Update is called once per frame
    void Update ()
    {
        velocity.x = directionalInput.x * moveSpeed;
        velocity.y = directionalInput.y * moveSpeed;

        Move(velocity * Time.deltaTime);

        if(m_activateAimSprite)
        {
            m_aimSprite.SetActive(true);
        }
	}

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    private void Move(Vector2 moveAmount)
    {
        m_aimSprite.transform.Translate(moveAmount);
    }

    public void SelectItem()
    {

        RaycastHit2D hit = Physics2D.Raycast(m_aimSprite.transform.position, Vector2.zero);
        if(hit.collider !=null && hit.collider.tag == "Item" && m_playerResourceScript.m_playerCurrency >= hit.collider.GetComponent<ShopItems>().m_itemCost)
        {
            m_playerResourceScript.m_playerCurrency -= hit.collider.GetComponent<ShopItems>().m_itemCost;
        }
    }


}
