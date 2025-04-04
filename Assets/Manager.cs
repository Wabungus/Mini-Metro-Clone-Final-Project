using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Every station shape variant present.
/// </summary>
public enum STATION_SHAPE
{
    SQUARE,
    CIRCLE,
    TRIANGLE,
    DIAMOND,
    PLUS,
    STAR,
    LENGTH // Used for math, not a station shape.
}

public class Station
{
    #region Fields
    private GameObject accessor;
    private Rect clickCollider;
    private STATION_SHAPE shape;
    private Vector2 storedTransferPosition;
    #endregion

    #region PROPERTIES
    /// <summary>
    /// A reference to this station's GameObject.
    /// </summary>
    public GameObject Accessor { get { return accessor; } }

    /// <summary>
    /// The shape type of this station.
    /// </summary>
    public STATION_SHAPE Shape { get { return shape; } }

    /// <summary>
    /// The x position in the scene of the station GameObject.
    /// </summary>
    public float PositionX 
    { 
        get { return accessor.transform.position.x; } 
        set { accessor.transform.position.Set (value, accessor.transform.position.y, 0); } 
    }

    /// <summary>
    /// The y position in the scene of the station GameObject.
    /// </summary>
    public float PositionY 
    { 
        get { return accessor.transform.position.y; } 
        set { accessor.transform.position.Set (accessor.transform.position.x, value, 0); } 
    }

    /// <summary>
    /// The x & y scale of the station GameObject (effects SpriteRenderer).
    /// </summary>
    public float Scale 
    { 
        get { return accessor.transform.localScale.x; } 
        set { accessor.transform.localScale.Set(value, value, 1); } 
    }

    /// <summary>
    /// The collider within which you can click on this station.
    /// </summary>
    public Rect ClickCollider { get { return clickCollider; } }
    #endregion

    #region CONSTRUCTORS
    /// <summary>
    /// Creates a Station class instance, containing shortcuts to its associated station GameObject.
    /// </summary>
    /// <param name="_storedTransferPosition">Where to place the station object once it is created.</param>
    /// <param name="_shape">The shape for this station, converts to enum format internally.</param>
    /// <param name="_icon">The Sprite to apply to the station GameObject.</param>
    public Station (Vector2 _storedTransferPosition, int _shape)
    {
        storedTransferPosition = _storedTransferPosition;
        shape = (STATION_SHAPE)_shape;
        clickCollider = new Rect (
                storedTransferPosition.x - 0.2f,
                storedTransferPosition.y - 0.2f,
                0.4f, 0.4f);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Injects the action GameObject of the station in since it can only be created in the main class.
    /// </summary>
    /// <param name="_stationGameObject">Station GameObject to inject</param>
    /// <param name="_icon">The sprite to use for this game object</param>
    public void InjectStationObject(GameObject _stationGameObject, Sprite _icon)
    {
        if (accessor != null) return;
        accessor = _stationGameObject;
        accessor.transform.position = storedTransferPosition;
        accessor.GetComponent<SpriteRenderer>().sprite = _icon;
        
    }
    #endregion
}

public class CameraData
{
    #region FIELDS
    private Camera camera;
    private bool debugMode;
    private float sizeStart;
    private float sizeEnd;
    private float width;
    private float height;
    private float left;
    private float top;
    private float right;
    private float bottom;
    private float maxWidth;
    private float maxHeight;
    private float maxLeft;
    private float maxTop;
    private float maxRight;
    private float maxBottom;
    private float currentPercent;
    #endregion

    #region PROPERTIES
    /// <summary>
    /// The actual Camera object.
    /// </summary>
    public Camera Accessor { get { return camera; } }

    /// <summary>
    /// Whether or not the camera is currently in debug mode.
    /// </summary>
    public bool DebugMode { get { return debugMode; } }

    /// <summary>
    /// The current x position of the Camera.
    /// </summary>
    public float PositionX { get { return camera.transform.position.x; } }

