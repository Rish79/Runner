using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoping : MonoBehaviour {

    private Vector2 directionalInput;
    private Vector3 velocity;
    private Vector2 m_itemPlacementStartPos;

    private float moveSpeed = 5.0f;

    public GameObject m_boughtItem;
    public GameObject m_itemClone;

    [SerializeField] private GameObject m_aimSprite;
    [SerializeField] private GameObject m_manager;

     public bool m_activateAimSprite = false;

    private bool m_noClone = true;

    private PlayerResource m_playerResourceScript;
    private PlayerInput m_playerInputScript;
    private Manager m_managerScript;


    private void Awake()
    {
        m_playerResourceScript = GetComponent<PlayerResource>();
        m_playerInputScript = GetComponent<PlayerInput>();
        m_managerScript = m_manager.GetComponent<Manager>();
        m_itemPlacementStartPos = new Vector2(transform.position.x + 3f, transform.position.y);
    }

    // Update is called once per frame
    void Update ()
    {
        velocity.x = directionalInput.x * moveSpeed;
        velocity.y = directionalInput.y * moveSpeed;

        if(m_activateAimSprite)
        {
            MoveAim(velocity * Time.deltaTime);
            m_aimSprite.SetActive(true);
        } else
        {
            m_aimSprite.SetActive(false);
        }
        if(m_playerInputScript.m_isPlacing)
        {
            if(m_noClone)
            {
                m_itemClone = Instantiate(m_boughtItem, m_itemPlacementStartPos, m_boughtItem.transform.rotation);
                m_noClone = false;
            }
            MoveBoughtItem(velocity * Time.deltaTime);
        }

    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    private void MoveAim(Vector2 moveAmount)
    {
        m_aimSprite.transform.Translate(moveAmount);
    }
    private void MoveBoughtItem(Vector2 moveAmount)
    {
        m_itemClone.transform.Translate(moveAmount);
    }

    public void SelectItem()
    {

        RaycastHit2D hit = Physics2D.Raycast(m_aimSprite.transform.position, Vector2.zero);
        if(hit.collider !=null && hit.collider.tag == "Item" && m_playerResourceScript.m_playerCurrency >= hit.collider.GetComponent<ShopItems>().m_itemCost && m_boughtItem == null)
        {
            m_boughtItem = hit.collider.GetComponent<ShopItems>().m_ItemPrefab;
            m_playerResourceScript.m_playerCurrency -= hit.collider.GetComponent<ShopItems>().m_itemCost;
            m_managerScript.m_playerShopCount += 1;
            m_activateAimSprite = false;

        }
    }

    public void PlaceItem()
    {
        if (m_itemClone.transform.position.x > -5.5f)
        {
            m_itemClone = null;
            m_boughtItem = null;
            m_noClone = true;
            m_managerScript.m_playerPlacementCount += 1;
            m_playerInputScript.m_isPlacing = false;
        }
    }


}
