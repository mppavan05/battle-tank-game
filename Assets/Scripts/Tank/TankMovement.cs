using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;
    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdLing;
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.2f;

    private string m_MovementAxisName;
    private string m_TurnAxisName;
    private Rigidbody m_Rigidbody;
    private float m_MovementInputValue;
    private float m_TurnInputValue;
    private float m_OrginalPitch;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }

    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }

    private void Start()
    {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        m_OrginalPitch = m_MovementAudio.pitch;
    }

    private void Update()
    {
        // store the player input and make sure the audio for the engine is playing.
        m_MovementInputValue = Input.GetAxis (m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);

        EngineAudio();
    }

    private void EngineAudio()
    {
        // play the correct audio clip based on weather or not the tank is moving and what audio id current in use.

        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
        {
            if(m_MovementAudio.clip == m_EngineDriving)
            {
                m_MovementAudio.clip = m_EngineIdLing;
                m_MovementAudio.pitch = Random.Range(m_OrginalPitch - m_PitchRange, m_OrginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            if (m_MovementAudio.clip == m_EngineIdLing)
            {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OrginalPitch - m_PitchRange, m_OrginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }

        }
    }

    private void FixedUpdate()
    {
        // move and turn the tank.
        Move();
        Turn();
    }

    private void Move()
    {
        // Adjust the position of the tank based on thr player's input.

        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }

    private void Turn()
    {
        // adjust the rotation of th tank based on the player's input.

        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        // way of unity to storing rotation movement
        
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);

    }


}