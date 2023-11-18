using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InputController : MonoBehaviour
{

    public static InputController _Instance { get; set; }
    public event Action<Axis> onAxisChange;
    public GameObject player;

    private static Axis axis = new Axis { Horizontal = 0, Vertical =0};
    Axis LastAxis = new Axis { Horizontal = 0, Vertical =0};

    void Start()
    {
        _Instance = this;

    }

    void Update()
    {
        var verticalInput = Input.GetAxis("Vertical");
        var horizontalInput = Input.GetAxis("Horizontal");

        axis.Vertical = Mathf.RoundToInt(verticalInput);
        axis.Horizontal = Mathf.RoundToInt(horizontalInput);

    }

    public void set_Player(GameObject play)
    {
        player = play;
    }

    private void LateUpdate()
    {
        if (AxisChange())
        {
            LastAxis = new Axis { Horizontal = axis.Horizontal, Vertical = axis.Vertical };
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


