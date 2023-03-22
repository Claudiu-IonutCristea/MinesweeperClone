using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
	private readonly DateTime _startTime;
	private DateTime _lastAccesedTime;
	private bool _stopped;

	public TimeSpan Elapsed
	{
		get
		{
			if(!_stopped)
			{
				_lastAccesedTime = DateTime.Now;
			}

			return _startTime - _lastAccesedTime;
		}
	}

	public bool IsStopped => _stopped;

	public Timer()
	{
		_startTime = DateTime.Now;
		_stopped = false;
	}

	public void Stop()
	{
		_stopped = true;
	}
}
