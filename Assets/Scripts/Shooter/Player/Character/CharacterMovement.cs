using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : NetworkBehaviour
{
	private CharacterInput characterInput;

	[SerializeField]
	private float speed;

	[SerializeField]
	private float jumpSpeed;

	[SerializeField]
	private float gravityScale;

	private CharacterController characterController;

	private Vector3 velocity;

	public override void OnStartNetwork()
	{
		base.OnStartNetwork();

		characterController = GetComponent<CharacterController>();
		characterInput = GetComponent<CharacterInput>();
	}

	private void Update()
	{
		if (!IsOwner) return;

		Vector3 desiredVelocity = Vector3.ClampMagnitude(((transform.forward * characterInput.vertical) + (transform.right * characterInput.horizontal)) * speed, speed);

		velocity.x = desiredVelocity.x;
		velocity.z = desiredVelocity.z;

		if (characterController.isGrounded)
		{

			velocity.y = 0.0f;
			if (characterInput.jump)
			{
				velocity.y = jumpSpeed;
			}
		}
		else
		{
			velocity.y += Physics.gravity.y * gravityScale * Time.deltaTime;
		}

		characterController.Move(velocity * Time.deltaTime);
	}
}
