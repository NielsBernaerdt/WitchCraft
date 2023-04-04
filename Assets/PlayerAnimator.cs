using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private int _currentState;
    private float _prevX;

    [SerializeField] private BasePawn _player;
    private Animator _anim;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _renderer = GetComponentInChildren<SpriteRenderer>();

        _prevX = _player.transform.position.x;
    }
    private void FixedUpdate()
    {
        //direction of sprite
        float _currentX = _player.transform.position.x;
        if(_currentX < _prevX)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        _prevX = _currentX;
    }
    private void Update()
    {
        //state of sprite
        var state = _player.GetCurrentAnimationState();

        if (state == _currentState)
            return;
        _anim.CrossFade(state, 0, 0);
        _currentState = state;
    }
}