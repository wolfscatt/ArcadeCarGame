using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarLights : MonoBehaviour
{
    public enum Side
    {
        Front,
        Back
    }

    [Serializable]
    public struct Light
    {
        public GameObject lightObj;
        public Material lightMat;
        public Side side;
    }

    public Toggle lightToggle;

    public bool isFrontLightOn;
    public bool isBackLightOn;

    public Color frontLightOnColor;
    public Color frontLightOffColor;
    public Color backLightOnColor;
    public Color backLightOffColor;

    public List<Light> lights;


    private void Start()
    {
        isFrontLightOn = lightToggle.isOn;
        isBackLightOn = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            OperateFrontLights();
        }
    }

    public void OperateFrontLights()
    {
        isFrontLightOn = !isFrontLightOn;

        if (isFrontLightOn)
        {
            foreach (var light in lights)
            {
                if (light.side == Side.Front && light.lightObj.activeInHierarchy == false)
                {
                    light.lightObj.SetActive(true);
                    light.lightMat.color = frontLightOnColor;
                }
            }
            lightToggle.gameObject.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            foreach (var light in lights)
            {
                if (light.side == Side.Front && light.lightObj.activeInHierarchy == true)
                {
                    light.lightObj.SetActive(false);
                    light.lightMat.color = frontLightOffColor;
                }
            }
            lightToggle.gameObject.GetComponent<Image>().color = Color.white;
        }

    }

    public void OperateBackLights()
    {
        if (isBackLightOn)
        {
            foreach (var light in lights)
            {
                if (light.side == Side.Back && light.lightObj.activeInHierarchy == false)
                {
                    light.lightObj.SetActive(true);
                    light.lightMat.color = backLightOnColor;
                }
            }
        }
        else
        {
            foreach (var light in lights)
            {
                if (light.side == Side.Back && light.lightObj.activeInHierarchy == true)
                {
                    light.lightObj.SetActive(false);
                    light.lightMat.color = backLightOffColor;
                }
            }
        }

    }
}