    /// <summary>
    /// The current y position of the Camera.
    /// </summary>
    public float PositionY { get { return camera.transform.position.y; } }

    /// <summary>
    /// The current width of the Camera's view.
    /// </summary>
    public float Width { get { return width; } }

    /// <summary>
    /// The current height of the Camera's view.
    /// </summary>
    public float Height { get { return height; } }

    /// <summary>
    /// The current position in the scene of the Camera's left edge.
    /// </summary>
    public float Left { get { return left; } }

    /// <summary>
    /// The current position in the scene of the Camera's top edge.
    /// </summary>
    public float Top { get { return top; } }

    /// <summary>
    /// The current position in the scene of the Camera's right edge.
    /// </summary>
    public float Right { get { return right; } }

    /// <summary>
    /// The current position in the scene of the Camera's bottom edge.
    /// </summary>
    public float Bottom { get { return bottom; } }

    /// <summary>
    /// The maximum width of the Camera's view.
    /// </summary>
    public float MaxWidth { get { return maxWidth; } }

    /// <summary>
    /// The maximum height of the Camera's view.
    /// </summary>
    public float MaxHeight { get { return maxHeight; } }

    /// <summary>
    /// The maximum position in the scene of the Camera's left edge.
    /// </summary>
    public float MaxLeft { get { return maxLeft; } }

    /// <summary>
    /// The maximum position in the scene of the Camera's top edge.
    /// </summary>
    public float MaxTop { get { return maxTop; } }

    /// <summary>
    /// The maximum position in the scene of the Camera's right edge.
    /// </summary>
    public float MaxRight { get { return maxRight; } }

    /// <summary>
    /// The maximum position in the scene of the Camera's bottom edge.
    /// </summary>
    public float MaxBottom { get { return maxBottom; } }
    #endregion

    #region CONSTRUCTORS
    /// <summary>
    /// Creates a storage system for the Camera along with allowing easier access to its information.
    /// </summary>
    /// <param name="_camera">The Camera object in the scene.</param>
    /// <param name="_sizeStart">The starting size to use for the Camera.</param>
    /// <param name="_sizeEnd">The maximum size that the Camera can reach.</param>
    public CameraData (Camera _camera, float _sizeStart, float _sizeEnd)
    {
        // STEP 1:
        // Insert data into generic fields.
        camera = _camera;
        sizeStart = _sizeStart;
        sizeEnd = _sizeEnd;
        UpdateCameraSize(0.0f);
        debugMode = false;

        // STEP 2:
        // Set Max Camera view values.
        maxHeight = sizeEnd * 2;
        maxWidth = maxHeight * camera.aspect;
        maxLeft = camera.transform.position.x - maxWidth * 0.5f;
        maxRight = camera.transform.position.x + maxWidth * 0.5f;
        maxTop = camera.transform.position.y + maxHeight * 0.5f;
        maxBottom = camera.transform.position.y - maxHeight * 0.5f;
    }
    #endregion

    #region METHODS
    /// <summary>
    /// Changes the Camera's' dimensions using a percentage value between 0 and 1.
    /// </summary>
    /// <param name="_size">The percentage for the new size for the Camera between its minimum and maximums.</param>
    public void UpdateCameraSize(float _percentSize)
    {
        currentPercent = _percentSize;
        if (debugMode)
        {
            camera.orthographicSize = sizeEnd;
            height = (sizeStart + (sizeEnd - sizeStart) * Mathf.Clamp(_percentSize, 0.0f, 1.0f)) * 2;
            width = height * camera.aspect;
            left = camera.transform.position.x - width * 0.5f;
            right = camera.transform.position.x + width * 0.5f;
            top = camera.transform.position.y + height * 0.5f;
            bottom = camera.transform.position.y - height * 0.5f;
        }
        else
        {
            camera.orthographicSize = sizeStart + (sizeEnd - sizeStart) * Mathf.Clamp(_percentSize, 0.0f, 1.0f);
            height = camera.orthographicSize * 2;
            width = height * camera.aspect;
            left = camera.transform.position.x - width * 0.5f;
            right = camera.transform.position.x + width * 0.5f;
            top = camera.transform.position.y + height * 0.5f;
            bottom = camera.transform.position.y - height * 0.5f;
        }
    }

