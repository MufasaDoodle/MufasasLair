using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCameraLook : NetworkBehaviour
{
    private CharacterInput input;

	[SerializeField]
	private Transform myCamera;

	[SerializeField]
	private float xMin;

	[SerializeField]
	private float xMax;

	private Vector3 eulerAngles;

	public override void OnStartNetwork()
	{
		base.OnStartNetwork();

		input = GetComponent<CharacterInput>();
	}

	public override void OnStartClient()
	{
		base.OnStartClient();

		myCamera.GetComponent<Camera>().enabled = IsOwner;
		myCamera.GetComponent<AudioListener>().enabled = IsOwner;
	}

	private void Update()
	{
		eulerAngles.x -= input.mouseY;

		eulerAngles.x = Mathf.Clamp(eulerAngles.x, xMin, xMax);

		myCamera.localEulerAngles = eulerAngles;

		transform.Rotate(0.0f, input.mouseX, 0.0f, Space.World);
	}

	public Transform GetCamera()
	{
		return myCamera;
	}
}
