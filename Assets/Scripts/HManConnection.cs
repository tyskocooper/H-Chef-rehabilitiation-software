using UnityEngine;
using Articares.Core;
using System;

public class HManConnection : MonoBehaviour
{
    public static HManConnection Instance { get; private set; }

    public bool IsConnected { get; private set; } = false;
    public enum HManState { Disconnected, Connected, ExerciseRunning, ExerciseStopped }
    public HManState CurrentState { get; private set; } = HManState.Disconnected;

    // Location Y and X are read from HMan's SDK (DLLS)
    public float LocationX { get; private set; }
    public float LocationY { get; private set; }

    private ArticaresComm _comm;

    private const string HMan_IP = "192.168.102.1"; 
    private const int HMan_Port = 3000;


    //experimenting with h-mans resistance settings

    public float startingResistance = 20f;
    public float resistanceIncrement = 10f;

    private float currentResistance;



    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _comm = new ArticaresComm();

        bool connected = _comm.EstablishConnection(HMan_IP, HMan_Port);
        SetConnected(connected);

        if (connected)
        {
            currentResistance = startingResistance;

            RunExercise(1); // single target for now BUT can add multiple targets
        }
    }

    public void RunExercise(int numTargets)
{
    if (!IsConnected) return;

    bool started = _comm.StartExercise(numTargets);
    if (started)
    {
   
        bool targetSet = _comm.SetTarget("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0");

        if (targetSet)
        {
            CurrentState = HManState.ExerciseRunning;
        }
        else
        {
            Debug.LogError("SetTarget failed — H-MAN will not send response data.");
        }
    }
}

    public void updateResistance(int score)
    {
        currentResistance = Mathf.Min(startingResistance +(score * resistanceIncrement));
         _comm.SetTarget("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0");
        
    }
    void Update()
    {
        if (!IsConnected || CurrentState != HManState.ExerciseRunning) return;

        LocationX = _comm.hman_data.location_X;
        LocationY = _comm.hman_data.location_Y;
    }

    public void StopExercise()
    {
        if (!IsConnected) return;
        _comm.StopExercise();
        CurrentState = HManState.ExerciseStopped;
    }

    private void SetConnected(bool connected)
    {
        IsConnected = connected;
        CurrentState = connected ? HManState.Connected : HManState.Disconnected;
    }

    void OnDestroy()
    {
        if (IsConnected)
        {
            _comm.StopExercise();
            _comm.CloseConnection();
        }
    }
}