    /// <summary>
    /// Toggles on / off debug mode for the camera.
    /// </summary>
    public void ToggleDebugMode ()
    {
        debugMode = !debugMode;
        UpdateCameraSize(currentPercent);
    }
    #endregion
}

public class Timer
{
    #region FIELDS
    private float time;
    private float length;
    private bool trigger;
    #endregion

    #region PROPERTIES
    public bool Trigger { get { return trigger; } }
    #endregion

    #region CONSTRUCTORS
    /// <summary>
    /// Creates a timer that can be incremented, with a trigger to check when it is completed.
    /// </summary>
    /// <param name="_length">The length of the timer, tied to 'Time.deltaTime' (60 = 1 second).</param>
    public Timer (float _length)
    {
        length = _length;
        time = 0.0f;
        trigger = false;
    }
    #endregion

    #region METHODS
    public void IncrementTimer(float _deltaTime)
    {
        time += _deltaTime;
        trigger = (time >= length);
        if (trigger)
        {
            time -= length;
        }
    }
    #endregion
}

public class StationGridReference
{
    #region FIELDS
    private bool used;
    private Vector2 truePosition;
    private Vector2Int gridPosition;
    private Vector2Int removalPosition;
    #endregion

    #region PROPERTIES
    /// <summary>
    /// The x position in the scene.
    /// </summary>
    public float TruePositionX { get { return truePosition.x; } }

    /// <summary>
    /// The y position in the scene.
    /// </summary>
    public float TruePositionY { get { return truePosition.y; } }

    /// <summary>
    /// The x position in the 'trueGrid' of the StationGrid.
    /// </summary>
    public int GridPositionX { get { return gridPosition.x; } }

    /// <summary>
    /// The y position in the 'trueGrid' of the StationGrid.
    /// </summary>
    public int GridPositionY { get { return gridPosition.y; } }

    /// <summary>
    /// The x position in the 'removalList' of the StationGrid.
    /// </summary>
    public int RemovalPositionX { get { return removalPosition.x; } set { removalPosition.x = value; } }

    /// <summary>
    /// The y position in the 'removalList' of the StationGrid.
    /// </summary>
    public int RemovalPositionY { get { return removalPosition.y; } set { removalPosition.y = value; } }

    /// <summary>
    /// Whether or not this station slot can still be used.
    /// </summary>
    public bool Used { get { return used; } set { used = value; } }
    #endregion

    #region CONSTRUCTORS
    /// <summary>
    /// Creates an instance specifically to store information on where stations should be placed.
    /// </summary>
    /// <param name="_position">The actual location this station should be placed at.</param>
    /// <param name="_truePosition">The position in the 'trueGrid'.</param>
    /// <param name="_removalPosition">The position in the 'removalGrid'.</param>
    public StationGridReference (Vector2 _truePosition, Vector2Int _gridPosition, Vector2Int _removalPosition)
    {
        truePosition = _truePosition;
        gridPosition = _gridPosition;
        removalPosition = _removalPosition;
    }
    #endregion

    #region METHODS
    // Currently empty.
    #endregion
}

public class StationGrid
{
    #region FIELDS
    private bool debugMode;
    private CameraData cameraData;
    private float distanceBetweenGridIndices;
    private float stationPlacementScreenPercent;
    private float stationBoundaryRadius;

    private StationGridReference[,] trueGrid;
    private Vector2Int trueGridDimensions;
    private List<List<StationGridReference>> removalGrid;
    private List<int> removalGridRemainingXPositions;

    private int startingStationTotal;
    private int stationMaximumTotal;
    private List<Station> stations;
    private List<Station>[] shapeStations;
    #endregion

