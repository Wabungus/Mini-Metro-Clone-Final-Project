using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

/// <summary>
///     Every station shape variant present.
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

public enum LINE_DIR
{
    E,
    NE,
    N,
    NW,
    W,
    SW,
    S,
    SE
}

public class Station
{
    #region Fields

    private GameObject accessor;
    private Rect clickCollider;
    private STATION_SHAPE shape;
    private Vector2 storedTransferPosition;
    private List<FancyPoint> throughPoints;
    private List<FancyPoint>[] exitingLineDirectionCounts;
    #endregion

    #region PROPERTIES

    /// <summary>
    ///     A reference to this station's GameObject.
    /// </summary>
    public GameObject Accessor
    {
        get { return accessor; }
    }

    /// <summary>
    ///     The shape type of this station.
    /// </summary>
    public STATION_SHAPE Shape
    {
        get { return shape; }
    }

    /// <summary>
    ///     The x position in the scene of the station GameObject.
    /// </summary>
    public float X
    {
        get { return accessor.transform.position.x; }
        set { accessor.transform.position.Set(value, accessor.transform.position.y, 0); }
    }

    /// <summary>
    ///     The y position in the scene of the station GameObject.
    /// </summary>
    public float Y
    {
        get { return accessor.transform.position.y; }
        set { accessor.transform.position.Set(accessor.transform.position.x, value, 0); }
    }

    /// <summary>
    /// The posituion vector of the station GameObject.
    /// </summary>
    public Vector2 Position
    {
        get { return accessor.transform.position; }
    }

    /// <summary>
    ///     The x & y scale of the station GameObject (effects SpriteRenderer).
    /// </summary>
    public float Scale
    {
        get { return accessor.transform.localScale.x; }
        set { accessor.transform.localScale.Set(value, value, 1); }
    }

    /// <summary>
    ///     The collider within which you can click on this station.
    /// </summary>
    public Rect ClickCollider
    {
        get { return clickCollider; }
    }

    public List<FancyPoint> ThroughPoints { get { return throughPoints; } }

    /// <summary>
    /// Contains the number of lines exiting at each station angle.
    /// </summary>
    public List<FancyPoint>[] ExitingLineDirectionCounts { get { return exitingLineDirectionCounts; } }
    #endregion

    #region CONSTRUCTORS

    /// <summary>
    ///     Creates a Station class instance, containing shortcuts to its associated station GameObject.
    /// </summary>
    /// <param name="_storedTransferPosition">Where to place the station object once it is created.</param>
    /// <param name="_shape">The shape for this station, converts to enum format internally.</param>
    /// <param name="_icon">The Sprite to apply to the station GameObject.</param>
    public Station (Vector2 _storedTransferPosition, int _shape)
    {
        storedTransferPosition = _storedTransferPosition;
        shape = (STATION_SHAPE)_shape;
        clickCollider = new Rect(
            storedTransferPosition.x - 0.2f,
            storedTransferPosition.y - 0.2f,
            0.4f, 0.4f);
        throughPoints = new List<FancyPoint>();
        exitingLineDirectionCounts = new List<FancyPoint>[8];
    }

    #endregion

    #region Methods
    /// <summary>
    /// For when a line hooks up to a station, this helps establish where additional lines should go when offsetting.
    /// </summary>
    /// <param name="_lineDirection"></param>
    /// <param name="_fancyPoint"></param>
    public void SetPointToStation(LINE_DIR _lineDirection, FancyPoint _fancyPoint)
    {
        throughPoints.Add(_fancyPoint);
        exitingLineDirectionCounts[(int)_lineDirection].Add(_fancyPoint);
    }

    public void RemovePointToStation(FancyPoint _fancyPoint)
    {
        throughPoints.Remove(_fancyPoint);
        for (int _i = 0; _i < exitingLineDirectionCounts.Length; ++_i)
        {
            exitingLineDirectionCounts[_i].Remove(_fancyPoint);
        }
    }

