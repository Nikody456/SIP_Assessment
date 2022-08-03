using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed;
    [SerializeField] private float _dragForce;
    [SerializeField] private Transform _orientation;
    private float _horizontalInput;
    private float _verticalInput;
    private Vector3 _moveDirection;

    [Header("Ground Check")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _ground;
    private bool _isGrounded;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    private void Update()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 1.5f, _ground);

        GetInput();

        Debug.Log(_speed);

        if (_isGrounded)
            _rb.drag = _dragForce;
        else
            _rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void GetInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        _moveDirection = _orientation.forward * _verticalInput + _orientation.right * _horizontalInput;

        _rb.AddForce(_moveDirection.normalized * _speed * 10.0f, ForceMode.Force);
    }

    private void LimitSpeed()
    {
        Vector3 flatVelocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);

        if (flatVelocity.magnitude > _speed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * _speed;
            _rb.velocity = new Vector3(limitedVelocity.x, _rb.velocity.y, limitedVelocity.z);
        }
    }
}
