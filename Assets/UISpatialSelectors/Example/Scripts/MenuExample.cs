using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuExample : MonoBehaviour
{
    private Transform _headpose;
    private float _currentDistance;
    private Vector3 _targetPosition;

    private const float Threshold = 0.3f;
    private const float LerpSpeed = 1f;
    private readonly Vector3 Offset = new Vector3(0f, 0f, 0.7f);

    private void Start()
    {
        _headpose = Camera.main.transform;
        transform.position = _headpose.transform.position + _headpose.TransformDirection(Offset);
        _targetPosition = transform.position;
    }

    private void Update()
    {
        //billboard towards headpose
        Vector3 lookDir = (transform.position - _headpose.transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
    }

    private void LateUpdate()
    {
        //stay within range of headpose
        Vector3 offsetPosition = _headpose.transform.position + _headpose.TransformDirection(Offset);
        _currentDistance = (transform.position - offsetPosition).magnitude;
        if (_currentDistance > Threshold)
        {
            _targetPosition = offsetPosition;
        }
        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * LerpSpeed);
    }
}

