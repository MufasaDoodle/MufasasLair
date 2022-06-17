using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class View : MonoBehaviour
{
    public bool IsInitialized { get; private set; }
    
	public virtual void Initialize()
	{
		IsInitialized = true;
	}

    public virtual void Show()
	{
		gameObject.SetActive(true);
	}

    public virtual void Hide()
	{
		gameObject.SetActive(false);
	}
}
