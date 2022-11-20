
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Essentials")]
    static Player instance;
    InputControls _controls;
    Camera mainCamera;

    [Header("Plaayer Body")]
    [ShowInInspector]
    CharacterController2D _charController;
    Looter _looter;

    [Header("Inner Variables")]
    float movement;
    bool isJumping;
    bool lookAtMouse = false;


    public static Player GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        mainCamera = Camera.main;

        _controls = new InputControls();

        _controls.Controls.Move.performed += ctx => movement = ctx.ReadValue<float>();
        _controls.Controls.Move.canceled += ctx => movement = 0f;

        _controls.Controls.Jump.performed += ctx => isJumping = true;

        _controls.Controls.Use.performed += ctx => UseAction();

        _controls.Controls.Look.performed += ctx => LookAt(ctx.ReadValue<Vector2>());

        if(_charController == null)
        {
            _controls.Controls.Disable();
        }
    }

    void Start()
    {
    }

    private void Update()
    {
    }
    
    private void FixedUpdate()
    {
        if(movement != 0 || isJumping)
        {
            Move();
        }
    }

    public void GetPlayerBody()
    {
        movement = 0f;
        isJumping = false;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Length > 0)
        {
            _charController = players[0].GetComponent<CharacterController2D>();
            _looter = players[0].GetComponent<Looter>();
            _controls.Controls.Enable();
        }
    }

    private void RemovePlayerBody()
    {
        _charController = null;
        _looter = null;
        _controls.Controls.Disable();
    }

    void Move()
    {
        if(_charController != null)
        {
            _charController.Move(movement, false, isJumping);
            isJumping = false;
        }
    }

    void UseAction()
    {
        if (_looter.isAtSwitch)
        {
            LoadNextLevel();
        }
        else
        {
            _looter.LootOrDrop();
        }
    }

    void LookAt(Vector2 poi)
    {
        if (lookAtMouse)
        {
            if (_looter.isHolding)
            {
                Vector2 screenPoi = mainCamera.ScreenToWorldPoint(poi);
                _looter.RotateLootTo(screenPoi);
            }
        }
    }

    void LoadNextLevel()
    {
        RemovePlayerBody();
        LevelManager lm = LevelManager.GetInstance();
        if (lm.hasNextLevel())
        {
            lm.LoadNextLevel();
        }
        else
        {
            lm.LoadEning();
        }
    }

    private void OnEnable()
    {
        _controls.Controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Controls.Disable();
    }
}
