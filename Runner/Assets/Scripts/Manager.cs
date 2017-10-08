using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    [SerializeField] private GameObject m_canvas;
    [SerializeField] private GameObject m_finishLine;

    public int m_playerShopCount = 0;
    public int m_playerPlacementCount = 0;
    [HideInInspector] public bool m_activateItemPlacement = false;

    private bool m_stopPlayerShop = false;
    public bool m_activatePlayerInput = false;

    private FinishLine m_finishLineScript;

    private void Awake()
    {
        m_finishLineScript = m_finishLine.GetComponent<FinishLine>();
    }

    // Update is called once per frame
    void Update ()
    {
	    if(m_playerShopCount >= 4)
        {
            m_activateItemPlacement = true;
            m_canvas.SetActive(false);

            m_stopPlayerShop = true;
            m_playerShopCount = 0;

        }

        if (m_stopPlayerShop)
        {
            foreach (GameObject i in m_finishLineScript.m_players)
            {
                i.GetComponent<PlayerInput>().m_isShop = false;
            }
            m_stopPlayerShop = false;
        }
        
        if(m_playerPlacementCount >= 4)
        {
            m_activatePlayerInput = true;
            m_playerPlacementCount = 0;
        }

        if (m_activatePlayerInput)
        {
            foreach (GameObject i in m_finishLineScript.m_players)
            {
                i.GetComponent<PlayerInput>().m_isInput = true;
                i.GetComponent<Player>().m_stopMove = false;
            }
            m_finishLineScript.m_players.Clear();
            m_activatePlayerInput = false;
        }

    }
}
