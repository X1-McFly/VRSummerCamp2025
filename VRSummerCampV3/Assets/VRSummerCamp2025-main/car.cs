using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(Rigidbody))]
public class XRCarController : MonoBehaviour
{
    [Header("Physics")]
    public float acceleration   = 100f;
    public float maxSpeed       = 20f;
    public float brakeForce     = 100f;

    [Header("Steering")]
    [Range(0f,1f)] public float steerSensitivity = 0.8f;
    public float maxSteerAngle = 10f;

    [Header("Input Settings")]
    [Range(0f,0.5f)] public float steerDeadZone   = 0.2f;
    [Range(0f,0.5f)] public float triggerDeadZone = 0.1f;

    Rigidbody   rb;
    InputDevice leftHand, rightHand;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        TryInitDevices();
    }

    void TryInitDevices()
    {
        var lh = new List<InputDevice>();
        var rh = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand,  lh);
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rh);
        if (lh.Count > 0) leftHand  = lh[0];
        if (rh.Count > 0) rightHand = rh[0];
    }

    void FixedUpdate()
    {
        if (!leftHand.isValid || !rightHand.isValid)
            TryInitDevices();

        // --- Steering (left stick X) ---
        leftHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 ls);
        float steerInput = Mathf.Abs(ls.x) > steerDeadZone ? ls.x : 0f;

        // --- Triggers ---
        leftHand .TryGetFeatureValue(CommonUsages.trigger, out float rawBrake);
        rightHand.TryGetFeatureValue(CommonUsages.trigger, out float rawAccel);
        float brakeInput = rawBrake > triggerDeadZone ? rawBrake : 0f;
        float accelInput = rawAccel > triggerDeadZone ? rawAccel : 0f;

        // --- Forward / Reverse (exclusive) ---
        if (accelInput > 0f)
            rb.AddForce(transform.forward * accelInput * acceleration * Time.fixedDeltaTime);
        else if (brakeInput > 0f)
            rb.AddForce(-transform.forward * brakeInput  * brakeForce   * Time.fixedDeltaTime);

        // --- Speed cap ---
        Vector3 fv = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (fv.magnitude > maxSpeed)
            rb.linearVelocity = fv.normalized * maxSpeed + Vector3.up * rb.linearVelocity.y;

        // --- Apply steering ---
        float steerAngle = steerInput * maxSteerAngle * steerSensitivity;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, steerAngle, 0f));
    }
}