using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResource : MonoBehaviour
{
    public float m_playerCurrency;
    public float m_playerPoints;
	
    [SerializeField] private int m_playerNumber;

    [SerializeField] private Text m_playerCurrencyText;
    [SerializeField] private Text m_playerPointsText;

    private void Update()
    {
        m_playerCurrencyText.text = "Player " + m_playerNumber.ToString() + " " + "Coins: " + m_playerCurrency.ToString();
        m_playerPointsText.text = "Player " + m_playerNumber.ToString() + " " + "Score: " + m_playerPoints.ToString() + "/500";
    }

}
