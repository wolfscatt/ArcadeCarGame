using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public enum ControlMode
    {
        Keyboard,
        Mobile
    }
    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public GameObject wheelEffectObj;
        public ParticleSystem smokeParticle;
        public Axel axel;
    }

    public ControlMode controlMode;

    public float maxAcceleration = 30f;
    public float brakeAcceleration = 50f;
    public float moveTorque = 600f;
    public float brakeTorque = 300f;

    public float turnSenstivity = 1f;
    public float maxSteerAngle = 30f;
    public Vector3 centerOfMass;

    public List<Wheel> wheels;

    private float moveInput;
    private float steerInput;
    private bool isBraking;

    private Rigidbody carRb;

    private CarLights carLights;

    private void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = centerOfMass;

        carLights = GetComponent<CarLights>();
    }
    private void Update()
    {
        GetInputs();
        AnimateWheels();
        WheelEffect();
    }

    private void FixedUpdate()
    {
        Move();
        Steer();
        Brake();
    }

    public void MoveInput(float input)
    {
        moveInput = input;
    }
    public void SteerInput(float input)
    {
        steerInput = input;
    }

    public void HandBrakeInput(bool input)
    {
        Debug.Log("Handbrake:" + input);
        isBraking = input;
    }

    private void GetInputs()
    {
        if (controlMode == ControlMode.Keyboard)
        {
            moveInput = Input.GetAxis("Vertical");
            steerInput = Input.GetAxis("Horizontal");
        }
    }

    private void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveInput * maxAcceleration * Time.deltaTime * moveTorque;
        }
    }

    private void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var steerAngle = steerInput * maxSteerAngle * turnSenstivity;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
            }
        }
    }

    private void Brake()
    {
        if (Input.GetKey(KeyCode.Space) || moveInput == 0 || isBraking)
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = brakeTorque * brakeAcceleration * Time.deltaTime;

            }
            carLights.isBackLightOn = true;
            carLights.OperateBackLights();
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
            carLights.isBackLightOn = false;
            carLights.OperateBackLights();
        }

    }

    private void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }

    private void WheelEffect()
    {
        foreach (var wheel in wheels)
        {
            if ((Input.GetKey(KeyCode.Space) || isBraking) && wheel.axel == Axel.Rear && wheel.wheelCollider.isGrounded == true && carRb.velocity.magnitude > 5f)
            {
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
                wheel.smokeParticle.Emit(1);
            }
            else
            {
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
            }
        }
    }
}

