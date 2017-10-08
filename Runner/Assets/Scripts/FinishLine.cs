using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour {

    [SerializeField] private float m_initCurrencyAmount = 25f;
    [SerializeField] private float m_currencyAddition = 25f;
    private float m_tempCurrencyAmount;

    [SerializeField] private float m_initPointsAmount = 100f;
    [SerializeField] private float m_pointSubtraction = 25f;
    private float m_tempPointAmount;

    private int m_playerFinishCount = 0;

    public List<GameObject> m_players;

    private bool m_roundEnded = false;

    PlayerResource m_playerResScript;
    PlayerInput m_playerInputScript;


    [SerializeField] private GameObject m_canvas;

    private void Awake()
    {
        m_tempCurrencyAmount = m_initCurrencyAmount;
        m_tempPointAmount = m_initPointsAmount;

        m_players = new List<GameObject>();

    }

    // Update is called once per frame
    void Update () {
        
		
	}

    void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.tag == "Player")
        {
            m_playerResScript = collision.gameObject.GetComponent<PlayerResource>();
            m_playerInputScript = collision.gameObject.GetComponent<PlayerInput>();

            m_players.Add(collision.gameObject);

            m_playerInputScript.SetNoMovment();
            collision.gameObject.GetComponent<Player>().m_stopMove = true;

            m_playerInputScript.m_isInput = false;
            m_playerFinishCount += 1;

            m_playerResScript.m_playerCurrency += m_tempCurrencyAmount;
            m_playerResScript.m_playerPoints += m_tempPointAmount;
            m_tempCurrencyAmount += m_currencyAddition;
            m_tempPointAmount -= m_pointSubtraction;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (m_playerFinishCount >= 4)
        {
            m_roundEnded = true;

            if (m_roundEnded)
            {
                m_tempCurrencyAmount = m_initCurrencyAmount;
                m_tempPointAmount = m_initPointsAmount;

                foreach (GameObject i in m_players)
                {
                    i.GetComponent<Player>().m_moveToStart = true;
                }
                m_canvas.SetActive(true);
                m_roundEnded = false;
            }

            m_playerFinishCount = 0;
        }
    }
}
