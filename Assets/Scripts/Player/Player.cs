using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
	public static Player LocalInstance { get; private set; }

	[field: SerializeField]
	[field: SyncVar]
	public int Score
	{
		get;

		[ServerRpc(RequireOwnership = false)]
		private set;
	}

	public override void OnStartClient()
	{
		base.OnStartClient();

		if (!IsOwner) return;

		LocalInstance = this;

		ViewManager.Instance.Initialize();
	}

	private void Update()
	{
		if (!IsOwner) return;

		if (Input.GetKeyDown(KeyCode.R))
		{
			Score = (Random.Range(0, 1024)); //the setter is a serverRpc, so this is valid
		}
	}
}
