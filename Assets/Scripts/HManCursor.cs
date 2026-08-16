using UnityEngine;

//cusor reprented by a unity object to represent the position of the hman in-game
public class HmanCursor : MonoBehaviour
{
    
    //references H-Man connection as the cursor reads positional data from SDK
    public HManConnection connection;

    //deadzone prevents slight movements or shakes thorwing the cursor off target
    public float deadzone = 0.02f;


    // range the h-man coordinates can operate in - maps to the game world (kitchen)
    public float rangeX = 100f;
    public float rangeY = 100f;

   //makes sure the hman cursor is centred in the middle for the start of play
   //PLEASE ENSURE H-MAN HANDLE IS PHYSICALLY CENTRED BEFORE APPLICATION IS LAUNCHED
    public Transform origin;

    //defines the playable area in the game world 
    //clamps the cursor within these parameters so it does not travel off screen
    public float gameWidth = 4f;
    public float gameHeight = 3f;


    //controls how smoothly the cursor moves towards its target position
    //this is handy if users feedback that the cursor is too reactive
    //raw h-man position is extremely sensitive to jitters and shakes and will often jolt more than expected
    //increase to add some lag to the cursor movement
    public float smoothing = 0.15f; 
    
    //mouse based cursor to be used if hman is not connected
    //extremely useful for developing the game without HMan connection 
    public bool mouseCursor = true;

    //tracks the smoothed position
    // track cusor position relative to raw h-man position
    //useful if re-editing smoothing
    private Vector2 _smoothedPos;


    void Awake()
    {
        //if no connection is establishes in the inspector, fallback to the original instance
        //this was helpful to prevent loss of connection between scenes
        if (connection == null)
            connection = HManConnection.Instance;
    }

    void Update()
    {
        //only uses recieved H-Man data input if device is connected and live
        bool hmanConnected = connection !=null && connection.IsConnected;

         Vector2 mapped;

        if (hmanConnected)
        {
            //calls LoctationX and LocationY from Articares SDK and maps to the game world
            mapped = MapToWorld(connection.LocationX, connection.LocationY);
        }
        else if (mouseCursor && Camera.main != null)
        {
            //applies previously mentioned mouse alternative to this function
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //no 'clicking' of the mouse required. positional cooridnates only similar to hman
            
            //measures the mouse position relative to the same origin point the H-Man uses
            Vector3 basePos = origin != null ? origin.position : Vector3.zero;

            Vector2 relative = new Vector2(mouseWorld.x - basePos.x, mouseWorld.y - basePos.y);

            // same paramaters as h-man movement previously establshed
            relative.x = Mathf.Clamp(relative.x, -gameWidth, gameWidth);
            relative.y = Mathf.Clamp(relative.y, -gameHeight, gameHeight);

            mapped = relative;
        }
        else
        {
            //cursor holds its last known position if disconnection occurs
            //was not able to apply this to User Acceptance Testing for the H-Man but works for the mouse disconnection 
            mapped = _smoothedPos;
        }
        
        //smoothedPos slights drags behind the target position rather than snapping to it instantly
        //.Lerp returns a point between a minimum and maximum value
        _smoothedPos = Vector2.Lerp(_smoothedPos, mapped, 1f - smoothing);

        transform.position = new Vector3(origin.position.x + _smoothedPos.x, origin.position.y + _smoothedPos.y);
    }
    
        //converts the raw H-Man positional data into the clamped game world position 
        //accounts for deadzone to further stablise against jitters and unintentional movement
        private Vector2 MapToWorld(float rawX, float rawY)
    {
        float nx = Mathf.Abs(rawX) < deadzone ? 0f : rawX;
        float ny = Mathf.Abs(rawY) < deadzone ? 0f : rawY;

        float normX = Mathf.Clamp(nx / rangeX, -1f, 1f);
        float normY = Mathf.Clamp(ny / rangeY, -1f, 1f);

        return new Vector2(normX * gameWidth, normY * gameHeight);
    }
}
