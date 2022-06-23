using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : NetworkBehaviour
{
    private Character character;

	public float horizontal;
	public float vertical;

	public float mouseX;
	public float mouseY;

	public float sensitivity;
	
	public bool jump;

	public bool fire;

	public bool reload;

	public override void OnStartNetwork()
	{
		base.OnStartNetwork();

		character = GetComponent<Character>();
	}

	private void Update()
	{
		if (!IsOwner) return;

		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");

		mouseX = Input.GetAxis("Mouse X") * sensitivity;
		mouseY = Input.GetAxis("Mouse Y") * sensitivity;

		jump = Input.GetButton("Jump");

		fire = Input.GetButton("Fire1");

		reload = Input.GetKeyDown(KeyCode.R);
	}
}