    #region PROPERTIES
    /// <summary>
    /// Whether or not the StationGrid is in debug mode for visualizing station spawning.
    /// </summary>
    public bool DebugMode { get { return debugMode; } }

    /// <summary>
    /// Returns the dimensions of the 'trueGrid'.
    /// </summary>
    public Vector2Int TrueGridDimensions { get { return trueGridDimensions; } }

    /// <summary>
    /// Returns the number of currently existing stations.
    /// </summary>
    public int stationCount {  get { return stations.Count; } }

    /// <summary>
    /// The left edge of the current camera view where stations can spawn.
    /// </summary>
    public float StationSpawnLeft 
    { 
        get 
        {
            return cameraData.Left + cameraData.Width * (1.0f - stationPlacementScreenPercent) * 0.5f;
        } 
    }

    /// <summary>
    /// The right edge of the current camera view where stations can spawn.
    /// </summary>
    public float StationSpawnRight
    {
        get
        {
            return cameraData.Right - cameraData.Width * (1.0f - stationPlacementScreenPercent) * 0.5f;
        }
    }

    /// <summary>
    /// The top edge of the current camera view where stations can spawn.
    /// </summary>
    public float StationSpawnTop
    {
        get
        {
            return cameraData.Top - cameraData.Height * (1.0f - stationPlacementScreenPercent) * 0.5f;
        }
    }

    /// <summary>
    /// The bottom edge of the current camera view where stations can spawn.
    /// </summary>
    public float StationSpawnBottom
    {
        get
        {
            return cameraData.Bottom + cameraData.Height * (1.0f - stationPlacementScreenPercent) * 0.5f;
        }
    }
    #endregion

    #region CONSTRUCTORS
    /// <summary>
    /// A submanager which handles stations and their placement within the current camera.
    /// </summary>
    /// <param name="_cameraData">The camera information manager.</param>
    /// <param name="_distanceBetweenGridIndices">How much space in scene should be between each index in the grid.</param>
    /// <param name="_startingStationTotal">How many stations should there be at game start.</param>
    /// <param name="_stationMaximumTotal">The maximum number of stations that can exist.</param>
    /// <param name="_stationPlacementScreenPercent">The percentage of padding around camera's edges to prevent station spawning.</param>
    /// <param name="_stationBoundaryRadius">The radius around a station to block other stations from spawning in.</param>
    /// <param name="_stationPrefab">The basic station prefab to pull from.</param>
    /// <param name="_stationSprites">Array of sprites for stations.</param>
    public StationGrid (
            CameraData _cameraData, float _distanceBetweenGridIndices, 
            int _startingStationTotal, int _stationMaximumTotal,
            float _stationPlacementScreenPercent, float _stationBoundaryRadius)
    {
        // STEP 1:
        // Insert data into generic fields.
        debugMode = false;
        cameraData = _cameraData;
        distanceBetweenGridIndices = _distanceBetweenGridIndices;
        startingStationTotal = _startingStationTotal;
        stationMaximumTotal = _stationMaximumTotal;
        stationPlacementScreenPercent = _stationPlacementScreenPercent;
        stationBoundaryRadius = _stationBoundaryRadius;
        shapeStations = new List<Station>[(int)STATION_SHAPE.LENGTH];
        for (int _i = 0; _i < shapeStations.Length; ++_i) shapeStations[_i] = new List<Station> ();
        stations = new List<Station> ();

        // STEP 2:
        // Get the grid dimensions.
        trueGridDimensions = new Vector2Int (
                (int)(cameraData.MaxWidth * _stationPlacementScreenPercent / distanceBetweenGridIndices),
                (int)(cameraData.MaxHeight * _stationPlacementScreenPercent / distanceBetweenGridIndices));

        // STEP 3:
        // Create the two grids the fill them piecemail.
        trueGrid = new StationGridReference[trueGridDimensions.x, trueGridDimensions.y];
        removalGrid = new List<List<StationGridReference>> ();
        for (int _x = 0; _x < trueGridDimensions.x; ++_x)
        {
            // Add slot to 'removalGrid'
            removalGrid.Add(new List<StationGridReference> ());

            for (int _y = 0; _y < trueGridDimensions.y; ++_y)
            {
                // Create StationGridReference
                StationGridReference _station = new StationGridReference (
                        new Vector2 (
                            cameraData.MaxLeft
                            + cameraData.MaxWidth * (1.0f - stationPlacementScreenPercent) * 0.5f
                            + cameraData.MaxWidth * _stationPlacementScreenPercent
                            * ((_x + 0.5f) / (float)(trueGridDimensions.x)),
                            cameraData.MaxBottom
                            + cameraData.MaxHeight * (1.0f - stationPlacementScreenPercent) * 0.5f
                            + cameraData.MaxHeight * _stationPlacementScreenPercent
                            * ((_y + 0.5f) / (float)(trueGridDimensions.y))),
                        new Vector2Int (_x, _y),
                        new Vector2Int (0, 0)); // Fixes itself whenever GridRemoveUsed() runs.

                // Assign to 'trueGrid'.
                trueGrid[_x, _y] = _station;

                // Check to see if positions overlap with river lines.
                // TODO - finish rivers first, just set them to Used, don't remove that is done below.

                // Assign to 'removalGrid'.
                removalGrid[_x].Add(_station);
            }
        }

        // STEP 4:
        // Remove all used slots & update positions.
        GridRemoveUsed();
    }
    #endregion

