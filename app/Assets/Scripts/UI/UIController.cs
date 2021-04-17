/*
Code By: Aaron Lander
This script controls all assets on the UI, including loading 
or changing any image or animation.
*/

using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public PhotonView photonView;

    [SerializeField] private Image m_UIHealth;
    [SerializeField] private Sprite[] m_HealthImages;
    [SerializeField] private CharacterHealth m_CharacterHealth;
    private int m_healthIndex;

    // Start is called before the first frame update
    void Start()
    {
        if(!photonView.IsMine) 
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CharacterHealth)
        {
            int health = m_CharacterHealth.GetCharacterHealth();

            if (health != m_healthIndex)
            {
                // Characer health has changed, update UI
                m_healthIndex = health;
                UpdateHealthImage();
            }
        }
    }

    public void UpdateHealthImage()
    {
        m_UIHealth.sprite = m_HealthImages[m_healthIndex - 1];
    }
}
