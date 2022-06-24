using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
	public static ViewManager Instance { get; private set; }

	[SerializeField]
	private bool autoInitialize;

	[SerializeField]
	private View[] views;

	[SerializeField]
	private View defaultView;

	[SerializeField]
	private List<ViewMap> otherViews = new();

	private Dictionary<string, GameObject> specificViews = new();

	private void Awake()
	{
		Instance = this;

		foreach (var view in otherViews)
		{
			specificViews[view.viewName] = view.view;
		}
	}

	private void Start()
	{
		if (autoInitialize) Initialize();
	}

	public void Initialize()
	{
		foreach (var view in views)
		{
			view.Initialize();

			view.Hide();
		}

		if(defaultView != null) defaultView.Show();
	}

	public void Show<TView>(object args = null) where TView : View
	{
		foreach (var view in views)
		{
			if (view is TView)
			{
				view.Show(args);
			}
			else
			{
				view.Hide();
			}
		}
	}

	public View GetView<TView>() where TView : View
	{
		foreach (var view in views)
		{
			if (view is TView)
			{
				return view;
			}
		}

		return null;
	}

	public GameObject GetSpecificView(string viewName)
	{
		var view = specificViews[viewName];
		if(view == null)
		{
			Debug.LogError("Tried retrieving a view with name " + viewName + ", but was not found");
			return null;
		}

		return view;
	}

	[Serializable]
	public struct ViewMap
	{
		public string viewName;
		public GameObject view;

		public ViewMap(string viewName, GameObject view)
		{
			this.viewName = viewName;
			this.view = view;
		}
	}
}