    /// <summary>
    ///     Injects the action GameObject of the station in since it can only be created in the main class.
    /// </summary>
    /// <param name="_stationGameObject">Station GameObject to inject</param>
    /// <param name="_icon">The sprite to use for this game object</param>
    public void InjectStationObject (GameObject _stationGameObject, Sprite _icon)
    {
        if (accessor != null)
        {
            return;
        }

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
    ///     The actual Camera object.
    /// </summary>
    public Camera Accessor
    {
        get { return camera; }
    }

    /// <summary>
    ///     Whether or not the camera is currently in debug mode.
    /// </summary>
    public bool DebugMode
    {
        get { return debugMode; }
    }

    /// <summary>
    ///     The current x position of the Camera.
    /// </summary>
    public float PositionX
    {
        get { return camera.transform.position.x; }
    }

    /// <summary>
    ///     The current y position of the Camera.
    /// </summary>
    public float PositionY
    {
        get { return camera.transform.position.y; }
    }

    /// <summary>
    ///     The current width of the Camera's view.
    /// </summary>
    public float Width
    {
        get { return width; }
    }

    /// <summary>
    ///     The current height of the Camera's view.
    /// </summary>
    public float Height
    {
        get { return height; }
    }

    /// <summary>
    ///     The current position in the scene of the Camera's left edge.
    /// </summary>
    public float Left
    {
        get { return left; }
    }

    /// <summary>
    ///     The current position in the scene of the Camera's top edge.
    /// </summary>
    public float Top
    {
        get { return top; }
    }

    /// <summary>
    ///     The current position in the scene of the Camera's right edge.
    /// </summary>
    public float Right
    {
        get { return right; }
    }

    /// <summary>
    ///     The current position in the scene of the Camera's bottom edge.
    /// </summary>
    public float Bottom
    {
        get { return bottom; }
    }

    /// <summary>
    ///     The maximum width of the Camera's view.
    /// </summary>
    public float MaxWidth
    {
        get { return maxWidth; }
    }

    /// <summary>
    ///     The maximum height of the Camera's view.
    /// </summary>
    public float MaxHeight
    {
        get { return maxHeight; }
    }

    /// <summary>
    ///     The maximum position in the scene of the Camera's left edge.
    /// </summary>
    public float MaxLeft
    {
        get { return maxLeft; }
    }

    /// <summary>
    ///     The maximum position in the scene of the Camera's top edge.
    /// </summary>
    public float MaxTop
    {
        get { return maxTop; }
    }

    /// <summary>
    ///     The maximum position in the scene of the Camera's right edge.
    /// </summary>
    public float MaxRight
    {
        get { return maxRight; }
    }

    /// <summary>
    ///     The maximum position in the scene of the Camera's bottom edge.
    /// </summary>
    public float MaxBottom
    {
        get { return maxBottom; }
    }

    #endregion

    #region CONSTRUCTORS

    /// <summary>
    ///     Creates a storage system for the Camera along with allowing easier access to its information.
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
    ///     Changes the Camera's' dimensions using a percentage value between 0 and 1.
    /// </summary>
    /// <param name="_size">The percentage for the new size for the Camera between its minimum and maximums.</param>
    public void UpdateCameraSize (float _percentSize)
    {
        currentPercent = _percentSize; // Corrected: Use _percentSize
        if (debugMode)
        {
            camera.orthographicSize = sizeEnd;
            height = (sizeStart + (sizeEnd - sizeStart) * Mathf.Clamp(_percentSize, 0.0f, 1.0f)) * 2; // Corrected: Use _percentSize
            width = height * camera.aspect;
            left = camera.transform.position.x - width * 0.5f;
            right = camera.transform.position.x + width * 0.5f;
            top = camera.transform.position.y + height * 0.5f;
            bottom = camera.transform.position.y - height * 0.5f;
        }
        else
        {
            camera.orthographicSize = sizeStart + (sizeEnd - sizeStart) * Mathf.Clamp(_percentSize, 0.0f, 1.0f); // Corrected: Use _percentSize
            height = camera.orthographicSize * 2;
            width = height * camera.aspect;
            left = camera.transform.position.x - width * 0.5f;
            right = camera.transform.position.x + width * 0.5f;
            top = camera.transform.position.y + height * 0.5f;
            bottom = camera.transform.position.y - height * 0.5f;
        }
    }

    /// <summary>
    ///     Toggles on / off debug mode for the camera.
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
    private bool runsOnce;

    #endregion

    #region PROPERTIES

    /// <summary>
    ///     Set to true when timer completes.
    /// </summary>
    public bool Trigger
    {
        get { return trigger; }
    }

    /// <summary>
    ///     Returns the percentage of time elpased until the camera is at its maximum size.
    /// </summary>
    public float TimerPercentage
    {
        get { return time / length; }
    }

    #endregion

    #region CONSTRUCTORS

    /// <summary>
    ///     Creates a timer that can be incremented, with a trigger to check when it is completed.
    /// </summary>
    /// <param name="_length">The length of the timer, tied to 'Time.deltaTime' (60 = 1 second).</param>
    /// <param name="_runsOnce"> If true, timer will not repeat</param>
    public Timer (float _length, bool _runsOnce)
    {
        runsOnce = _runsOnce;
        length = _length;
        time = 0.0f;
        trigger = false;
    }

    #endregion

    #region METHODS

    /// <summary>
    ///     Increments the timer. If Runsonce is true, the timer will stop incrementing after time > length.
    /// </summary>
    /// <param name="_deltaTime"> Increments the timer. </param>
    public void IncrementTimer (float _deltaTime)
    {
        if (runsOnce)
        {
            time = Mathf.Min(time + _deltaTime, length);
            trigger = time >= length;
        }
        else
        {
            time += _deltaTime;
            trigger = time >= length;
            if (trigger)
            {
                time -= length;
            }
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
    ///     The x position in the scene.
    /// </summary>
    public float TruePositionX
    {
        get { return truePosition.x; }
    }

    /// <summary>
    ///     The y position in the scene.
    /// </summary>
    public float TruePositionY
    {
        get { return truePosition.y; }
    }

    /// <summary>
    ///     The x position in the 'trueGrid' of the StationGrid.
    /// </summary>
    public int GridPositionX
    {
        get { return gridPosition.x; }
    }

    /// <summary>
    ///     The y position in the 'trueGrid' of the StationGrid.
    /// </summary>
    public int GridPositionY
    {
        get { return gridPosition.y; }
    }

    /// <summary>
    ///     The x position in the 'removalList' of the StationGrid.
    /// </summary>
    public int RemovalPositionX
    {
        get { return removalPosition.x; }
        set { removalPosition.x = value; }
    }

    /// <summary>
    ///     The y position in the 'removalList' of the StationGrid.
    /// </summary>
    public int RemovalPositionY
    {
        get { return removalPosition.y; }
        set { removalPosition.y = value; }
    }

    /// <summary>
    ///     Whether or not this station slot can still be used.
    /// </summary>
    public bool Used
    {
        get { return used; }
        set { used = value; }
    }

    #endregion

    #region CONSTRUCTORS

    /// <summary>
    ///     Creates an instance specifically to store information on where stations should be placed.
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
    ///     Whether or not the StationGrid is in debug mode for visualizing station spawning.
    /// </summary>
    public bool DebugMode
    {
        get { return debugMode; }
    }

    /// <summary>
    ///     Returns the dimensions of the 'trueGrid'.
    /// </summary>
    public Vector2Int TrueGridDimensions
    {
        get { return trueGridDimensions; }
    }

    /// <summary>
    ///     Returns the number of currently existing stations.
    /// </summary>
    public int stationCount
    {
        get { return stations.Count; }
    }

    /// <summary>
    ///     The left edge of the current camera view where stations can spawn.
    /// </summary>
    public float StationSpawnLeft
    {
        get
        {
            return cameraData.Left + cameraData.Width * (1.0f - stationPlacementScreenPercent) * 0.5f;
        }
    }

    /// <summary>
    ///     The right edge of the current camera view where stations can spawn.
    /// </summary>
    public float StationSpawnRight
    {
        get
        {
            return cameraData.Right - cameraData.Width * (1.0f - stationPlacementScreenPercent) * 0.5f;
        }
    }

    /// <summary>
    ///     The top edge of the current camera view where stations can spawn.
    /// </summary>
    public float StationSpawnTop
    {
        get
        {
            return cameraData.Top - cameraData.Height * (1.0f - stationPlacementScreenPercent) * 0.5f;
        }
    }

    /// <summary>
    ///     The bottom edge of the current camera view where stations can spawn.
    /// </summary>
    public float StationSpawnBottom
    {
        get
        {
            return cameraData.Bottom + cameraData.Height * (1.0f - stationPlacementScreenPercent) * 0.5f;
        }
    }

    /// <summary>
    /// The list which contains all available stations.
    /// </summary>
    public List<Station> Stations { get { return stations; } }

    /// <summary>
    /// The array of lists which contains all stations organized by shape.
    /// </summary>
    public List<Station>[] ShapeStations { get { return shapeStations; } }
    #endregion

    #region CONSTRUCTORS

    /// <summary>
    ///     A submanager which handles stations and their placement within the current camera.
    /// </summary>
    /// <param name="_cameraData">The camera information manager.</param>
    /// <param name="_distanceBetweenGridIndices">How much space in scene should be between each index in the grid.</param>
    /// <param name="_startingStationTotal">How many stations should there be at game start.</param>
    /// <param name="_stationMaximumTotal">The maximum number of stations that can exist.</param>
    /// <param name="_stationPlacementScreenPercent">The percentage of padding around camera's edges to prevent station spawning.</param>
    /// <param name="_stationBoundaryRadius">The radius around a station to block other stations from spawning in.</param>
    /// <param name="_stationPrefab">The basic station prefab to pull from.</param>
    /// <param name="_stationSprites">Array of sprites for stations.</param>
    /// <param name="_riverManager">Used to turn off any slots that overlap with rivers.</param>
    public StationGrid (
        CameraData _cameraData, float _distanceBetweenGridIndices,
        int _startingStationTotal, int _stationMaximumTotal,
        float _stationPlacementScreenPercent, float _stationBoundaryRadius,
        RiverManager _riverManager)
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
        for (int _i = 0; _i < shapeStations.Length; ++_i)
        {
            shapeStations[_i] = new List<Station>();
        }

        stations = new List<Station>();

        // STEP 2:
        // Get the grid dimensions.
        trueGridDimensions = new Vector2Int(
            (int)(cameraData.MaxWidth * _stationPlacementScreenPercent / distanceBetweenGridIndices),
            (int)(cameraData.MaxHeight * _stationPlacementScreenPercent / distanceBetweenGridIndices));

        // STEP 3:
        // Create the two grids the fill them piecemail.
        trueGrid = new StationGridReference[trueGridDimensions.x, trueGridDimensions.y];
        removalGrid = new List<List<StationGridReference>>();
        for (int _x = 0; _x < trueGridDimensions.x; ++_x)
        {
            // Add slot to 'removalGrid'
            removalGrid.Add(new List<StationGridReference>());

            for (int _y = 0; _y < trueGridDimensions.y; ++_y)
            {
                // Create StationGridReference
                StationGridReference _station = new StationGridReference(
                    new Vector2(
                        cameraData.MaxLeft
                        + cameraData.MaxWidth * (1.0f - stationPlacementScreenPercent) * 0.5f
                        + cameraData.MaxWidth * _stationPlacementScreenPercent
                        * ((_x + 0.5f) / (float)(trueGridDimensions.x)),
                        cameraData.MaxBottom
                        + cameraData.MaxHeight * (1.0f - stationPlacementScreenPercent) * 0.5f
                        + cameraData.MaxHeight * _stationPlacementScreenPercent
                        * ((_y + 0.5f) / (float)(trueGridDimensions.y))),
                    new Vector2Int(_x, _y),
                    new Vector2Int(0, 0)); // Fixes itself whenever GridRemoveUsed() runs.

                // Assign to 'trueGrid'.
                trueGrid[_x, _y] = _station;

                // Check to see if positions overlap with river lines.
                if (_riverManager.PointOverlapWithRiver(new Vector2(_station.TruePositionX, _station.TruePositionY)))
                {
                    _station.Used = true;
                }

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
    ///     Returns a StationGridReference at a given position in the 'trueGrid'.
    /// </summary>
    /// <param name="_x">X position in true grid to pull from.</param>
    /// <param name="_y">Y position in true grid to pull from.</param>
    /// <returns></returns>
    public StationGridReference GetStationGridReference (int _x, int _y)
    {
        return trueGrid[_x, _y];
    }

    /// <summary>
    ///     Removes used indices and updates positions on those after.
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
                    removalGrid[_x][_y].RemovalPositionY = _y;
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
    ///     Creates a station and places it somewhere on the currently visible screen.
    /// </summary>
    public Station CreateStation ()
    {
        // STEP 1:
        // Make sure a station can still be added.
        if (removalGrid.Count == 0 || stations.Count == stationMaximumTotal)
        {
            return null;
        }

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
        if (_available.Count == 0)
        {
            return null;
        }

        // STEP 4:
        // Pick and create random station.
        StationGridReference _station = _available[Random.Range(0, _available.Count)];
        _station.Used = true;
        int _stationShape = Random.Range(0, (int)STATION_SHAPE.LENGTH);
        Station _stationObject = new Station(new Vector2(_station.TruePositionX, _station.TruePositionY), _stationShape);
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
    ///     Returns an existing Station class instance.
    /// </summary>
    /// <param name="_index">The index of the Station to be returned.</param>
    /// <returns></returns>
    public Station GetStation (int _index)
    {
        return stations[_index];
    }

    /// <summary>
    ///     Toggles on / off debug mode for the StationGrid.
    /// </summary>
    public void ToggleDebugMode ()
    {
        debugMode = !debugMode;
    }

    #endregion
}

public class FancyPoint
{
    #region FIELDS
    private Vector2 point;
    private FancyLine linePrevious;
    private LINE_DIR linePreviousDirection;
    private FancyLine lineNext;
    private LINE_DIR lineNextDirection;
    private Station linkedStation;
    private bool destroyed;
    #endregion

    #region PROPERTIES

    public LINE_DIR LinePreviousDirection { get { return linePreviousDirection; } set { linePreviousDirection = value; } }

    public LINE_DIR LineNextDirection { get { return lineNextDirection; } set { lineNextDirection = value; } }

    public float X { get { return point.x; } set { point.x = value; } }

    public float Y { get { return point.y; } set { point.y = value; } }

    public Vector2 Point { get { return point; } set { point = value; } }

    public FancyLine LinePrevious { get { return linePrevious; } set { linePrevious = value; } }

    public FancyLine LineNext { get { return lineNext; } set { lineNext = value; } }

    public Station LinkedStation { get { return linkedStation; } set { linkedStation = value; } }

    public bool Destroyed { get { return destroyed; } set { destroyed = value; } }
    #endregion

    #region CONSTRUCTORS
    public FancyPoint (Vector2 _point)
    {
        point = _point;
        destroyed = false;
    }
    #endregion

    #region METHODS
    #endregion
}

public class FancyDottedPoint
{
    #region FIELDS
    private Vector2 point;
    private FancyDottedPoint pointPrevious;
    private FancyDottedPoint pointNext;
    private bool isLastPoint;
    #endregion

    #region PROPERTIES

    public float X { get { return point.x; } set { point.x = value; } }

    public float Y { get { return point.y; } set { point.y = value; } }

    public Vector2 Point { get { return point; } set { point = value; } }

    public FancyDottedPoint PointPrevious { get { return pointPrevious; } set { pointPrevious = value; } }

    public FancyDottedPoint PointNext { get { return pointNext; } set { pointNext = value; } }

    public bool IsLastPoint { get { return isLastPoint; } set { isLastPoint = value; } }
    #endregion

    #region CONSTRUCTORS
    public FancyDottedPoint (Vector2 _point)
    {
        point = _point;
    }
    #endregion

    #region METHODS
    #endregion
}

public class FancyLine
{
    #region FIELDS
    FancyPoint pointStart;
    FancyPoint pointEnd;

    bool doBending;
    FancyPoint bendPoint;

    float colliderWidth;

    // Collider Points
    Vector2 colliderPoint1;
    Vector2 colliderPoint4;
    
    Vector2 colliderBendPoint1;
    Vector2 colliderBendPoint4;

    Vector2 colliderBendPoint2;
    Vector2 colliderBendPoint3;

    Vector2 colliderPoint2;
    Vector2 colliderPoint3;

    // Angle logic
    float mainAngle;
    float bendAngle1;
    float bendAngle2;

    // Length between mini segments
    FancyDottedPoint startDottedPoint;
    FancyDottedPoint endDottedPoint;
    float lengthBetweenDots;

    // Collider Slopes
    float slope1to2;
    float slope2to3;
    float slope3to4;
    float slope4to1;

    float slope1to1B;
    float slope1Bto4B;
    float slope4Bto4;

    float slope2Bto2;
    float slope3to3B;
    float slope3Bto2B;

    // Collider Y Intercepts
    float yIntercept1to2;
    float yIntercept2to3;
    float yIntercept3to4;
    float yIntercept4to1;

    float yIntercept1to1B;
    float yIntercept1Bto4B;
    float yIntercept4Bto4;

    float yIntercept2Bto2;
    float yIntercept3to3B;
    float yIntercept3Bto2B;
    #endregion

    #region PROPERTIES
    public FancyPoint PointStart
    {
        get { return pointStart; }
        set
        {
            pointStart = value;
            SetData();
        }
    }

    public FancyPoint PointEnd
    {
        get { return pointEnd; }
        set
        {
            pointEnd = value;
            SetData();
        }
    }

    public Vector2 VectorPointEnd
    {
        set
        {
            pointEnd.Point = value;
            SetData();
        }
    }

    public bool DoBending { get { return doBending; } }

    public FancyPoint BendPoint { get { return bendPoint; } }

    public FancyDottedPoint StartDottedPoint { get { return startDottedPoint; } }
    #endregion

    #region CONSTRUCTORS
    public FancyLine (FancyPoint _pointStart, FancyPoint _pointEnd, bool _doBending, float _colliderWidth, float _lengthBetweenDots)
    {
        pointStart = _pointStart;
        pointEnd = _pointEnd;
        doBending = _doBending;
        colliderWidth = _colliderWidth;
        lengthBetweenDots = _lengthBetweenDots;
        SetData();
    }
    #endregion

    #region METHODS

    private void SetData ()
    {
        // STEP 1:
        // Set base angle.
        mainAngle = (Mathf.Repeat(Mathf.Atan2(pointEnd.Y - pointStart.Y, pointEnd.X - pointStart.X) * Mathf.Rad2Deg, 360)) * Mathf.Deg2Rad;
        
        // STEP 2:
        // Calculate angle data then stop if not a bend.
        if (doBending)
        {
            SetNewBend();
            BendDottedLine();
            BendCollider();
        }
        else
        {
            NormalDottedLine();
            NormalCollider();
        }
    }

    private void NormalDottedLine ()
    {
        // STEP 1:
        // Calculate length of line and place first point.
        float _lineTotatlLength = Mathf.Sqrt(Mathf.Pow(pointStart.Y - pointEnd.Y, 2) + Mathf.Pow(pointStart.X - pointEnd.X, 2));
        float _lineCurrentLength = 0.0f;
        startDottedPoint = new FancyDottedPoint(new Vector2(pointStart.X, pointStart.Y));
        endDottedPoint = startDottedPoint;
        
        // STEP 2:
        // Place all points between.
        while (_lineCurrentLength + _lineCurrentLength < _lineTotatlLength)
        {
            FancyDottedPoint _newPoint = new FancyDottedPoint(new Vector2(
                        endDottedPoint.X + lengthBetweenDots * Mathf.Cos(mainAngle),
                        endDottedPoint.Y + lengthBetweenDots * Mathf.Sin(mainAngle)));
            endDottedPoint.PointNext = _newPoint;
            _newPoint.PointPrevious = endDottedPoint;
            endDottedPoint = _newPoint;
            _lineCurrentLength += lengthBetweenDots;
        }

        // STEP 3:
        // Place final point.
        FancyDottedPoint _newEndPoint = new FancyDottedPoint(new Vector2(pointEnd.X, pointEnd.Y));
        endDottedPoint.PointNext = _newEndPoint;
        _newEndPoint.PointPrevious = endDottedPoint;
        endDottedPoint = _newEndPoint;
    }

    private void NormalCollider ()
    {
        // STEP 1:
        // Get the points for collision.
        float _perpendicularAngle = mainAngle + (Mathf.PI / 2);
        Vector2 _pointOffset = new Vector2(
                (colliderWidth / 2.0f) * Mathf.Cos(_perpendicularAngle), 
                (colliderWidth / 2.0f) * Mathf.Sin(_perpendicularAngle));
        colliderPoint1 = new Vector2(pointStart.X + _pointOffset.x, pointStart.Y + _pointOffset.y);
        colliderPoint4 = new Vector2(pointStart.X - _pointOffset.x, pointStart.Y - _pointOffset.y);
        colliderPoint2 = new Vector2(pointEnd.X + _pointOffset.x, pointEnd.Y + _pointOffset.y);
        colliderPoint3 = new Vector2(pointEnd.X - _pointOffset.x, pointEnd.Y - _pointOffset.y);

        // STEP 2:
        // Calculate slopes.
        slope1to2 = ((colliderPoint1.y - colliderPoint2.y) / (colliderPoint1.x - colliderPoint2.x));
        slope2to3 = ((colliderPoint2.y - colliderPoint3.y) / (colliderPoint2.x - colliderPoint3.x));
        slope3to4 = ((colliderPoint3.y - colliderPoint4.y) / (colliderPoint3.x - colliderPoint4.x));
        slope4to1 = ((colliderPoint4.y - colliderPoint1.y) / (colliderPoint4.x - colliderPoint1.x));

        // STEP 3:
        // Calculate y intercepts.
        yIntercept1to2 = colliderPoint1.y - slope1to2 * colliderPoint1.x;
        yIntercept2to3 = colliderPoint2.y - slope2to3 * colliderPoint2.x;
        yIntercept3to4 = colliderPoint3.y - slope3to4 * colliderPoint3.x;
        yIntercept4to1 = colliderPoint4.y - slope4to1 * colliderPoint4.x;
    }

    /// <summary>
    /// Creates the 45 degreen angle bend between two points by inserting a special point in between two.
    /// </summary>
    private void SetNewBend ()
    {
        // STEP 1:
        // Get the cleaned angles for the 2 lines to be drawn from A and B to intersect at the bend point.
        bendAngle1 = Mathf.Repeat(Mathf.Floor(((mainAngle * Mathf.Rad2Deg + 25.5f)) / 45.0f) * 45.0f, 360);
        if (bendAngle1 == 0.0f)
        {
            bendAngle2 = (mainAngle * Mathf.Rad2Deg > 270) ? 315 : 45;
            Debug.Log("ohio");
            Debug.Log(mainAngle * Mathf.Rad2Deg);
            Debug.Log(bendAngle1);
            Debug.Log(bendAngle2);
        }
        else
        {
            bendAngle2 = 
                    (mainAngle * Mathf.Rad2Deg <= bendAngle1) 
                    ? Mathf.Repeat(bendAngle1 - 45.0f, 360) 
                    : Mathf.Repeat(bendAngle1 + 45.0f, 360);
        }

        // STEP 2:
        // Get the position for the bend point.
        float _bendX = 0;
        float _bendY = 0;
        if (bendAngle1 == 0 && bendAngle2 == 315)
        {
            _bendY = PointStart.Y;
            _bendX = pointEnd.X - Mathf.Abs(PointStart.Y - pointEnd.Y);
        }
        else if (bendAngle1 == 0 && bendAngle2 == 45)
        {
            _bendY = PointStart.Y;
            _bendX = pointEnd.X - Mathf.Abs(PointStart.Y - pointEnd.Y);
        }
        else if (bendAngle1 == 45 && bendAngle2 == 0)
        {
            _bendY = PointEnd.Y;
            _bendX = pointStart.X + Mathf.Abs(PointEnd.Y - pointStart.Y);
        }
        else if (bendAngle1 == 45 && bendAngle2 == 90)
        {
            _bendX = PointEnd.X;
            _bendY = pointStart.Y + Mathf.Abs(PointEnd.X - pointStart.X);
        }
        else if (bendAngle1 == 90 && bendAngle2 == 45)
        {
            _bendX = PointStart.X;
            _bendY = pointEnd.Y - Mathf.Abs(PointStart.X - pointEnd.X);
        }
        else if (bendAngle1 == 90 && bendAngle2 == 135)
        {
            _bendX = PointStart.X;
            _bendY = pointEnd.Y - Mathf.Abs(PointStart.X - pointEnd.X);
        }
        else if (bendAngle1 == 135 && bendAngle2 == 90)
        {
            _bendX = PointEnd.X;
            _bendY = pointStart.Y + Mathf.Abs(PointEnd.X - pointStart.X);
        }
        else if (bendAngle1 == 135 && bendAngle2 == 180)
        {
            _bendY = PointEnd.Y;
            _bendX = pointStart.X - Mathf.Abs(PointEnd.Y - pointStart.Y);
        }
        else if (bendAngle1 == 180 && bendAngle2 == 135)
        {
            _bendY = PointStart.Y;
            _bendX = pointEnd.X + Mathf.Abs(PointStart.Y - pointEnd.Y);
        }
        else if (bendAngle1 == 180 && bendAngle2 == 225)
        {
            _bendY = PointStart.Y;
            _bendX = pointEnd.X + Mathf.Abs(PointStart.Y - pointEnd.Y);
        }
        else if (bendAngle1 == 225 && bendAngle2 == 180)
        {
            _bendY = PointEnd.Y;
            _bendX = pointStart.X - Mathf.Abs(PointEnd.Y - pointStart.Y);
        }
        else if (bendAngle1 == 225 && bendAngle2 == 270)
        {
            _bendX = PointEnd.X;
            _bendY = pointStart.Y - Mathf.Abs(PointEnd.X - pointStart.X);
        }
        else if (bendAngle1 == 270 && bendAngle2 == 225)
        {
            _bendX = PointStart.X;
            _bendY = pointEnd.Y + Mathf.Abs(PointStart.X - pointEnd.X);
        }
        else if (bendAngle1 == 270 && bendAngle2 == 315)
        {
            _bendX = PointStart.X;
            _bendY = pointEnd.Y + Mathf.Abs(PointStart.X - pointEnd.X);
        }
        else if (bendAngle1 == 315 && bendAngle2 == 270)
        {
            _bendX = PointEnd.X;
            _bendY = pointStart.Y - Mathf.Abs(PointEnd.X - pointStart.X);
        }
        else if (bendAngle1 == 315 && bendAngle2 == 0)
        {
            _bendY = PointStart.Y;
            _bendX = pointEnd.X - Mathf.Abs(PointStart.Y - pointEnd.Y);
        }
        bendAngle1 *= Mathf.Deg2Rad;
        bendAngle2 *= Mathf.Deg2Rad;

        // STEP 3:
        // Create the bend point.
        bendPoint = new FancyPoint (new Vector2 (_bendX, _bendY));
    }

    private void BendDottedLine ()
    {
        // STEP 1:
        // Calculate length of line and place first point pre bend.
        float _lineTotatlLength = Mathf.Sqrt(Mathf.Pow(pointStart.Y - bendPoint.Y, 2) + Mathf.Pow(pointStart.X - bendPoint.X, 2));
        float _lineCurrentLength = 0.0f;
        startDottedPoint = new FancyDottedPoint(new Vector2(pointStart.X, pointStart.Y));
        endDottedPoint = startDottedPoint;

        // STEP 2:
        // Place all points between.
        while (_lineCurrentLength + _lineCurrentLength < _lineTotatlLength)
        {
            FancyDottedPoint _newPoint = new FancyDottedPoint(new Vector2(
                        endDottedPoint.X + lengthBetweenDots * Mathf.Cos(bendAngle1),
                        endDottedPoint.Y + lengthBetweenDots * Mathf.Sin(bendAngle1)));
            endDottedPoint.PointNext = _newPoint;
            _newPoint.PointPrevious = endDottedPoint;
            endDottedPoint = _newPoint;
            _lineCurrentLength += lengthBetweenDots;
        }

        // STEP 3:
        // Calculate length of line and place first point after the bend.
        _lineTotatlLength = Mathf.Sqrt(Mathf.Pow(bendPoint.Y - pointEnd.Y, 2) + Mathf.Pow(bendPoint.X - pointEnd.X, 2));
        _lineCurrentLength = 0.0f;
        FancyDottedPoint _bendPoint = new FancyDottedPoint(new Vector2(bendPoint.X, bendPoint.Y));
        endDottedPoint.PointNext = _bendPoint;
        _bendPoint.PointPrevious = endDottedPoint;
        endDottedPoint = _bendPoint;

        // STEP 4:
        // Place all points between.
        while (_lineCurrentLength + _lineCurrentLength < _lineTotatlLength)
        {
            FancyDottedPoint _newPoint = new FancyDottedPoint(new Vector2(
                        endDottedPoint.X + lengthBetweenDots * Mathf.Cos(bendAngle2),
                        endDottedPoint.Y + lengthBetweenDots * Mathf.Sin(bendAngle2)));
            endDottedPoint.PointNext = _newPoint;
            _newPoint.PointPrevious = endDottedPoint;
            endDottedPoint = _newPoint;
            _lineCurrentLength += lengthBetweenDots;
        }

        // STEP 5:
        // Place final point.
        FancyDottedPoint _newEndPoint = new FancyDottedPoint(new Vector2(pointEnd.X, pointEnd.Y));
        endDottedPoint.PointNext = _newEndPoint;
        _newEndPoint.PointPrevious = endDottedPoint;
        endDottedPoint = _newEndPoint;
    }

    private void BendCollider ()
    {
        // STEP 1:
        // Get points for collision.
        float _perpendicularAngle = bendAngle1 + (Mathf.PI / 2);
        Vector2 _pointOffset = new Vector2(
                (colliderWidth / 2.0f) * Mathf.Cos(_perpendicularAngle), 
                (colliderWidth / 2.0f) * Mathf.Sin(_perpendicularAngle));
        colliderPoint1 = new Vector2(pointStart.X + _pointOffset.x, pointStart.Y + _pointOffset.y);
        colliderPoint4 = new Vector2(pointStart.X - _pointOffset.x, pointStart.Y - _pointOffset.y);
        colliderBendPoint1 = new Vector2(bendPoint.X + _pointOffset.x, bendPoint.Y + _pointOffset.y);
        colliderBendPoint4 = new Vector2(bendPoint.X - _pointOffset.x, bendPoint.Y - _pointOffset.y);

        _perpendicularAngle = bendAngle2 + (Mathf.PI / 2);
        _pointOffset = new Vector2(
                (colliderWidth / 2.0f) * Mathf.Cos(_perpendicularAngle), 
                (colliderWidth / 2.0f) * Mathf.Sin(_perpendicularAngle));
        colliderBendPoint2 = new Vector2(bendPoint.X + _pointOffset.x, bendPoint.Y + _pointOffset.y);
        colliderBendPoint3 = new Vector2(bendPoint.X - _pointOffset.x, bendPoint.Y - _pointOffset.y);
        colliderPoint2 = new Vector2(pointEnd.X + _pointOffset.x, pointEnd.Y + _pointOffset.y);
        colliderPoint3 = new Vector2(pointEnd.X - _pointOffset.x, pointEnd.Y - _pointOffset.y);
        
        // STEP 2:
        // Calculate slopes.
        slope1to1B = ((colliderPoint1.y - colliderBendPoint1.y) / (colliderPoint1.x - colliderBendPoint1.x));
        slope1Bto4B = ((colliderBendPoint1.y - colliderBendPoint4.y) / (colliderBendPoint1.x - colliderBendPoint4.x));
        slope4Bto4 = ((colliderBendPoint4.y - colliderPoint4.y) / (colliderBendPoint4.x - colliderPoint4.x));
        slope4to1 = ((colliderPoint4.y - colliderPoint1.y) / (colliderPoint4.x - colliderPoint1.x));

        slope2Bto2 = ((colliderBendPoint2.y - colliderPoint2.y) / (colliderBendPoint2.x - colliderPoint2.x));
        slope2to3 = ((colliderPoint2.y - colliderPoint3.y) / (colliderPoint2.x - colliderPoint3.x));
        slope3to3B = ((colliderPoint3.y - colliderBendPoint3.y) / (colliderPoint3.x - colliderBendPoint3.x));
        slope3Bto2B = ((colliderBendPoint3.y - colliderBendPoint2.y) / (colliderBendPoint3.x - colliderBendPoint2.x));

        // STEP 3:
        // Calculate y intercepts.
        yIntercept1to1B = colliderPoint1.y - slope1to1B * colliderPoint1.x;
        yIntercept1Bto4B = colliderBendPoint1.y - slope1Bto4B * colliderBendPoint1.x;
        yIntercept4Bto4 = colliderBendPoint4.y - slope4Bto4 * colliderBendPoint4.x;
        yIntercept4to1 = colliderPoint4.y - slope4to1 * colliderPoint4.x;

        yIntercept2Bto2 = colliderBendPoint2.y - slope2Bto2 * colliderBendPoint2.x;
        yIntercept2to3 = colliderPoint2.y - slope2to3 * colliderPoint2.x;
        yIntercept3to3B = colliderPoint3.y - slope3to3B * colliderPoint3.x;
        yIntercept3Bto2B = colliderBendPoint3.y - slope3Bto2B * colliderBendPoint3.x;
    }

    public Vector2[] GetColliderPositions ()
    {
        if (DoBending)
        {
            return new Vector2[] 
            { 
                colliderPoint1, colliderBendPoint1, colliderBendPoint4, colliderPoint4, 
                colliderPoint2, colliderBendPoint2, colliderBendPoint3, colliderPoint3
            };
        }
        else
        {
            return new Vector2[]
            {
                colliderPoint1, colliderPoint2, colliderPoint3, colliderPoint4
            };
        }
    }

    public bool PointInCollider(Vector2 _checkingPoint)
    {
        if (doBending)
        {
            bool _withinVertical1 = false;
            bool _withinHorizontal1 = false;
            if (slope1to1B == 0 && slope4Bto4 == 0)
            {
                _withinVertical1 = 
                        ((_checkingPoint.y < colliderPoint1.y && _checkingPoint.y > colliderPoint4.y &&
                          _checkingPoint.y < colliderBendPoint1.y && _checkingPoint.y > colliderBendPoint4.y) ||
                         (_checkingPoint.y > colliderPoint1.y && _checkingPoint.y < colliderPoint4.y &&
                          _checkingPoint.y > colliderBendPoint1.y && _checkingPoint.y < colliderBendPoint4.y));
                _withinHorizontal1 =
                        ((_checkingPoint.x < colliderPoint1.x && _checkingPoint.x > colliderBendPoint1.x &&
                          _checkingPoint.x < colliderPoint4.x && _checkingPoint.x > colliderBendPoint4.x) ||
                         (_checkingPoint.x > colliderPoint1.x && _checkingPoint.x < colliderBendPoint1.x &&
                          _checkingPoint.x > colliderPoint4.x && _checkingPoint.x < colliderBendPoint4.x));
            }
            else
            {
                _withinVertical1 = 
                        ((_checkingPoint.y < slope1to1B * _checkingPoint.x + yIntercept1to1B &&
                          _checkingPoint.y > slope4Bto4 * _checkingPoint.x + yIntercept4Bto4) ||
                         (_checkingPoint.y > slope1to1B * _checkingPoint.x + yIntercept1to1B &&
                          _checkingPoint.y < slope4Bto4 * _checkingPoint.x + yIntercept4Bto4));
                _withinHorizontal1 = 
                        ((_checkingPoint.x < (_checkingPoint.y - yIntercept4to1) / slope4to1 &&
                          _checkingPoint.x > (_checkingPoint.y - yIntercept1Bto4B) / slope1Bto4B) ||
                         (_checkingPoint.x > (_checkingPoint.y - yIntercept4to1) / slope4to1 &&
                          _checkingPoint.x < (_checkingPoint.y - yIntercept1Bto4B) / slope1Bto4B));
            }

            bool _withinVertical2 = false;
            bool _withinHorizontal2 = false;
            if (slope2Bto2 == 0 && slope3to3B == 0)
            {
                _withinVertical1 =
                        ((_checkingPoint.y < colliderBendPoint2.y && _checkingPoint.y > colliderBendPoint3.y &&
                          _checkingPoint.y < colliderPoint2.y && _checkingPoint.y > colliderPoint3.y) ||
                         (_checkingPoint.y > colliderBendPoint2.y && _checkingPoint.y < colliderBendPoint3.y &&
                          _checkingPoint.y > colliderPoint2.y && _checkingPoint.y < colliderPoint3.y));
                _withinHorizontal1 =
                        ((_checkingPoint.x < colliderBendPoint2.x && _checkingPoint.x > colliderPoint2.x &&
                          _checkingPoint.x < colliderBendPoint3.x && _checkingPoint.x > colliderPoint3.x) ||
                         (_checkingPoint.x > colliderBendPoint2.x && _checkingPoint.x < colliderPoint2.x &&
                          _checkingPoint.x > colliderBendPoint3.x && _checkingPoint.x < colliderPoint3.x));
            }
            else
            {
                _withinVertical2 = 
                        ((_checkingPoint.y < slope2Bto2 * _checkingPoint.x + yIntercept2Bto2 &&
                          _checkingPoint.y > slope3to3B * _checkingPoint.x + yIntercept3to3B) ||
                         (_checkingPoint.y > slope2Bto2 * _checkingPoint.x + yIntercept2Bto2 &&
                          _checkingPoint.y < slope3to3B * _checkingPoint.x + yIntercept3to3B));
                _withinHorizontal2 = 
                        ((_checkingPoint.x < (_checkingPoint.y - yIntercept3Bto2B) / slope3Bto2B &&
                          _checkingPoint.x > (_checkingPoint.y - yIntercept2to3) / slope2to3) ||
                         (_checkingPoint.x > (_checkingPoint.y - yIntercept3Bto2B) / slope3Bto2B &&
                          _checkingPoint.x < (_checkingPoint.y - yIntercept2to3) / slope2to3));
            }
            return ((_withinVertical1 && _withinHorizontal1) || (_withinVertical2 && _withinHorizontal2));
        }
        else
        {
            bool _withinVertical = false;
            bool _withinHorizontal = false;
            if (slope1to2 == 0 && slope3to4 == 0)
            {
                _withinVertical =
                        ((_checkingPoint.y < colliderPoint1.y && _checkingPoint.y > colliderPoint4.y &&
                          _checkingPoint.y < colliderPoint2.y && _checkingPoint.y > colliderPoint3.y) ||
                         (_checkingPoint.y > colliderPoint1.y && _checkingPoint.y < colliderPoint4.y &&
                          _checkingPoint.y > colliderPoint2.y && _checkingPoint.y < colliderPoint3.y));
                _withinHorizontal =
                        ((_checkingPoint.x < colliderPoint1.x && _checkingPoint.x > colliderPoint2.x &&
                          _checkingPoint.x < colliderPoint4.x && _checkingPoint.x > colliderPoint3.x) ||
                         (_checkingPoint.x > colliderPoint1.x && _checkingPoint.x < colliderPoint2.x &&
                          _checkingPoint.x > colliderPoint4.x && _checkingPoint.x < colliderPoint3.x));
            }
            else
            {
                _withinVertical =
                        ((_checkingPoint.y < slope1to2 * _checkingPoint.x + yIntercept1to2 &&
                          _checkingPoint.y > slope3to4 * _checkingPoint.x + yIntercept3to4) ||
                         (_checkingPoint.y > slope1to2 * _checkingPoint.x + yIntercept1to2 &&
                          _checkingPoint.y < slope3to4 * _checkingPoint.x + yIntercept3to4));
                _withinHorizontal =
                        ((_checkingPoint.x < (_checkingPoint.y - yIntercept4to1) / slope4to1 &&
                          _checkingPoint.x > (_checkingPoint.y - yIntercept2to3) / slope2to3) ||
                         (_checkingPoint.x > (_checkingPoint.y - yIntercept4to1) / slope4to1 &&
                          _checkingPoint.x < (_checkingPoint.y - yIntercept2to3) / slope2to3));
            }
            return (_withinVertical && _withinHorizontal);
        }
    }
    #endregion
}

public class FancyLineCollection
{
    #region FIELDS
    GameObject accessor;
    LineRenderer lineRenderer;
    List<FancyLine> fancyLines;
    bool isLoop;
    float width;
    #endregion

    #region PROPERTIES
    public bool IsLoop { get { return isLoop; } set { isLoop = value; } }

    public List<FancyLine> FancyLines { get { return fancyLines; } }
    #endregion

    #region CONSTRUCTORS
    public FancyLineCollection (FancyLine _startingLine, float _width)
    {
        fancyLines = new List<FancyLine> ();
        fancyLines.Add(_startingLine);
        width = _width;
    }

    public FancyLineCollection (float _width)
    {
        fancyLines = new List<FancyLine>();
        width = _width;
    }
    #endregion

    #region METHODS
    public void LineVisible (bool _visible)
    {
        lineRenderer.enabled = _visible;
    }

    public Vector3[] GetTotalVectors ()
    {
        List<Vector3> _returner = new List<Vector3> ();
        
        for (int _i = 0; _i < fancyLines.Count; ++_i)
        {
            RecursiveGetTotalVectors(_returner, fancyLines[_i].StartDottedPoint);
        }
        return _returner.ToArray();
    }

    public void RecursiveGetTotalVectors(List<Vector3> _returner, FancyDottedPoint _dottedPoint)
    {
        _returner.Add(_dottedPoint.Point);
        if (_dottedPoint.PointNext != null) RecursiveGetTotalVectors(_returner, _dottedPoint.PointNext);
    }

    private Vector3[] GetTotalPointVectors ()
    {
        List<Vector3> _returner = new List<Vector3>();
        for (int _i = 0; _i < fancyLines.Count; ++_i)
        {
            // Starting point.
            if (_i == 0)
            {
                _returner.Add(fancyLines[_i].PointStart.Point);
            }

            // Bend point.
            if (fancyLines[_i].DoBending)
            {
                _returner.Add(fancyLines[_i].BendPoint.Point);
            }

            // Ending point.
            _returner.Add(fancyLines[_i].PointEnd.Point);
        }
        return _returner.ToArray();
    }

    public void SetRendererPoints ()
    {
        Vector3[] _vectors = GetTotalPointVectors();
        lineRenderer.positionCount = _vectors.Length;
        lineRenderer.SetPositions(_vectors);
        /*float _incrementCurvePosition = (1.0f / _vectors.Length);
        float _currentIncrement = 0;
        float _widthCurrent = 1.0f;
        int _widthFlip = 0;
        Debug.Log(_incrementCurvePosition);
        AnimationCurve _setCurve = new AnimationCurve();

        while (_currentIncrement < 1)
        {
            Debug.Log(_currentIncrement);
            _setCurve.AddKey(_currentIncrement, _widthCurrent);
            _widthFlip++;
            if (_widthFlip > 2)
            {
                _widthCurrent = (_widthCurrent == 1.0f) ? 0.0f : 1.0f;
                _widthFlip = 0;
            }
            _currentIncrement += _incrementCurvePosition;
        }
        _setCurve.AddKey(_currentIncrement, _widthCurrent);
        lineRenderer.widthCurve = _setCurve;*/
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }

    public void AddFirstLine(FancyPoint _newPoint1, FancyPoint _newPoint2, bool _doBending, float _colliderWidth, float _lengthBetweenDots)
    {
        FancyLine _line = new FancyLine(
                _newPoint1,
                _newPoint2,
                _doBending,
                _colliderWidth,
                _lengthBetweenDots);
        _newPoint1.LineNext = _line;
        _newPoint2.LinePrevious = _line;
        fancyLines.Add(_line);
    }

    public void ClearLine()
    {
        for (int _i = 0; _i < fancyLines.Count; ++_i)
        {
            fancyLines[_i].PointStart.Destroyed = true;
        }
        fancyLines[fancyLines.Count - 1].PointEnd.Destroyed = true;
        fancyLines.Clear();
    }

    public void DeletePointFromLine(FancyPoint _targetPoint)
    {
        for (int _i = 0; _i < fancyLines.Count; ++_i)
        {
            if (_i == 0)
            {
                if (fancyLines[_i].PointStart == _targetPoint)
                {
                    fancyLines[_i].PointStart.Destroyed = true;
                    fancyLines[_i].PointEnd.LinePrevious = null;
                    fancyLines.RemoveAt(_i);
                    return;
                }
            }
            else
            {
                if (fancyLines[_i].PointStart == _targetPoint)
                {
                    fancyLines[_i].PointStart.Destroyed = true;
                    fancyLines[_i].PointStart.LinePrevious.PointEnd = fancyLines[_i].PointEnd;
                    fancyLines[_i].PointEnd.LinePrevious = fancyLines[_i].PointStart.LinePrevious;
                    fancyLines.RemoveAt(_i);
                    return;
                }
            }
        }
        if (fancyLines[fancyLines.Count - 1].PointEnd == _targetPoint)
        {
            fancyLines[fancyLines.Count - 1].PointEnd.Destroyed = true;
            fancyLines[fancyLines.Count - 1].PointStart.LineNext = null;
            fancyLines.RemoveAt(fancyLines.Count - 1);
        }
    }

    public void AddNewLine(FancyPoint _newPoint, bool _doBending, float _colliderWidth, float _lengthBetweenDots)
    {
        FancyLine _line = new FancyLine(
                fancyLines[fancyLines.Count - 1].PointEnd, 
                _newPoint, 
                _doBending, 
                _colliderWidth, 
                _lengthBetweenDots);
        fancyLines[fancyLines.Count - 1].PointEnd.LineNext = _line;
        _line.PointStart.LineNext = _line;
        _line.PointEnd.LinePrevious = _line;
        fancyLines.Add(_line);
    }

    public void InsertNewLine (int _index, FancyPoint _newPoint, bool _doBending, float _colliderWidth, float _lengthBetweenDots)
    {
        FancyLine _line = new FancyLine(
                _newPoint, 
                fancyLines[_index].PointStart, 
                _doBending, 
                _colliderWidth, 
                _lengthBetweenDots);
        _line.PointStart.LineNext = _line;
        _line.PointEnd.LinePrevious = _line;
        _line.PointEnd.LineNext = fancyLines[_index];
        
        if (_index != 0)
        {
            _line.PointStart.LinePrevious = fancyLines[_index - 1];
            fancyLines[_index - 1].PointEnd.LineNext = _line;
        }
        fancyLines.Insert(_index, _line);
    }

    public FancyLine PointInColliders(Vector2 _checkingPoint)
    {
        for (int _i = 0; _i < fancyLines.Count; ++_i)
        {
            if (fancyLines[_i].PointInCollider(_checkingPoint))
            {
                return fancyLines[_i];
            }
        }
        return null;
    }

    public List<Vector2[]> GetColliderPositions ()
    {
        List<Vector2[]> _colliders = new List<Vector2[]>();
        for (int _i = 0; _i < fancyLines.Count; ++_i)
        {
            Vector2[] _colliderPoints = fancyLines[_i].GetColliderPositions();
            if (fancyLines[_i].DoBending)
            {
                _colliders.Add(new Vector2[] { _colliderPoints[0], _colliderPoints[1], _colliderPoints[2], _colliderPoints[3] });
                _colliders.Add(new Vector2[] { _colliderPoints[4], _colliderPoints[5], _colliderPoints[6], _colliderPoints[7] });
            }
            else
            {
                _colliders.Add(_colliderPoints);
            }
        }
        return _colliders;
    }

    public void InjectFancyLineObject (GameObject _fancyLineGameObject)
    {
        if (accessor != null) return;
        accessor = _fancyLineGameObject;
        lineRenderer = accessor.GetComponent<LineRenderer>();
        SetRendererPoints();
    }
    #endregion
}

public class RiverManager
{
    #region FIELDS
    List<FancyLineCollection> rivers;
    List<FancyLineCollection> lineInjectorStorage;
    bool debugMode;
    #endregion

    #region PROPERTIES
    /// <summary>
    ///     Toggles on / off debug mode for the rivers.
    /// </summary>
    public void ToggleDebugMode ()
    {
        debugMode = !debugMode;
    }

    /// <summary>
    ///     Whether or not river colliders should be shown for debugging.
    /// </summary>
    public bool DebugMode
    {
        get { return debugMode; }
    }
    #endregion

    #region CONSTRUCTORS
    public RiverManager (List<FancyLineCollection> _lineInjectorStorage, List<Vector2[]> _riverVectors, 
                         float _width, float _colliderWidth, float _lengthBetweenDots)
    {
        // STEP 1:
        // Create each river's line.
        rivers = new List<FancyLineCollection>();
        for (int _i = 0; _i < _riverVectors.Count; ++_i)
        {
            // Establish FancyLineCollection
            for (int _j = 0; _j < _riverVectors[_i].Length; ++_j)
            {
                if (_j == 0)
                {
                    rivers.Add(
                            new FancyLineCollection(
                                new FancyLine(
                                    new FancyPoint(_riverVectors[_i][_j]),
                                    new FancyPoint(_riverVectors[_i][_j + 1]),
                                    false, _colliderWidth, _lengthBetweenDots),
                                _width));
                    _lineInjectorStorage.Add(rivers[_i]);
                }
                else if (_j > 1)
                {
                    rivers[_i].AddNewLine(new FancyPoint(_riverVectors[_i][_j]), false, _colliderWidth, _lengthBetweenDots);
                }
            }
        }
    }

    public RiverManager (List<FancyLineCollection> _lineInjectorStorage, Vector2[] _riverVectors, 
                         float _width, float _colliderWidth, float _lengthBetweenDots)
    {
        // STEP 1:
        // Create each river's line.
        rivers = new List<FancyLineCollection>();
        for (int _i = 0; _i < _riverVectors.Length; ++_i)
        {
            if (_i == 0)
            {
                rivers.Add(
                        new FancyLineCollection(
                            new FancyLine(
                                new FancyPoint(_riverVectors[_i]),
                                new FancyPoint(_riverVectors[_i + 1]),
                                false, _colliderWidth, _lengthBetweenDots),
                            _width));
                _lineInjectorStorage.Add(rivers[0]);
            }
            else if (_i > 1)
            {
                rivers[0].AddNewLine(new FancyPoint(_riverVectors[_i]), false, _colliderWidth, _lengthBetweenDots);
            }
        }
    }
    #endregion

    #region METHODS
    public bool PointOverlapWithRiver(Vector2 _checkingPoint)
    {
        for (int _i = 0; _i < rivers.Count; ++_i)
        {
            if (rivers[_i].PointInColliders(_checkingPoint) != null) return true;
        }
        return false;
    }

    public List<Vector2[]> GetRiverColliders()
    {
        List<Vector2[]> _riverColliders = new List<Vector2[]>();
        for (int _i = 0; _i < rivers.Count; ++_i)
        {
            _riverColliders.AddRange(rivers[_i].GetColliderPositions());
        }
        return _riverColliders;
        
    }
    #endregion
}

public class StationLineManager
{
    #region FIELDS
    private MouseData mouseData;
    private FancyLineCollection[] subwayLines;
    private int usableLines;
    private int usedLines;
    private List<Station> stations;
    private List<Station>[] shapeStations;
    private bool debugMode;

    private float lengthBetweenDots = 0.4f;
    private float colliderWidth = 1.0f;

    /*private FancyLineCollection selectedLineCollection;
    private FancyLine selectedLine;
    private Station selectedStartingStation;*/
    private FancyLineCollection mouseCollection;
    private FancyPoint mousePoint;
    private Station mouseStationStart;
    private bool mouseSelected;

    private bool mouseBetweenTwoStations;

    private List<FancyLineCollection> lineInjectorStorage;

    private float width;
    #endregion

    #region PARAMETERS
    /// <summary>
    ///     Whether or not station and line colliders should be shown for debugging.
    /// </summary>
    public bool DebugMode
    {
        get { return debugMode; }
    }

    public List<Station> Stations { get { return stations; } }
    #endregion

    #region CONSTRUCTORS
    public StationLineManager (MouseData _mouseData, StationGrid _stationGrid, List<FancyLineCollection> _lineInjectorStorage,
                               int _usableLines)
    {
        mouseData = _mouseData;
        subwayLines = new FancyLineCollection[7];
        stations = _stationGrid.Stations;
        shapeStations = _stationGrid.ShapeStations;
        lineInjectorStorage = _lineInjectorStorage;
        width = 1.0f;
        for (int _i = 0; _i < 7; ++_i)
        {
            subwayLines[_i] = new FancyLineCollection(width);
            lineInjectorStorage.Add(subwayLines[_i]);
        }
        mouseSelected = false;
        mouseBetweenTwoStations = false;
        usableLines = _usableLines;
        usedLines = 0;
    }
    #endregion

    #region METHODS
    public void InputHandling()
    {
        if (mouseSelected)
        {
            if (mouseBetweenTwoStations)
            {

            }
            else
            {

            }
        }
        else
        {
            if (mouseData.LeftPressed)
            {
                // STEP 1:
                // Check if a station was clicked.
                mouseStationStart = StationCollisionCheck();

                if (mouseStationStart != null && usedLines < usableLines)
                {
                    // Found a station, establish line starting from it.
                    subwayLines[usedLines].AddFirstLine(
                            new FancyPoint())
                    mouseBetweenTwoStations = false;
                }

                // STEP 2:
                // Check if a 'T' was clicked.


                // STEP 3:
                // Check if a line was clicked.
            }
        }


        if (selectedLine == null)
        {
            if (mouseData.LeftPressed)
            {
                selectedStartingStation = StationCollisionCheck();

                if (selectedStartingStation != null && linesUsed < linesTotal)
                {



                    if (subwayLines[linesUsed] == null)
                    {

                        /*
                        selectedLine = new FancyLine(
                            new FancyPoint(new Vector2(selectedStartingStation.X, selectedStartingStation.Y)),
                            new FancyPoint(new Vector2(mouseData.X, mouseData.Y)),
                            true,
                            width * 2.0f,
                            1.0f);
                        selectedLine.PointStart.LinkedStation = selectedStartingStation;
                        selectedLineCollection = new FancyLineCollection(selectedLine, width);
                        lineInjectorStorage.Add(selectedLineCollection);
                        subwayLines[linesUsed] = selectedLineCollection;
                        linesUsed++;
                        */
                    }
                    else
                    {
                        /*subwayLines[linesUsed].LineVisible(true);
                        selectedLineCollection = subwayLines[linesUsed];
                        selectedLine = subwayLines[linesUsed].FancyLines[subwayLines[linesUsed].FancyLines.Count - 1];*/
                    }
                    
                }
            }
        }
        else
        {
            // Debug.Log(mouseData.Position);
            // Debug.Log(selectedLine.PointEnd.Point);
            
            /*
            Station _hoveringStation = StationCollisionCheck();

            if (_hoveringStation == null)
            {
                selectedLine.VectorPointEnd = mouseData.Position;
                selectedLineCollection.SetRendererPoints();
            }
            else
            {
                // Check if station is in the line.
                if (!StationInLine(selectedLineCollection, _hoveringStation))
                {
                    selectedLine.VectorPointEnd = _hoveringStation.Position;
                    selectedLine.PointEnd.LinkedStation = _hoveringStation;
                    selectedLineCollection.AddNewLine();
                    selectedLineCollection.SetRendererPoints();
                }
                
            }

            if (mouseData.LeftReleased)
            {
                
                if (selectedLine.PointStart.LinkedStation == selectedStartingStation)
                {
                    // Full line wasn't drawn.
                    subwayLines[linesUsed].LineVisible(false);
                    linesUsed--;
                }
                else
                {

                }
            }

            */
        }
    }

    private bool StationInLine(FancyLineCollection _lineCollection, Station _station)
    {
        for (int _i = 0; _i < _lineCollection.FancyLines.Count; ++_i)
        {
            if (_lineCollection.FancyLines[_i].PointStart.LinkedStation == _station) return true;
        }
        return (_lineCollection.FancyLines[_lineCollection.FancyLines.Count - 1].PointEnd.LinkedStation == _station);
    }

    public Station StationCollisionCheck ()
    {
        for (int _i = 0; _i < stations.Count; ++_i)
        {
            if (mouseData.X > stations[_i].ClickCollider.x &&
                mouseData.X < stations[_i].ClickCollider.x + stations[_i].ClickCollider.width &&
                mouseData.Y > stations[_i].ClickCollider.y &&
                mouseData.Y < stations[_i].ClickCollider.y + stations[_i].ClickCollider.height)
            {
                return stations[_i];
            }
        }
        return null;
    }

    /// <summary>
    ///     Toggles on / off debug mode for station line relevant colliders.
    /// </summary>
    public void ToggleDebugMode ()
    {
        debugMode = !debugMode;
    }
    #endregion
}

public class MouseData
{
    #region FIELDS
    private Vector2 position;
    private bool leftPressed;
    private bool leftReleased;
    #endregion

    #region PARAMETERS
    public float X { get { return position.x; } }
    public float Y { get { return position.y; } }
    public Vector2 Position { get { return position; } }
    public bool LeftPressed { get { return leftPressed; } }
    public bool LeftReleased { get { return leftReleased; } }
    #endregion

    #region CONSTRUCTORS
    public MouseData ()
    {
        position = Vector2.zero;
        leftPressed = false;
        leftReleased = false;
    }
    #endregion

    #region METHODS
    public void SetData()
    {
        // Handle Mouse Position
        position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Buttons pressed.
        if (leftPressed) leftPressed = false;
        if (leftReleased) leftReleased = false;
        if (Input.GetMouseButtonDown(0)) leftPressed = true;
        if (Input.GetMouseButtonUp(0)) leftReleased = true;
    }
    #endregion
}
public class MainManager : MonoBehaviour
{
    #region Static Instance
    public static MainManager Instance { get; private set; }

    private void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple Main Manager instances found!");
            Destroy(gameObject);
        }
    }
    #endregion

    #region FIELDS

    // Serialized Elements
    [SerializeField] private GameObject stationPrefab;
    [SerializeField] private GameObject fancyLinePrefab;

    [SerializeField]
    private Sprite[] stationSprites = new Sprite[(int)STATION_SHAPE.LENGTH];

    [SerializeField] private int startingStationTotal = 2;

    [SerializeField] private int stationMaximumTotal = 10;

    // Key element storage systems
    private List<Timer> timers;
    private CameraData cameraData;
    private StationGrid stationGrid;

    private bool isGameRunning = false;
    public bool isGamePaused = false; // Add this to track pause state

    //Camera Zoom Out
    [SerializeField]
    private float maxCameraTime = 600f;

    private Timer cameraZoomOutTime;

    //Station Spawning
    [SerializeField]
    private float timeBetweenStationSpawns = 15f;

    private Timer stationSpawnTimer;

    // Line handling
    List<FancyLineCollection> lineInjectorStorage;
    int linesInjected = 0;

    // Mouse Handling
    MouseData mouseData;

    RiverManager riverManager;
    StationLineManager stationLineManager;

    // Train Resources
    public int availableTrains = 3; // Example starting value

    #endregion

    #region UNITY MONOBEHAVIOR

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start ()
    {
        // STEP 1:
        // Insert data into generic fields.
        isGameRunning = true;
        isGamePaused = false; // Initialize pause state
        cameraData = new CameraData(Camera.main, 5.0f, 10.0f);
        cameraData.ToggleDebugMode();
        timers = new List<Timer>();
        lineInjectorStorage = new List<FancyLineCollection>();

        // STEP 2:
        // Create Rivers and inject the line prefab so they properly appear.
        Vector2[] _settingRiverVectors = 
        { 
            new Vector2 (cameraData.MaxLeft, cameraData.Top * 0.6f),
            new Vector2 (cameraData.PositionX, cameraData.Top * 0.4f),
            new Vector2 (cameraData.MaxRight, cameraData.Top * 0.6f)
        };
        riverManager = new RiverManager(
                lineInjectorStorage,
                _settingRiverVectors,
                0.7f, 1.5f, 2.0f);
        riverManager.ToggleDebugMode();
        for (int _i = 0; _i < lineInjectorStorage.Count; ++_i)
        {
            lineInjectorStorage[_i].InjectFancyLineObject(Instantiate(fancyLinePrefab, Vector3.zero, Quaternion.identity));
        }
        linesInjected = lineInjectorStorage.Count;

        // STEP 3:
        // Create starting stations.
        stationGrid = new StationGrid(cameraData, 1.0f, startingStationTotal, stationMaximumTotal, 0.9f, 3.0f, riverManager);
        stationGrid.ToggleDebugMode();
        for (int _i = 0; _i < startingStationTotal; ++_i)
        {
            CreateStation();
        }

        // STEP 4:
        // Set variables for camera size change  and station spawning.
        cameraZoomOutTime = CreateTimer(maxCameraTime, true);
        stationSpawnTimer = CreateTimer(timeBetweenStationSpawns, false);

        // STEP 5:
        // Mouse Setup for input on stations.
        mouseData = new MouseData();

        // STEP 6:
        // Establish the stationLineManager
        stationLineManager = new StationLineManager(mouseData, stationGrid, lineInjectorStorage);
        stationLineManager.ToggleDebugMode();
    }

    // Update is called once per frame
    private void Update ()
    {
        // STEP 1: 
        // Set mouse info to be used.
        mouseData.SetData();

        // STEP 2:
        // Handled time based features.
        if (!isGamePaused) // Only update game elements if not paused
        {
            // Increment all existing Timer class instances.
            foreach (Timer timer in timers)
            {
                timer.IncrementTimer(Time.deltaTime);
            }

            // Camera scaling and spawing of stations over time.
            cameraData.UpdateCameraSize(cameraZoomOutTime.TimerPercentage);
            if (stationSpawnTimer.Trigger) CreateStation();
        }

        // STEP 3:
        // Player input for lines
        stationLineManager.InputHandling();

        // STEP 4:
        // Line object placing
        while (linesInjected < lineInjectorStorage.Count)
        {
            lineInjectorStorage[linesInjected].InjectFancyLineObject(Instantiate(fancyLinePrefab, Vector3.zero, Quaternion.identity));
            linesInjected++;
        }
    }

    #endregion

    #region METHODS

    /// <summary>
    ///     Creates a station and instantiates its associated object in the scene.
    /// </summary>
    private void CreateStation ()
    {
        Station _station = stationGrid.CreateStation();
        if (_station != null)
        {
            _station.InjectStationObject(
                Instantiate(stationPrefab, Vector3.zero, Quaternion.identity),
                stationSprites[(int)_station.Shape]);
        }
    }

    /// <summary>
    ///     Creates a new Timer and adds it to the timers list
    /// </summary>
    /// <param name="_length"> Length of the timer. </param>
    /// <param name="_runsOnce"> Whether or not the timer will run once or will loop. </param>
    /// <returns> Returns the newly created Timer. </returns>
    private Timer CreateTimer (float _length, bool _runsOnce)
    {
        Timer _timer = new Timer(_length, _runsOnce);
        timers.Add(_timer);
        return _timer;
    }

    // Methods to control game state from UI
    public void PauseGame ()
    {
        isGamePaused = true;
        Time.timeScale = 0f; // Stop time for game elements
        Debug.Log("Game Paused");
    }

    public void ResumeGame ()
    {
        isGamePaused = false;
        Time.timeScale = 1f; // Resume normal time
        Debug.Log("Game Resumed");
    }

    public void ExitGame ()
    {
        Debug.Log("Exiting Game...");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void SelectColor (int colorIndex)
    {
        Debug.Log("Selected color index: " + colorIndex);
        // Implement logic for selecting this color for line drawing
    }

    public void SelectTunnel ()
    {
        Debug.Log("Selected Tunnel");
        // Implement logic for selecting the tunnel tool
    }

    public void SelectTrain ()
    {
        Debug.Log("Selected Train");
        // Implement logic for selecting the train tool
    }

    /// <summary>
    ///     Sets the game speed.
    /// </summary>
    /// <param name="speed">The speed multiplier (1f = normal, 2f = double speed, etc.).</param>
    public void SetGameSpeed (float speed)
    {
        Time.timeScale = speed;
        Debug.Log("Game speed set to: " + speed);
    }

    // Method to get available train count
    public int GetAvailableTrains ()
    {
        return availableTrains;
    }

    // Method to reduce available train count
    public void UseTrain ()
    {
        availableTrains--;
        if (availableTrains < 0)
        {
            availableTrains = 0; // Prevent going below zero
        }

        Debug.Log("Train used. Available trains: " + availableTrains);
    }

    // Method to increase available train count (if needed)
    public void AddTrain (int amount)
    {
        availableTrains += amount;
        Debug.Log("Trains added. Available trains: " + availableTrains);
    }
    #endregion
    
    #region GIZMOS
    private void OnDrawGizmos ()
    {
        if (!isGameRunning)
        {
            return;
        }
        
        // Show camera size.
        if (cameraData.DebugMode)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(
                new Vector3(cameraData.Left, cameraData.Top, 0),
                new Vector3(cameraData.Left, cameraData.Bottom, 0));
            Gizmos.DrawLine(
                new Vector3(cameraData.Right, cameraData.Top, 0),
                new Vector3(cameraData.Right, cameraData.Bottom, 0));
            Gizmos.DrawLine(
                new Vector3(cameraData.Left, cameraData.Top, 0),
                new Vector3(cameraData.Right, cameraData.Top, 0));
            Gizmos.DrawLine(
                new Vector3(cameraData.Left, cameraData.Bottom, 0),
                new Vector3(cameraData.Right, cameraData.Bottom, 0));
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

        if (riverManager.DebugMode)
        {
            List<Vector2[]> _colliders = riverManager.GetRiverColliders();
            Gizmos.color = Color.red;
            for (int _i = 0; _i < _colliders.Count; ++_i)
            {
                Gizmos.DrawLine(
                    _colliders[_i][0],
                    _colliders[_i][1]);
                Gizmos.DrawLine(
                    _colliders[_i][1],
                    _colliders[_i][2]);
                Gizmos.DrawLine(
                    _colliders[_i][2],
                    _colliders[_i][3]);
                Gizmos.DrawLine(
                    _colliders[_i][3],
                    _colliders[_i][0]);
            }
        }

        if (stationLineManager.DebugMode)
        {
            Gizmos.color = Color.white;
            for (int _i = 0; _i < stationLineManager.Stations.Count; ++_i)
            {
                Gizmos.DrawLine(
                        new Vector3 (
                            stationLineManager.Stations[_i].ClickCollider.x, 
                            stationLineManager.Stations[_i].ClickCollider.y, 0),
                        new Vector3(
                            stationLineManager.Stations[_i].ClickCollider.x + stationLineManager.Stations[_i].ClickCollider.width,
                            stationLineManager.Stations[_i].ClickCollider.y, 0));
                Gizmos.DrawLine(
                        new Vector3(
                            stationLineManager.Stations[_i].ClickCollider.x + stationLineManager.Stations[_i].ClickCollider.width,
                            stationLineManager.Stations[_i].ClickCollider.y, 0),
                        new Vector3(
                            stationLineManager.Stations[_i].ClickCollider.x + stationLineManager.Stations[_i].ClickCollider.width,
                            stationLineManager.Stations[_i].ClickCollider.y + stationLineManager.Stations[_i].ClickCollider.height, 0));
                Gizmos.DrawLine(
                        new Vector3(
                            stationLineManager.Stations[_i].ClickCollider.x + stationLineManager.Stations[_i].ClickCollider.width,
                            stationLineManager.Stations[_i].ClickCollider.y + stationLineManager.Stations[_i].ClickCollider.height, 0),
                        new Vector3(
                            stationLineManager.Stations[_i].ClickCollider.x,
                            stationLineManager.Stations[_i].ClickCollider.y + stationLineManager.Stations[_i].ClickCollider.height, 0));
                Gizmos.DrawLine(
                        new Vector3(
                            stationLineManager.Stations[_i].ClickCollider.x,
                            stationLineManager.Stations[_i].ClickCollider.y + stationLineManager.Stations[_i].ClickCollider.height, 0),
                        new Vector3(
                            stationLineManager.Stations[_i].ClickCollider.x,
                            stationLineManager.Stations[_i].ClickCollider.y, 0));
            }
        }
    }
    #endregion
}