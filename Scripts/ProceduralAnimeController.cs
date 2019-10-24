using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralAnimeController : MonoBehaviour
{
    [SerializeField] private Transform _target;

    [Header("Head")] [SerializeField] private Transform _headBone;
    [SerializeField] private float _headMaxTurnAngle;
    [SerializeField] private float _headTrackingSpeed;

    [Header("Eyes")] [SerializeField] private Transform _leftEyeBone;
    [SerializeField] private Transform _rightEyeBone;
    [SerializeField] private float _eyeTrackingSpeed;
    [SerializeField] private float _leftEyeMaxYRotation;
    [SerializeField] private float _leftEyeMinYRotation;
    [SerializeField] private float _rightEyeMaxYRotation;
    [SerializeField] private float _rightEyeMinYRotation;

    [Header("Legs")]
    [SerializeField] private LegIKStepper _frontLeftLegStepper;
    [SerializeField] private LegIKStepper _frontRightLegStepper;
    [SerializeField] private LegIKStepper _backLeftLegStepper;
    [SerializeField] private LegIKStepper _backRightLegStepper;

    [Header("Movement")] 
    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _turnAcceleration;
    [SerializeField] private float _moveAcceleration;
    [SerializeField] private float _minDistToTarget;
    [SerializeField] private float _maxDistToTarget;
    [SerializeField] private float _maxAngelToTarget;

    public Vector3 _currentVelocity;
    public Vector3 FlockAdditon;
    private float _currentAngularVelocity;
    
    private void Start()
    {
        _frontLeftLegStepper.gameObject.transform.parent = null;
        _frontRightLegStepper.gameObject.transform.parent = null;
        _backLeftLegStepper.gameObject.transform.parent = null;
        _backRightLegStepper.gameObject.transform.parent = null;
        StartCoroutine(LegUpdate());
    }


    void RootMotionUpdate(float dt)
    {
        //Rotation
        Vector3 towardTarget = _target.position - transform.position;
        Vector3 towardTargetProjected = Vector3.ProjectOnPlane(towardTarget, transform.up);
        float angelToTarget = Vector3.SignedAngle(transform.forward, towardTargetProjected, transform.up);
        float targetAngularVelocity = 0;

        if (Mathf.Abs(angelToTarget) > _maxAngelToTarget)
        {
            if (angelToTarget > 0)
                targetAngularVelocity = _turnSpeed;
            else
                targetAngularVelocity = -_turnSpeed;
        }

        _currentAngularVelocity = Mathf.Lerp(_currentAngularVelocity, targetAngularVelocity,
            1 - Mathf.Exp(-_turnAcceleration * Time.deltaTime));
        
        transform.Rotate(0, _currentAngularVelocity * dt, 0, Space.World); //Y in worldspace

        //Position
        Vector3 targetVelocity = Vector3.zero;

        if (Mathf.Abs(angelToTarget) < 90)
        {
            float distToTarget = Vector3.Distance(transform.position, _target.position);

            if (distToTarget > _maxDistToTarget)
                targetVelocity = _moveSpeed * towardTargetProjected.normalized;
            
            else if (distToTarget < _minDistToTarget)
                targetVelocity = (_moveSpeed / 2f) * -towardTargetProjected.normalized;

        }

        _currentVelocity = Vector3.Lerp(_currentVelocity, targetVelocity, 1 - Mathf.Exp(-_moveAcceleration * dt));
        transform.position += _currentVelocity * dt;
        
        if((transform.position - _target.position).magnitude > 5f)
            transform.position += FlockAdditon * dt;

    }

    private void LateUpdate()
    {
        float dt = Time.deltaTime;
        
        RootMotionUpdate(dt);
        HeadTrackingUpdate(dt);
        EyeTrackingUpdate(dt);
    }

    private void HeadTrackingUpdate(float dt)
    {
      //  Quaternion currentLocalRotation = _headBone.localRotation;
      //  _headBone.localRotation = Quaternion.identity;
      //  Vector3 targetWorldLookDir = _target.position - _headBone.position;
      //  Vector3 targetLocalLookDir = _headBone.InverseTransformDirection(targetWorldLookDir);
//
      //  //Clamp head angle
      //  targetLocalLookDir =
      //      Vector3.RotateTowards(Vector3.forward, targetLocalLookDir, Mathf.Deg2Rad * _headMaxTurnAngle, 0);
//
      //  Quaternion targetLocalRotation = Quaternion.LookRotation(targetLocalLookDir, Vector3.up);
//
      //  //Slerp to the angle
      //  _headBone.localRotation = Quaternion.Slerp(currentLocalRotation, targetLocalRotation,
      //      1 - Mathf.Exp((-_headTrackingSpeed * dt)));
    }

    private void EyeTrackingUpdate(float dt)
    {
        ////Head already clamped!
        //Quaternion targetEyeRotation = Quaternion.LookRotation(_target.position - _headBone.position, transform.up);
//
        //_leftEyeBone.rotation =
        //    Quaternion.Slerp(_leftEyeBone.rotation, targetEyeRotation, 1 - Mathf.Exp(-_eyeTrackingSpeed * dt));
        //_rightEyeBone.rotation =
        //    Quaternion.Slerp(_rightEyeBone.rotation, targetEyeRotation, 1 - Mathf.Exp(-_eyeTrackingSpeed * dt));
//
        //float leftEyeCurrentYRotation = _leftEyeBone.localEulerAngles.y;
        //float rightEyeCurrentYRotation = _rightEyeBone.localEulerAngles.y;
//
        ////-360 as we want -180 to 180 range as we only want to manipulate one axis and are using euler angle method for doing it
        //if (leftEyeCurrentYRotation > 180)
        //    leftEyeCurrentYRotation -= 360;
        //if (rightEyeCurrentYRotation > 180)
        //    rightEyeCurrentYRotation -= 360;
//
        //float leftEyeClampedYRoation = Mathf.Clamp(leftEyeCurrentYRotation, _leftEyeMinYRotation, _leftEyeMaxYRotation);
        //float rightEyeClampedYRoation =
        //    Mathf.Clamp(rightEyeCurrentYRotation, _rightEyeMinYRotation, _rightEyeMaxYRotation);
//
        //_leftEyeBone.localEulerAngles = new Vector3(leftEyeClampedYRoation, _leftEyeBone.localEulerAngles.y,
        //    _leftEyeBone.localEulerAngles.z);
        //_rightEyeBone.localEulerAngles = new Vector3(rightEyeClampedYRoation, _rightEyeBone.localEulerAngles.y,
        //    _rightEyeBone.localEulerAngles.z);
    }
    
    IEnumerator LegUpdate()
    {
        while (true)
        {
            do
            {
                _frontLeftLegStepper.TryMove();
                _backRightLegStepper.TryMove();
                yield return null;
                
            } while (_frontLeftLegStepper.Moving || _backRightLegStepper.Moving);

            do
            {
                _frontRightLegStepper.TryMove();
                _backLeftLegStepper.TryMove();
                yield return null;
            } while (_frontRightLegStepper.Moving || _backLeftLegStepper.Moving);
        }
    }
}