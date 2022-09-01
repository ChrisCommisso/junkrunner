using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;
	public Vector3 camOffset;
	public static CameraShake instance;
	// How long the object should shake for.
	public float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	Vector3 originalPos;

	void Awake()
	{
		camOffset = Vector3.zero;
		if (instance == null) { 
		instance = this;
		}
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}

	void FixedUpdate()
	{
		originalPos = transform.position;
		if (instance.shakeDuration > 0)
		{
			camOffset=Random.insideUnitSphere * shakeAmount;

			instance.shakeDuration -= Time.fixedDeltaTime * decreaseFactor;
		}
		else
		{
			instance.shakeDuration = 0f;
			
		}
	}
}