    #region METHODS
    /// <summary>
    /// Returns a StationGridReference at a given position in the 'trueGrid'.
    /// </summary>
    /// <param name="_x">X position in true grid to pull from.</param>
    /// <param name="_y">Y position in true grid to pull from.</param>
    /// <returns></returns>
    public StationGridReference GetStationGridReference (int _x, int _y) { return trueGrid[_x, _y]; }

    /// <summary>
    /// Removes used indices and updates positions on those after.
    /// </summary>
    private void GridRemoveUsed ()
    {
        for (int _x = 0; _x < removalGrid.Count; ++_x)
        {
            for (int _y = 0; _y < removalGrid[_x].Count; ++_y)
            {
                if (removalGrid[_x][_y].Used)
                {
                    // Remove from the grid.
                    removalGrid[_x].RemoveAt(_y);
                    --_y;
                }
                else
                {
                    // Update its position
                    removalGrid[_x][_y].RemovalPositionX = _x;
                    removalGrid[_x][_y].RemovalPositionX = _y;
                }
            }

            if (removalGrid[_x].Count == 0)
            {
                removalGrid.RemoveAt(_x);
                --_x;
            }
        }
    }
    
    /// <summary>
    /// Creates a station and places it somewhere on the currently visible screen.
    /// </summary>
    public Station CreateStation ()
    {
        // STEP 1:
        // Make sure a station can still be added.
        if (removalGrid.Count == 0 || stations.Count == stationMaximumTotal) return null;

        // STEP 2:
        // Get all slots that are within current camera view.
        List<StationGridReference> _available = new List<StationGridReference>();
        for (int _x = 0; _x < removalGrid.Count; ++_x)
        {
            for (int _y = 0; _y < removalGrid[_x].Count; ++_y)
            {
                if (removalGrid[_x][_y].TruePositionX
                    > StationSpawnLeft &&
                    removalGrid[_x][_y].TruePositionX
                    < StationSpawnRight &&
                    removalGrid[_x][_y].TruePositionY
                    > StationSpawnBottom &&
                    removalGrid[_x][_y].TruePositionY
                    < StationSpawnTop)
                {
                    _available.Add(removalGrid[_x][_y]);
                }
            }
        }

        // STEP 3:
        // If none are available cancel here.
        if (_available.Count == 0) return null;

        // STEP 4:
        // Pick and create random station.
        StationGridReference _station = _available[Random.Range(0, _available.Count)];
        _station.Used = true;
        int _stationShape = Random.Range(0, (int)STATION_SHAPE.LENGTH);
        Station _stationObject = new Station (new Vector2(_station.TruePositionX, _station.TruePositionY), _stationShape);
        stations.Add(_stationObject);
        shapeStations[_stationShape].Add(_stationObject);

        // STEP 5:
        // Calculate edges of relevance to check within.
        int _leftIndex = -1;
        int _rightIndex = trueGridDimensions.x;
        for (int _x = 0; _x < trueGridDimensions.x; ++_x)
        {
            if (_leftIndex == -1 &&
                trueGrid[_x, 0].TruePositionX > _station.TruePositionX - stationBoundaryRadius)
            {
                _leftIndex = _x;
            }

            if (trueGrid[_x, 0].TruePositionX > _station.TruePositionX + stationBoundaryRadius)
            {
                _rightIndex = _x;
                break;
            }
        }
        int _bottomIndex = -1;
        int _topIndex = trueGridDimensions.y;
        for (int _y = 0; _y < trueGridDimensions.y; ++_y)
        {
            if (_bottomIndex == -1 &&
                trueGrid[0, _y].TruePositionY > _station.TruePositionY - stationBoundaryRadius)
            {
                _bottomIndex = _y;
            }

            if (trueGrid[0, _y].TruePositionY > _station.TruePositionY + stationBoundaryRadius)
            {
                _topIndex = _y;
                break;
            }
        }

        // STEP 6:
        // Make all possible station positions within radius used.
        for (int _x = _leftIndex; _x < _rightIndex; ++_x)
        {
            for (int _y = _bottomIndex; _y < _topIndex; ++_y)
            {
                if (!trueGrid[_x, _y].Used &&
                    Mathf.Pow(trueGrid[_x, _y].TruePositionX - _station.TruePositionX, 2)
                    + Mathf.Pow(trueGrid[_x, _y].TruePositionY - _station.TruePositionY, 2)
                    < Mathf.Pow(stationBoundaryRadius, 2))
                {
                    // Station position within boundary radius and should be made unusable.
                    trueGrid[_x, _y].Used = true;
                }
            }
        }
        
        // STEP 7:
        // Remove all used slots.
        GridRemoveUsed();

        // STEP 8:
        // Return the station so its game object can be made in the main class.
        return _stationObject;
    }

