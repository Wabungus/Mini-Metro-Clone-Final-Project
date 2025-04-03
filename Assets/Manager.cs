using System.Collections.Generic;
using UnityEngine;

public class Station
{
    #region Fields
    private GameObject accessor;
    private Transform transform;
    private Rect clickCollider;
    #endregion

    #region PROPERTIES
    /// <summary>
    /// A reference to this station's GameObject.
    /// </summary>
    public GameObject Accessor { get { return accessor; } }

    /// <summary>
    /// The x position in the scene of the station GameObject.
    /// </summary>
    public float PositionX { get { return transform.position.x; } set { transform.position.Set (value, transform.position.y, 0); } }

    /// <summary>
    /// The y position in the scene of the station GameObject.
    /// </summary>
    public float PositionY { get { return transform.position.y; } set { transform.position.Set (transform.position.x, value, 0); } }

    /// <summary>
    /// The x & y scale of the station GameObject (effects SpriteRenderer).
    /// </summary>
    public float Scale { get { return transform.localScale.x; } set { transform.localScale.Set(value, value, 1); } }

    /// <summary>
    /// The collider within which you can click on this station.
    /// </summary>
    public Rect ClickCollider { get { return clickCollider; } }
    #endregion

    #region CONSTRUCTORS
    /// <summary>
    /// Creates a Station class instance, containing shortcuts to its associated station GameObject.
    /// </summary>
    /// <param name="_stationObject">The station GameObject, its position will be applied to this Station class instance.</param>
    /// <param name="_icon">The Sprite to apply to the station GameObject.</param>
    public Station (GameObject _stationObject, Sprite _icon)
    {
        accessor = _stationObject;
        transform = _stationObject.transform;
        _stationObject.GetComponent<SpriteRenderer>().sprite = _icon;
        clickCollider = new Rect (
                _stationObject.transform.position.x - 0.2f, 
                _stationObject.transform.position.y - 0.2f,
                0.4f, 0.4f);
    }
    #endregion

    #region Methods
    // Currently empty.
    #endregion
}

public class CameraData
{
    #region FIELDS
    private Camera camera;
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
    #endregion

    #region PROPERTIES
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

        // STEP 2:
        // Set Max Camera view values.
        maxWidth = sizeEnd;
        maxHeight = maxWidth * camera.aspect;
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
        camera.orthographicSize = sizeStart + (sizeEnd - sizeStart) * Mathf.Clamp(_percentSize, 0.0f, 1.0f);
        width = camera.orthographicSize;
        height = width * camera.aspect;
        left = camera.transform.position.x - width * 0.5f;
        right = camera.transform.position.x + width * 0.5f;
        top = camera.transform.position.y + height * 0.5f;
        bottom = camera.transform.position.y - height * 0.5f;
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
    private Vector2 position;
    private Vector2Int initialGridPosition;
    #endregion

    #region PROPERTIES
    /// <summary>
    /// The x position this grid position represents.
    /// </summary>
    public float PositionX { get { return position.x; } }

    /// <summary>
    /// The y position this grid position represents.
    /// </summary>
    public float PositionY { get { return position.y; } }

    /// <summary>
    /// The initial x position in the 'availabilityGrid' of the StationGrid.
    /// </summary>
    public int InitialGridPositionX { get { return initialGridPosition.x; } }

    /// <summary>
    /// The initial y position in the 'availabilityGrid' of the StationGrid.
    /// </summary>
    public int InitialGridPositionY { get { return initialGridPosition.y; } }
    #endregion

    #region CONSTRUCTORS
    /// <summary>
    /// Creates an instance specifically to store information on where stations should be placed.
    /// </summary>
    /// <param name="_position">The actual location this station should be placed at.</param>
    /// <param name="_initialGridPosition">The initial grid position of this station.</param>
    public StationGridReference (Vector2 _position, Vector2Int _initialGridPosition)
    {
        position = _position;
        initialGridPosition = _initialGridPosition;
        used = false;
    }
    #endregion

    #region METHODS
    // Currently empty.
    #endregion
}

public class StationGrid
{
    #region FIELDS
    private CameraData cameraData;
    private float distanceBetweenGridIndices;
    private Vector2Int initialGridDimensions;
    private List<List<StationGridReference>> availabilityGrid;

    private int startingStationTotal;
    private int stationMaximumTotal;
    private List<Station> stations;
    private float stationPlacementScreenPercent;
    private float stationBoundaryRadius;
    #endregion

    #region PROPERTIES
    /// <summary>
    /// Returns the dimensions of the 'availabilityGrid'.
    /// </summary>
    public Vector2Int InitialGridDimensions { get { return initialGridDimensions; } }

    /// <summary>
    /// Returns the number of currently existing stations.
    /// </summary>
    public int stationCount {  get { return stations.Count; } }
    #endregion

