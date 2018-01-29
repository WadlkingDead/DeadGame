using UnityEngine;
using UnityEditor; 
using System;
using System.Collections.Generic; 
using System.IO;


[CustomEditor(typeof(MotorControl))]


public class MotorEditor : Editor
{

    public void OnInspectorGUI() { MotorControl myPlayer = (MotorControl)target; }

}

	
	
	
	
	
	
