using UnityEngine;

[RequireComponent(typeof(WheelCollider))]
public class WheelController : MonoBehaviour
{
    [Header("Visual")]
    public Transform wheelMesh;

    [Header("Wheel Settings")]
    public bool steerable = true;
    public bool motor = true;
    public float maxMotorTorque = 200f;
    public float maxSteerAngle = 20f;
    public float maxBrakeTorque = 300f;

    private WheelCollider wc;

    void Start()
    {
        wc = GetComponent<WheelCollider>();
    }

    public void UpdateWheel(float steerInput, float motorInput, float brakeInput)
    {
        if (steerable)
            wc.steerAngle = steerInput * maxSteerAngle;

        if (motor)
            wc.motorTorque = wc.isGrounded ? motorInput * maxMotorTorque : 0f;

        wc.brakeTorque = brakeInput * maxBrakeTorque;

        // sync visual mesh
        if (wheelMesh)
        {
            wc.GetWorldPose(out Vector3 pos, out Quaternion rot);
            wheelMesh.position = pos;
            wheelMesh.rotation = rot;
        }
    }
}
