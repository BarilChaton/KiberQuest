using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class BetterCameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [Range(1, 10)]
    public float smoothFactor;
    [HideInInspector]
    public Vector3 minValues, maxValue;

    [HideInInspector]
    public bool setupComplete = false;

    public enum SetupState { None, Step1, Step2 }
    [HideInInspector]
    public SetupState setupState = SetupState.None;

    private void FixedUpdate()
    {
        Follow();
    }

    void Follow()
    {
        Vector3 targetPosition = target.position + offset;
        //Verify if target position is out of bounds or not
        //Limit it to the min and max values
        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minValues.x, maxValue.x),
            Mathf.Clamp(targetPosition.y, minValues.y, maxValue.y),
            Mathf.Clamp(targetPosition.z, minValues.z, maxValue.z));

        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }

    public void ResetValues()
    {
        setupComplete = false;
        minValues = Vector3.zero;
        maxValue = Vector3.zero;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(BetterCameraController))]
public class BetterCameraControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //Assign the MonoBehaviour target script
        var script = (BetterCameraController)target;
        //Check if values are setup or not

        //Blank space
        GUILayout.Space(20);

        GUIStyle defaultStyle = new GUIStyle();
        defaultStyle.fontSize = 12;
        defaultStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle titleStyle = new GUIStyle();
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 15;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("-=- Camera boundaries settings -=-", titleStyle);
        //If they are setup display the Min and Max values along with preview button
        //Also have a reset button for the values
        if (script.setupComplete)
        {
            GUILayout.Label("-=- Camera boundaries settings -=-", defaultStyle);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Minimum Values:", defaultStyle);
            GUILayout.Label("Maximum Values:", defaultStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"X = {script.minValues.x}", defaultStyle);
            GUILayout.Label($"X = {script.maxValue.x}", defaultStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("View Minimum"))
            {
                // Snap the camera to the minimum values
                Camera.main.transform.position = script.minValues;
            }
            if (GUILayout.Button("View Maximum"))
            {
                // Snap the camera to the maximum values
                Camera.main.transform.position = script.maxValue;
            }
            GUILayout.EndHorizontal();

            //Reset view on target
            if (GUILayout.Button("Focus on Player"))
            {
                Vector3 targetPos = script.target.position;
                targetPos.z = script.minValues.z;
                Camera.main.transform.position = targetPos;
            }

            if (GUILayout.Button("Reset camera values"))
            {
                script.ResetValues();
            }

            //[ View Minimum ] [ View Maximum ]
            //[         Focus on Target       ]
            //[         Reset Values          ]
        }
        //If they are not setup display a start setup button
        else
        {
            //Step0 : Show the start wizard button
            if (script.setupState == BetterCameraController.SetupState.None)
            {
                if (GUILayout.Button("Start setting camera values"))
                {
                    //Changes the state to step1
                    script.setupState = BetterCameraController.SetupState.Step1;
                }
            }
            //Step1 : Setup the bottom left boundary (min values)
            else if (script.setupState == BetterCameraController.SetupState.Step1)
            {
                //Instructions on what to do
                GUILayout.Label($"1- Select your main Camera.",defaultStyle);
                GUILayout.Label($"2- Move it to the bottom left bound limit of your level.",defaultStyle);
                GUILayout.Label($"3- Then click 'Set Minimum Values' button.",defaultStyle);
                
                if (GUILayout.Button("Set Minimum Values"))
                {
                    //Set the minimum value of the camera limit
                    script.minValues = Camera.main.transform.position;
                    //Change to step2
                    script.setupState = BetterCameraController.SetupState.Step2;
                }
            }
            //Step2 : Setup the top right boundary (max values)
            else if (script.setupState == BetterCameraController.SetupState.Step2)
            {
                //Instructions on what to do
                GUILayout.Label($"1- Select your main Camera.", defaultStyle);
                GUILayout.Label($"2- Move it to the top right bound limit of your level.", defaultStyle);
                GUILayout.Label($"3- Then click 'Set Maximum Values' button.", defaultStyle);

                if (GUILayout.Button("Set Maximum Values"))
                {
                    //Set the maximum value of the camera limit
                    script.maxValue = Camera.main.transform.position;
                    //Change to stepNone
                    script.setupState = BetterCameraController.SetupState.None;
                    //Disable the setup complete boolean to false
                    script.setupComplete = true;
                }
            }
            //Last thing disable the setupComplete value
            
            
            
        }




    }
}
#endif