using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Bill Zhao
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _playerTran;
    [SerializeField]
    private Camera _cam;
    private bool _startRotate;
    [SerializeField]
    private float _camYOffset;
    [SerializeField]
    private float _camXOffset;
    [SerializeField]
    private float _camRotateSpeed;
    private Vector3 _startPos;
    private Vector3 _targetPos;
    private float _startTime;
    [SerializeField]
    private float _journeyTime;



    private void Update()
    {
        
        _cam.transform.LookAt(_playerTran);
        if ((!CameraSeesPlayer() || Vector3.Distance(_playerTran.position,_cam.transform.position) > 25) && !_startRotate)
        {
            _startRotate = true;
            _startPos = _cam.transform.position;
            _targetPos = _playerTran.position + _playerTran.up * _camYOffset - _playerTran.forward * _camXOffset;
            _startTime = Time.time;

        }

        if (_startRotate && Vector3.Distance(_cam.transform.position, _targetPos) > 0.1f)
        {
            float fracComplete = (Time.time - _startTime) / _journeyTime;
            _cam.transform.position = Vector3.Slerp(_startPos - _playerTran.position, _targetPos - _playerTran.position, fracComplete) + _playerTran.position;
        }
        else
        {
            _startRotate = false;
        }
    }

    bool CameraSeesPlayer()
    {
        RaycastHit hit;

        //Raycast from Camera to the ball and see if it finds it

        if (Physics.Raycast(_cam.transform.position, _playerTran.position - _cam.transform.position, out hit, 10000))
            if (hit.transform != _playerTran)
                return false;

        return true;
    }


}
