using UnityEngine;
using Articares.Core;

public class HManConnection : MonoBehaviour
{
    public static HManConnection Instance { get; private set; }

    public bool IsConnected { get; private set; } = false;
    public enum HManState { Disconnected, Connected, ExerciseRunning, ExerciseStopped }
    public HManState CurrentState { get; private set; } = HManState.Disconnected;

    // Live position, read from the SDK's own internal buffer each frame
    public float LocationX { get; private set; }
    public float LocationY { get; private set; }

    private ArticaresComm _comm;

    private const string HMAN_IP = "192.168.102.1"; // confirm against your device's actual config
    private const int HMAN_PORT = 3000;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _comm = new ArticaresComm();

        bool connected = _comm.EstablishConnection(HMAN_IP, HMAN_PORT);
        SetConnected(connected);

        if (connected)
        {
            RunExercise(1); // single target to start; expand to multi-target later
        }
    }

    public void RunExercise(int numTargets)
{
    if (!IsConnected) return;

    bool started = _comm.StartExercise(numTargets);
    if (started)
    {
        // Free-moving target: zero gain/stiffness/damping = no force applied to handle,
        // but H-MAN will now start sending response data (location_X/Y etc.)
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
    void FixedUpdate()
    {
        if (!IsConnected || CurrentState != HManState.ExerciseRunning) return;

        // SDK updates hman_data internally as messages arrive — just read it
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