    #region CONSTRUCTORS
    public StationGrid (
            CameraData _cameraData, float _distanceBetweenGridIndices, 
            int _startingStationTotal, int _stationMaximumTotal,
            float _stationPlacementScreenPercent, float _stationBoundaryRadius)
    {
        // STEP 1: 
        // Insert data into generic fields.
        cameraData = _cameraData;
        distanceBetweenGridIndices = _distanceBetweenGridIndices;
        startingStationTotal = _startingStationTotal;
        stationMaximumTotal = _stationMaximumTotal;
        stationPlacementScreenPercent = _stationPlacementScreenPercent;
        stationBoundaryRadius = _stationBoundaryRadius;
        stations = new List<Station> ();

        // STEP 2:
        // Gets the initial count to use for 'availabilityGrid' before anything is removed.
        initialGridDimensions = new Vector2Int (
                (int)(_cameraData.MaxWidth / distanceBetweenGridIndices * 10),
                (int)(_cameraData.MaxHeight / distanceBetweenGridIndices * 10));

        // STEP 3:
        // Fills out the 2 layer list with relevant data for placement info.
        for (int _i = 0; _i < initialGridDimensions.x; ++_i)
        {
            availabilityGrid[_i] = new List<StationGridReference> ();
            for (int _j = 0; _j < initialGridDimensions.y; ++_j)
            {
                // TODO - Add check here for river overlaps that skips adding if found.

                availabilityGrid[_i].Add(
                        new StationGridReference (
                                new Vector2 (
                                    _cameraData.MaxLeft 
                                    + _cameraData.MaxWidth 
                                    * (_i / (initialGridDimensions.x * 0.1f)),
                                    _cameraData.MaxBottom 
                                    + _cameraData.MaxHeight 
                                    * (_i / (initialGridDimensions.y * 0.1f))),
                                new Vector2Int (_i, _j)));
            }
        }

        // STEP 4:
        // In case an entire horizontal row in grid wasn't added due to river overlaps, destroy it.
        for (int _i = 0; _i < availabilityGrid.Count; ++_i)
        {
            if (availabilityGrid[_i].Count == 0)
            {
                availabilityGrid[_i].RemoveAt(_i);
                --_i;
            }
        }

        // STEP 5:
        // Add however many stations is the starting amount.
        for (int _i = 0; _i < _startingStationTotal; ++_i) { CreateStation(); }



        // STEP 4:
        // Check all stations within removal square to see if they are within 



        /*
        // STEP 1:
        // Determine start and end indices given current camera view size.
        int _startX =
                // Distance from maximum left edge of camera to current left edge.
                (int)((cameraData.MaxRight - cameraData.Right
                // Portion of total width near the edge of the current camera.
                + cameraData.Width / 2 * stationPlacementScreenPercent)
                // Convert to index positions from world positional data.
                / distanceBetweenGridIndices);
        int _startY =
                // Distance from maximum top edge of camera to current top edge.
                (int)((cameraData.MaxTop - cameraData.Top
                // Portion of total height near the edge of the current camera.
                + cameraData.Height / 2 * stationPlacementScreenPercent)
                // Convert to index positions from world positional data.
                / distanceBetweenGridIndices);
        int _endX = availabilityGridDimensions.x - _startX;
        int _endY = availabilityGridDimensions.y - _startY;

        // STEP 2:
        // 

        // Pick a random position in 'availabilityGrid' to place a station.
        Vector2Int _stationIndex = new Vector2Int (
                Random.Range (_startX, _endX),
                Random.Range(_startY, _endY));

        // STEP 2:
        // Check to see if 
        */
    }
    #endregion

    #region METHODS
    public void CreateStation ()
    {
        // STEP 1:
        // Select a random station position.
        int _randomY = Random.Range(0, availabilityGrid.Count);
        int _randomX = Random.Range(0, availabilityGrid[_randomY].Count);
        StationGridReference _randomStationReference = availabilityGrid[_randomX][_randomY];

        // STEP 2:
        // Create the desired station.
        // TODO - Create station prefab and get sprites!
        // stations.Add(new Station(null, null));

        // STEP 3:
        // Check all stations to see if they are within boundary radius and remove them.
        for (int _i = 0; _i < availabilityGrid.Count; ++_i)
        {
            for (int _j = 0; _j < availabilityGrid[_i].Count; ++_j)
            {
                if (Mathf.Pow(availabilityGrid[_i][_j].PositionX - _randomStationReference.PositionX, 2) 
                    + Mathf.Pow(availabilityGrid[_i][_j].PositionY - _randomStationReference.PositionY, 2)
                    < Mathf.Pow(stationBoundaryRadius, 2))
                {
                    availabilityGrid[_i].RemoveAt(_j);
                    --_j;
                }
            }

            // Remove fully emptied rows.
            if (availabilityGrid[_i].Count == 0)
            {
                availabilityGrid[_i].RemoveAt(_i);
                --_i;
            }
        }
    }

    /// <summary>
    /// Returns an existing Station class instance.
    /// </summary>
    /// <param name="_index">The index of the Station to be returned.</param>
    /// <returns></returns>
    public Station GetStation(int _index) { return stations[_index]; }

    public void UpdateCameraSize (float _percentSize)
    {
        // STEP 1:
        // Actually update camera size.
        cameraData.UpdateCameraSize(_percentSize);

        // Find current edges of 
    }
    #endregion

}

public class Manager : MonoBehaviour
{
    #region FIELDS
    List<Timer> timers;
    #endregion

    #region UNITY MONOBEHAVIOR
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        // STEP 1:
    }

    // Update is called once per frame
    void Update()
    {
        // STEP 1:
        // Increment all existing Timer class instances.
        foreach (Timer timer in timers) timer.IncrementTimer(Time.deltaTime);

        // STEP 2:
    }
    #endregion

    #region METHODS
    
    #endregion
}
