using FishNet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preloader : MonoBehaviour
{
	private void Awake()
	{
		string[] arguments = System.Environment.GetCommandLineArgs();

		for (int i = 0; i < arguments.Length; i++)
		{
			if (arguments[i] == "-racingserver")
			{
				InstanceFinder.ServerManager.StartConnection();
			}
		}
	}
}
