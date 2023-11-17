using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InputController : MonoBehaviour
{
    private float seg;
    private int stateC = 0;
    public static InputController _Instance { get; set; }
    public event Action<Axis> onAxisChange;

    private static Axis axis = new Axis { Horizontal = 0, Vertical =0};
    Axis LastAxis = new Axis { Horizontal = 0, Vertical =0};

    void Start()
    {
        _Instance = this;

    }

    void Update()
    {
        seg += Time.deltaTime; //Contar segundos


        var verticalInput = Input.GetAxis("Vertical");
        var horizontalInput = Input.GetAxis("Horizontal");     
        
        if (seg >= Random.Range(15, 24)) //cambia direcciones cada cierto tiempo de forma random
        {
            stateC = Random.Range(0,3);
            seg = 0;
        }
        if (stateC == 0)
        {
            axis.Vertical = Mathf.RoundToInt(verticalInput); //se mueve normal
            axis.Horizontal = Mathf.RoundToInt(horizontalInput);
        }
        else if (stateC== 1)
        {
            axis.Vertical = Mathf.RoundToInt(verticalInput) * -1;  //se mueve al reves
            axis.Horizontal = Mathf.RoundToInt(horizontalInput) * -1;
        }
        else if(stateC== 2)
        {
            axis.Vertical = Mathf.RoundToInt(horizontalInput);  //vertical pasa a ser horizoltal y viceversa
            axis.Horizontal = Mathf.RoundToInt(verticalInput);
        }else if (stateC == 3)
        {
            axis.Vertical = Mathf.RoundToInt(horizontalInput) * -1;  //además de cambiar ejes, se mueve al revés 
            axis.Horizontal= Mathf.RoundToInt(verticalInput)*-1;
        }

        //axis.Vertical = Mathf.RoundToInt(verticalInput);
        //axis.Horizontal = Mathf.RoundToInt(horizontalInput);
        //Debug.Log(verticalInput);

    }

    private void LateUpdate()
    {
        if (AxisChange())
        {
            LastAxis = new Axis { Horizontal = axis.Horizontal, Vertical = axis.Vertical};
            //NetworkController._Instance.Socket.Emit("move", axis);
            onAxisChange?.Invoke(axis);
        }
    }
 

    private bool AxisChange()
    {
        return (axis.Vertical != LastAxis.Vertical || axis.Horizontal !=LastAxis.Horizontal);
    }
}

public class Axis
{
    public int Horizontal;
    public int Vertical;
}


