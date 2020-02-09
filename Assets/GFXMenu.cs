/*
 * GFXMenu.cs
 * Created by: #AUTHOR#
 * Created on: #CREATIONDATE# (dd/mm/yy)
 * Created for: #PURPOSE#
 */

using UnityEngine;
using UnityEngine.Rendering;

public class GFXMenu : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] KeyCode _OptionMenuKey = KeyCode.Tilde;
	[SerializeField] Volume _PostProcessVolume;
	[SerializeField] GameObject _GFXHolder;
	bool _GFXHolderStatus = false;
	
	private void Awake()
	{
		
	}
	
	private void Update()
	{
		if (Input.GetKeyDown(_OptionMenuKey))
		{
			_GFXHolderStatus = !_GFXHolderStatus;
			_GFXHolder.SetActive(_GFXHolderStatus);
		}
	}

	public void OnDOFClick()
	{
		_PostProcessVolume.enabled = !_PostProcessVolume.enabled;
	}
}
