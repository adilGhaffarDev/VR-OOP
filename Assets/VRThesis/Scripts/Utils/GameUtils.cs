using UnityEngine;
using System.Collections.Generic;

public class GameUtils  
{
	private static bool _debugMode = true;

	public static void Log(string log) 
	{
		if (_debugMode)
		{
			Debug.Log(log);
		}
	}
}
