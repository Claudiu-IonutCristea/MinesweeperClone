using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraMovement : MonoBehaviour
{
	private Vector3 _difference;
	private Vector3 _origin;

	private Camera _camera;

	public bool drag;
	public float zoomScale;

	private void Start()
	{
		_camera = GetComponent<Camera>();
	}

	public void CameraMove()
	{
		_difference = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - _camera.transform.position;
		if(drag == false)
		{
			drag = true;
			_origin = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		}

		if(drag)
		{
			_camera.transform.position = _origin - _difference;
		}
	}

	public void StopCameraMove()
	{
		drag = false;
	}

	public void Zoom(float value)
	{
		_camera.orthographicSize += value * zoomScale;
		_camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, 1.0f, 50.0f);
	}
}