    /// <summary>
    /// Returns an existing Station class instance.
    /// </summary>
    /// <param name="_index">The index of the Station to be returned.</param>
    /// <returns></returns>
    public Station GetStation(int _index) { return stations[_index]; }

    /// <summary>
    /// Toggles on / off debug mode for the StationGrid.
    /// </summary>
    public void ToggleDebugMode ()
    {
        debugMode = !debugMode;
    }
    #endregion

}

public class Manager : MonoBehaviour
{
    #region FIELDS
    // Serialized Elements
    [SerializeField] GameObject stationPrefab;
    [SerializeField] Sprite[] stationSprites = new Sprite[(int)STATION_SHAPE.LENGTH];

    [SerializeField] int startingStationTotal = 2;
    [SerializeField] int stationMaximumTotal = 10;

    // Key element storage systems
    List<Timer> timers;
    CameraData cameraData;
    StationGrid stationGrid;

    bool isGameRunning = false;
    #endregion

    #region UNITY MONOBEHAVIOR
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        // STEP 1:
        // Insert data into generic fields.
        isGameRunning = true;
        cameraData = new CameraData (Camera.main, 5.0f, 10.0f);
        //cameraData.ToggleDebugMode();
        timers = new List<Timer> ();
        stationGrid = new StationGrid (cameraData, 1.0f, startingStationTotal, stationMaximumTotal, 0.9f, 6.0f);
        //stationGrid.ToggleDebugMode();

        // STEP 2:
        // Create starting stations.
        for (int _i = 0; _i < startingStationTotal; ++_i) { CreateStation(); }
    }

