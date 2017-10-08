using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed_SlowScript : MonoBehaviour {

    [SerializeField] private bool m_isSpeedItem = false;
    [SerializeField] private bool m_isSlowItem = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (m_isSpeedItem && collision.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().m_speed = true;
        }

        if (m_isSlowItem && collision.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().m_slow = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().m_speed = false;
            collision.gameObject.GetComponent<Player>().m_slow = false;
        }
    }
}
