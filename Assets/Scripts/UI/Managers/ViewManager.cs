using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    public static ViewManager Instance { get; private set; }

	[SerializeField]
	private View[] views;
	
	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		Initialize();
	}

	public void Initialize()
	{
		foreach (var view in views)
		{
			view.Initialize();
		}
	}

	public void Show<TView>() where TView : View
	{
		foreach (var view in views)
		{
			if(view is TView)
			{
				view.Show();
			}
			else
			{
				view.Hide();
			}
		}
	}
}
