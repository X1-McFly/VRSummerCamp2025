using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(Rigidbody))]
public class XRCarController : MonoBehaviour
{
    [Header("Wheel References")]
    public WheelController frontLeftWheel;
    public WheelController frontRightWheel;

    [Header("Input Settings")]
    [Range(0f, 0.5f)] public float steerDeadZone   = 0.2f;
    [Range(0f, 0.5f)] public float triggerDeadZone = 0.1f;

    [Header("Suspension Bounciness")]
    public Collider carCollider;
    [Range(0f, 1f)] public float bounciness = 0.1f;

    Rigidbody rb;
    InputDevice leftHand, rightHand;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InitPhysicMaterial();
        TryInitDevices();
    }

    void InitPhysicMaterial()
    {
        if (carCollider)
        {
            var mat = new PhysicMaterial("CarBounce");
            mat.bounciness = bounciness;
            mat.bounceCombine = PhysicMaterialCombine.Multiply;
            carCollider.material = mat;
        }
    }

    void TryInitDevices()
    {
        var lh = new List<InputDevice>(), rh = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand,  lh);
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rh);
        if (lh.Count > 0) leftHand  = lh[0];
        if (rh.Count > 0) rightHand = rh[0];
    }

    void FixedUpdate()
    {
        if (!leftHand.isValid || !rightHand.isValid)
            TryInitDevices();

        // Steering (left stick X)
        leftHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 ls);
        float steerInput = Mathf.Abs(ls.x) > steerDeadZone ? ls.x : 0f;

        // Triggers for accel/brake
        leftHand .TryGetFeatureValue(CommonUsages.trigger, out float rawBrake);
        rightHand.TryGetFeatureValue(CommonUsages.trigger, out float rawAccel);
        float brakeInput = rawBrake > triggerDeadZone ? rawBrake : 0f;
        float accelInput = rawAccel > triggerDeadZone ? rawAccel : 0f;

        // Update front wheels
        frontLeftWheel .UpdateWheel(steerInput, accelInput, brakeInput);
        frontRightWheel.UpdateWheel(steerInput, accelInput, brakeInput);
    }
}
