using UnityEngine;
//Articares DLL
using Articares.Core;

public class HManConnection : MonoBehaviour
{

    //Some code is translated from the MatLab script provided by the University to C#

    //creating an instance of HManConnection so this functional script can be easily re-accessed
    public static HManConnection Instance { get; private set; }

    //true once EstablishConnection has successed
    public bool IsConnected { get; private set; } = false;

    //tracks the current state of the H-Man
    public enum HManState { Disconnected, Connected, ExerciseRunning, ExerciseStopped }
    public HManState CurrentState { get; private set; } = HManState.Disconnected;

    // Location Y and X are read from HMan's SDK (DLLS)
    public float LocationX { get; private set; }
    public float LocationY { get; private set; }


    //ArticaresComm is defined in Articare.Core DLL, helps with communicating with the H-Man device
    //low level functions within the SDK such as TCP/IP sockets, sending commands ect.
    private ArticaresComm _comm;


    //network settings for the H-Man device, provided in the Articares documentation
    private const string HMan_IP = "192.168.102.1"; 
    private const int HMan_Port = 3000;

    void Awake()
    {
        //destroys any duplicate instance that tries to activate
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        //ensures the object is not dropped(or destoryed by the above) when transitioning between scenes
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {   
        _comm = new ArticaresComm();


        //attempts connection to the H-Man's IP/TCP on startup
        bool connected = _comm.EstablishConnection(HMan_IP, HMan_Port);
        SetConnected(connected);

        if (connected)
        {
            RunExercise(1); // single target for now BUT can add multiple targets
        }
    }

    //begins an exercise session with a set number of targets
    public void RunExercise(int numTargets)
{
    if (!IsConnected) return;

    bool started = _comm.StartExercise(numTargets);
    if (started)
    {

        //targets are fixed to allow for proper positional set up and pulled directly from the documentation provided

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
    void Update()
    {
        if (!IsConnected || CurrentState != HManState.ExerciseRunning) return;

        //pulls positional x/y data directly from the SDK for the HManCursor/PlayerController
        LocationX = _comm.hman_data.location_X;
        LocationY = _comm.hman_data.location_Y;
    }

    //stops the exercise session 
    public void StopExercise()
    {
        if (!IsConnected) return;
        _comm.StopExercise();
        CurrentState = HManState.ExerciseStopped;
    }

    //isConnected and CurrentState handle similar functions and their states need to be consistent 
    //grouping under SetConnected means both can be called from the same place while staying consistent 
    private void SetConnected(bool connected)
    {
        IsConnected = connected;
        CurrentState = connected ? HManState.Connected : HManState.Disconnected;
    }

        
    void OnDestroy()
    {
        //cleanly closes connection in one command
        if (IsConnected)
        {
            _comm.StopExercise();
            _comm.CloseConnection();
        }
    }
}