    // Update is called once per frame
    void Update ()
    {
        // STEP 1:
        // Increment all existing Timer class instances.
        foreach (Timer timer in timers) timer.IncrementTimer(Time.deltaTime);

        // STEP 2:
        // TODO - Camera scaling and spawing of stations over time.
    }
    #endregion

    #region METHODS
    /// <summary>
    /// Creates a station and instantiates its associated object in the scene.
    /// </summary>
    private void CreateStation()
    {
        Station _station = stationGrid.CreateStation();
        if (_station != null) _station.InjectStationObject(
                Instantiate(stationPrefab, Vector3.zero, Quaternion.identity),
                stationSprites[(int)_station.Shape]);
    }

    // Gizmos
    private void OnDrawGizmos ()
    {
        if (!isGameRunning) return;

        // Show camera size.
        if (cameraData.DebugMode)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine (
                    new Vector3 (cameraData.Left, cameraData.Top, 0),
                    new Vector3 (cameraData.Left, cameraData.Bottom, 0));
            Gizmos.DrawLine (
                    new Vector3 (cameraData.Right, cameraData.Top, 0),
                    new Vector3 (cameraData.Right, cameraData.Bottom, 0));
            Gizmos.DrawLine (
                    new Vector3 (cameraData.Left, cameraData.Top, 0),
                    new Vector3 (cameraData.Right, cameraData.Top, 0));
            Gizmos.DrawLine (
                    new Vector3 (cameraData.Left, cameraData.Bottom, 0),
                    new Vector3 (cameraData.Right, cameraData.Bottom, 0));
        }

        if (stationGrid.DebugMode)
        {
            // Draw spawning screen space
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(
                    new Vector3(stationGrid.StationSpawnLeft, stationGrid.StationSpawnTop, 0),
                    new Vector3(stationGrid.StationSpawnLeft, stationGrid.StationSpawnBottom, 0));
            Gizmos.DrawLine(
                    new Vector3(stationGrid.StationSpawnRight, stationGrid.StationSpawnTop, 0),
                    new Vector3(stationGrid.StationSpawnRight, stationGrid.StationSpawnBottom, 0));
            Gizmos.DrawLine(
                    new Vector3(stationGrid.StationSpawnLeft, stationGrid.StationSpawnTop, 0),
                    new Vector3(stationGrid.StationSpawnRight, stationGrid.StationSpawnTop, 0));
            Gizmos.DrawLine(
                    new Vector3(stationGrid.StationSpawnLeft, stationGrid.StationSpawnBottom, 0),
                    new Vector3(stationGrid.StationSpawnRight, stationGrid.StationSpawnBottom, 0));

            // Show station spawn positions.
            for (int _x = 0; _x < stationGrid.TrueGridDimensions.x; ++_x)
            {
                for (int _y = 0; _y < stationGrid.TrueGridDimensions.y; ++_y)
                {
                    if (stationGrid.GetStationGridReference(_x, _y).Used)
                    {
                        Gizmos.color = Color.red;
                    }
                    else if (stationGrid.GetStationGridReference(_x, _y).TruePositionX
                             < stationGrid.StationSpawnLeft ||
                             stationGrid.GetStationGridReference(_x, _y).TruePositionX
                             > stationGrid.StationSpawnRight ||
                             stationGrid.GetStationGridReference(_x, _y).TruePositionY
                             < stationGrid.StationSpawnBottom ||
                             stationGrid.GetStationGridReference(_x, _y).TruePositionY
                             > stationGrid.StationSpawnTop)
                    {
                        Gizmos.color = Color.yellow;
                    }
                    else
                    {
                        Gizmos.color = Color.green;
                    }

                    Gizmos.DrawSphere(new Vector3(
                            stationGrid.GetStationGridReference(_x, _y).TruePositionX,
                            stationGrid.GetStationGridReference(_x, _y).TruePositionY,
                            0), 0.15f);
                }
            }
        }
    }

    #endregion
}
