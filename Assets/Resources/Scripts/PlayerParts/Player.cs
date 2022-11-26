
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

        _controls.Controls.Hint.performed += ctx => HintAction();

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
        Move();
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
        AudioManager.GetInstance().StopClip("step");
        _charController = null;
        _looter = null;
        _controls.Controls.Disable();
    }

    void Move()
    {
        if(_charController != null)
        {
            _charController.Move(movement, isJumping);
            isJumping = false;
        }
    }

    void UseAction()
    {
        if (_charController.IsGrounded())
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
    }

    void HintAction()
    {
        HintManager.GetInstance().SwitchHint(LevelManager.GetInstance().curLevel + 1);
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

    public void LoadNextLevel()
    {
        AudioManager.GetInstance().PlayClip("switch");
        _charController.EndAnimation();
        RemovePlayerBody();
        LevelManager lm = LevelManager.GetInstance();
        GameObject canvas = GameObject.FindGameObjectWithTag("UI");
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
        if (lm.hasNextLevel())
        {
            lm.LoadNextLevel();
        }
        else
        {
            lm.LoadEnding();
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
