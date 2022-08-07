using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	// ORIGINAL SCRIPT SOURCED FROM: https://gist.github.com/ftvs/5822103

	// Transform of the camera to shake. Grabs the gameObject's transform if null.
	public Transform camTransform;

	// How long the object should shake for.
	public static float shakeDuration = 0.2f;
	private static float _shakeDuration;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	Vector3 originalPos;

	void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}

		_shakeDuration = shakeDuration;
	}

	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}

    private void Update()
    {
		if (shakeDuration > 0)
		{
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shakeDuration = 0f;
			camTransform.localPosition = originalPos;
		}
	}

	public static void Shake()
    {
		shakeDuration = _shakeDuration;
	}
}

