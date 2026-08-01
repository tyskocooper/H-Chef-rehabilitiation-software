using UnityEngine;

//cusor reprented by a unity object to represent the position of the hman in-game
public class HmanCursor : MonoBehaviour
{
    
    public HManConnection connection;

    //deadzone prevents slight movements or shakes thorwing the cursor off target
    public float deadzone = 0.02f;

    public float rangeX = 100f;
    public float rangeY = 100f;

   //makes sure the hman cursor is centred in the middle for the start of play
   //please ensure that the hman is physically centred before the start of play
    public Transform origin;


    public float gameWidth = 4f;
    public float gameHeight = 3f;



    public float smoothing = 0.15f; // 0 = raw/instant, higher = smoother but laggier
    
    //mouse based cursor to be used if hman is not connected
    public bool mouseCursor = true;

    private Vector2 _smoothedPos;

    void OnGUI()
{
    if (connection == null) return;

    GUI.Label(new Rect(10, 10, 400, 20), $"Connected: {connection.IsConnected}");
    GUI.Label(new Rect(10, 30, 400, 20), $"Raw: {connection.LocationX:F4}, {connection.LocationY:F4}");
    GUI.Label(new Rect(10, 50, 400, 20), $"Smoothed: {_smoothedPos.x:F4}, {_smoothedPos.y:F4}");
    GUI.Label(new Rect(10, 70, 400, 20), $"Cursor Pos: {transform.position.x:F4}, {transform.position.y:F4}");
}

    void Awake()
    {
        if (connection == null)
            connection = HManConnection.Instance;
    }

    void Update()
    {

        bool hmanConnected = connection !=null && connection.IsConnected;

         Vector2 mapped;

        if (hmanConnected)
        {
            mapped = MapToWorld(connection.LocationX, connection.LocationY);
        }
        else if (mouseCursor && Camera.main != null)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //no 'clicking' of the mouse required. positional cooridnates only similar to hman
       
            Vector3 basePos = origin != null ? origin.position : Vector3.zero;

            Vector2 relative = new Vector2(mouseWorld.x - basePos.x, mouseWorld.y - basePos.y);

            // same paramaters as h-man movement previously establshed
            relative.x = Mathf.Clamp(relative.x, -gameWidth, gameWidth);
            relative.y = Mathf.Clamp(relative.y, -gameHeight, gameHeight);

            mapped = relative;
        }
        else
        {
            mapped = _smoothedPos;
        }

        _smoothedPos = Vector2.Lerp(_smoothedPos, mapped, 1f - smoothing);

        Vector3 finalBase = origin != null ? origin.position : Vector3.zero;
        transform.position = new Vector3(finalBase.x + _smoothedPos.x, finalBase.y + _smoothedPos.y);
    }

    private Vector2 MapToWorld(float rawX, float rawY)
    {
        float nx = Mathf.Abs(rawX) < deadzone ? 0f : rawX;
        float ny = Mathf.Abs(rawY) < deadzone ? 0f : rawY;

        float normX = Mathf.Clamp(nx / rangeX, -1f, 1f);
        float normY = Mathf.Clamp(ny / rangeY, -1f, 1f);

        return new Vector2(normX * gameWidth, normY * gameHeight);
    }
}
