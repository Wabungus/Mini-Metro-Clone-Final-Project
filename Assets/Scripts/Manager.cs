using NUnit.Framework;
using System.Collections.Generic;
using TMPro.SpriteAssetUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

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

#region Basic Classes
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
    ///     Whether or not the camera is currently in debug mode.
    /// </summary>
    public bool DebugMode
    {
        get { return debugMode; }
        set
        {
            debugMode = value;
            UpdateCameraSize(currentPercent);
        }
    }

    /// <summary>
    ///     The actual Camera object.
    /// </summary>
    public Camera Accessor
    {
        get { return camera; }
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
    public CameraData(Camera _camera, float _sizeStart, float _sizeEnd)
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
    public void UpdateCameraSize(float _percentSize)
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
    public Timer(float _length, bool _runsOnce)
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
    public void IncrementTimer(float _deltaTime)
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
    public void SetData ()
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

public class RectCollider
{
    #region FIELDS
    private float left;
    private float right;
    private float top;
    private float bottom;
    Vector2 topLeft;
    Vector2 bottomRight;
    #endregion

    #region PROPERTIES
    /// <summary>
    /// The left x position of the collider
    /// </summary>
    public float Left { get { return left; } }
    /// <summary>
    /// The right x position of the collider
    /// </summary>
    public float Right { get { return right; } }
    /// <summary>
    /// The top y position of the collider
    /// </summary>
    public float Top { get { return top; } }
    /// <summary>
    /// The bottom y position of the collider
    /// </summary>
    public float Bottom { get { return bottom; } }
    public Vector2 TopLeft { get { return topLeft; } }
    public Vector2 BottomRight { get { return bottomRight; } }
    #endregion

    #region CONSTRUCTOR
    public RectCollider (float _left, float _right, float _top, float _bottom)
    {
        left = _left;
        right = _right;
        top = _top;
        bottom = _bottom;
        topLeft = new Vector2(_left, _top);
        bottomRight = new Vector2(_right, _bottom);
    }
    #endregion

    #region METHODS
    public bool PositionInCollider (Vector2 _position)
    {
        return (_position.x >= left && _position.x <= right && _position.y >= bottom && _position.y <= top);
    }
    #endregion
}

public class AngledRectCollider
{
    #region FIELDS
    private Vector2[] points;
    private Vector2[] lineSubtracts;
    private float[] lineAs;
    #endregion

    #region PROPERTIES
    /// <summary>
    /// The position of pointA.
    /// </summary>
    public Vector2 PointA { get { return points[0]; } }
    /// <summary>
    /// The position of pointB.
    /// </summary>
    public Vector2 PointB { get { return points[1]; } }
    /// <summary>
    /// The position of pointC.
    /// </summary>
    public Vector2 PointC { get { return points[2]; } }
    /// <summary>
    /// The position of pointD.
    /// </summary>
    public Vector2 PointD { get { return points[3]; } }
    #endregion

    #region CONSTRUCTOR
    public AngledRectCollider (Vector2 _pointA, Vector2 _pointB, Vector2 _pointC, Vector2 _pointD)
    {
        points = new Vector2[] { _pointA, _pointB, _pointC, _pointD, _pointA };

        /*lineSubtracts = new Vector2[4];
        lineSubtracts[0] = new Vector2(_pointB.x - _pointA.x, _pointB.y - _pointA.y);
        lineSubtracts[1] = new Vector2(_pointC.x - _pointB.x, _pointC.y - _pointB.y);
        lineSubtracts[2] = new Vector2(_pointD.x - _pointC.x, _pointD.y - _pointC.y);
        lineSubtracts[3] = new Vector2(_pointA.x - _pointD.x, _pointA.y - _pointD.y);

        lineAs = new float[4];
        lineAs[0] = points[0].y * lineSubtracts[0].x - points[0].x * lineSubtracts[0].y;
        lineAs[1] = points[1].y * lineSubtracts[1].x - points[1].x * lineSubtracts[1].y;
        lineAs[2] = points[2].y * lineSubtracts[2].x - points[2].x * lineSubtracts[2].y;
        lineAs[3] = points[3].y * lineSubtracts[3].x - points[3].x * lineSubtracts[3].y;*/
    }
    #endregion

    #region METHODS
    /// <summary>
    /// Check whether a point is within the 4 sided polygon by drawing a line from the point
    /// to the origin (0,0). If an odd number of collisions was found then the line is
    /// within the polygon.
    /// </summary>
    /// <param name="_position">The position to check against the collider.</param>
    /// <returns>Returns a bool of whether the collision was a success.</returns>
    public bool PositionInCollider (Vector2 _position)
    {
        int _successes = 0;
        for (int _i = 0; _i < 4; ++_i)
        {
            float _a = (points[_i + 1].x - points[_i].x) * (points[_i].y - 0) - (points[_i + 1].y - points[_i].y) * (points[_i].x - 0);
            float _b = (points[_i + 1].x - points[_i].x) * (_position.y - 0) - (points[_i + 1].y - points[_i].y) * (_position.x - 0);
            float _c = (_position.x - 0) * (points[_i].y - 0) - (_position.y - 0) * (points[_i].x - 0);
            if (_b != 0.0f && _a / _b > 0 && _a / _b < 1 && _a / _c > 0 && _c / _b < 1) ++_successes;
        }
        return (_successes == 1 || _successes == 3);
    }
    #endregion
}
#endregion

public class EndT
{
    #region FIELDS
    private GameObject accessor;
    private AngledRectCollider collider;
    private bool isVisible;
    private Color color;
    
    #endregion

    #region PROPERTIES
    
    #endregion

    #region CONSTRUCTOR
    public EndT (Color _color)
    {
        accessor = null;
        collider = null;
        isVisible = false;
        color = _color;
    }
    #endregion

    #region METHODS
    public bool WithinCollider (Vector2 _point)
    {
        return collider.PositionInCollider(_point);
    }

    public void TiedLine ()
    {

    }

    public void InjectEndT(GameObject _object)
    {
        accessor = _object;
        collider = new AngledRectCollider(
                new Vector2(accessor.transform.position.x - 0.2f, accessor.transform.position.y - 0.2f),
                new Vector2(accessor.transform.position.x - 0.2f, accessor.transform.position.y + 0.2f),
                new Vector2(accessor.transform.position.x + 0.2f, accessor.transform.position.y + 0.2f),
                new Vector2(accessor.transform.position.x + 0.2f, accessor.transform.position.y - 0.2f));
        accessor.GetComponent<SpriteRenderer>().color = color;
    }
    #endregion
}

public class Station
{
    #region FIELDS
    private bool debugMode;
    private GameObject accessor;
    private STATION_SHAPE shape;
    private Vector2 position;

    private Point[,] pointConnections;
    private Vector2[,] accessConnections;
    private List<Point> totalPoints;

    private RectCollider collider;
    #endregion

    #region PROPERTIES
    /// <summary>
    /// Enables debug viewing for this station.
    /// </summary>
    public bool DebugMode { get { return debugMode; } set { debugMode = value; } }
    /// <summary>
    /// Allows direct access to the positions used for placing points.
    /// </summary>
    public Vector2[,] AccessConnections { get { return accessConnections; } }

    #region OBJECT DATA
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
    ///     The position of this station object.
    /// </summary>
    public Vector2 StationTruePosition { get { return position; } }

    /// <summary>
    ///     The x & y scale of the station GameObject (effects SpriteRenderer).
    /// </summary>
    public float Scale
    {
        get { return accessor.transform.localScale.x; }
        set { accessor.transform.localScale.Set(value, value, 1); }
    }
    #endregion
    #endregion

    #region CONSTRUCTORS
    /// <summary>
    ///     Creates a Station class instance, containing shortcuts to its associated station GameObject.
    /// </summary>
    /// <param name="_position">Where to place the station object once it is created.</param>
    /// <param name="_shape">The shape for this station, converts to enum format internally.</param>
    public Station(Vector2 _position, int _shape)
    {
        position = _position;
        shape = (STATION_SHAPE)_shape;
        pointConnections = new Point[8, 3];
        accessConnections = new Vector2[8, 3];
        totalPoints = new List<Point>();

        // Define the 24 points for station access.
        float _length = 0.2f;
        float _offsetLength = 0.1f;
        for (int _i = 0; _i < 8; ++_i)
        {
            accessConnections[_i, 0] = new Vector2(
                    position.x + _length * Mathf.Cos((Mathf.PI / 4) * _i), 
                    position.y + _length * Mathf.Sin((Mathf.PI / 4) * _i));
            accessConnections[_i, 1] = new Vector2(
                    accessConnections[_i, 0].x + _offsetLength * Mathf.Cos((Mathf.PI / 4) * _i + (Mathf.PI / 2)),
                    accessConnections[_i, 0].y + _offsetLength * Mathf.Sin((Mathf.PI / 4) * _i + (Mathf.PI / 2)));
            accessConnections[_i, 2] = new Vector2(
                    accessConnections[_i, 0].x + _offsetLength * Mathf.Cos((Mathf.PI / 4) * _i - (Mathf.PI / 2)),
                    accessConnections[_i, 0].y + _offsetLength * Mathf.Sin((Mathf.PI / 4) * _i - (Mathf.PI / 2)));
        }
        debugMode = true;

        // Set the collider
        collider = new RectCollider(_position.x - 0.4f, _position.x + 0.4f, _position.y + 0.4f, _position.y - 0.4f);
    }
    #endregion

    #region METHODS
    /// <summary>
    /// Whether or not a given position in the scene is within the station's collider.
    /// </summary>
    /// <param name="_position">The position to check.</param>
    /// <returns>Whether or not the position was within the collider.</returns>
    public bool PositionInStationCollider(Vector2 _position)
    {
        return collider.PositionInCollider(_position);
    }

    /// <summary>
    ///     Returns the position of an access connection using a given angle's index and access slot.
    /// </summary>
    /// <param name="_accessAngleIndex">The angle's index to access through.</param>
    /// <param name="_accessSlot">The slot in a set angle's index to access.</param>
    /// <returns>The position to access.</returns>
    public Vector2 GetAccessConnection(int _accessAngleIndex, int _accessSlot)
    {
        return accessConnections[_accessAngleIndex, _accessSlot];
    }

    /// <summary>
    ///     Returns the point in a connection using a given angle's index and access slot.
    /// </summary>
    /// <param name="_accessAngleIndex">The angle's index to access through.</param>
    /// <param name="_accessSlot">The slot in a set angle's index to access.</param>
    /// <returns>The point to access.</returns>
    public Point GetPointConnection (int _accessAngleIndex, int _accessSlot)
    {
        return pointConnections[_accessAngleIndex, _accessSlot];
    }

    /// <summary>
    ///     Returns the point in a connection using a given angle's index and access slot.
    /// </summary>
    /// <param name="_accessAngleIndex">The angle's index to access through.</param>
    /// <param name="_accessSlot">The slot in a set angle's index to access.</param>
    /// <param name="_point">The point to set here.</param>
    public void SetPointConnection (int _accessAngleIndex, int _accessSlot, Point _point)
    {
        pointConnections[_accessAngleIndex, _accessSlot] = _point;
    }

    /// <summary>
    ///     Injects the action GameObject of the station in since it can only be created in the main class.
    /// </summary>
    /// <param name="_stationGameObject">Station GameObject to inject</param>
    /// <param name="_icon">The sprite to use for this game object</param>
    public void InjectStationObject(GameObject _stationGameObject, Sprite _icon)
    {
        if (accessor != null) return;
        accessor = _stationGameObject;
        accessor.transform.position = position;
        accessor.GetComponent<SpriteRenderer>().sprite = _icon;
    }
    #endregion
}

public class Point
{
    #region FIELDS
    private List<Point> line;
    private Station tiedStation;

    /// <summary>
    /// Whether or not this point is the one held by the mouse.
    /// </summary>
    private bool isMouse;

    /// <summary>
    /// The angle which is a multiple of 45 degrees through which the line this point is a part of enters its station.
    /// </summary>
    private int entryAngleIndex;
    /// <summary>
    /// The slot of 3 in an access angle through which the line this point is a part of enters its station.
    /// </summary>
    private int entrySlot;
    /// <summary>
    /// The actual angle used when entering for this point.
    /// </summary>
    private int entryAngleTrue;

    /// <summary>
    /// The angle which is a multiple of 45 degrees through which the line this point is a part of exits its station.
    /// </summary>
    private int exitAngleIndex;
    /// <summary>
    /// The slot of 3 in an access angle through which the line this point is a part of exits its station.
    /// </summary>
    private int exitSlot;
    /// <summary>
    /// The actual angle used when exiting for this point.
    /// </summary>
    private int exitAngleTrue;

    /// <summary>
    /// The angle from this point's station to the mouse position.
    /// </summary>
    private float mouseAngleTrue;
    /// <summary>
    /// The previous angle from this point's station to the mouse.
    /// Used for calculations of whether lines should bend left or right.
    /// </summary>
    private float mouseAngleOld;
    /// <summary>
    /// The mouse angle converted to be a multiple of 45. 
    /// Set in reference to the entry / exit angle.
    /// </summary>
    private int mouseAngleFixed;
    #endregion

    #region PROPERTIES
    /// <summary>
    /// The line this point falls within.
    /// </summary>
    public List<Point> Line { get { return line; } }
    /// <summary>
    /// The station this line is tied to.
    /// </summary>
    public Station TiedStation { get { return tiedStation; } }

    /// <summary>
    /// Whether or not this point is the one held by the mouse.
    /// </summary>
    public bool IsMouse { get { return isMouse; } set { isMouse = value; } }

    #region ENTRY & EXITS
    /// <summary>
    /// The angle which is a multiple of 45 degrees through which the line this point is a part of enters its station.
    /// </summary>
    public int EntryAngleIndex { get { return entryAngleIndex; } set { entryAngleIndex = value; } }
    /// <summary>
    /// The slot of 3 in an access angle through which the line this point is a part of enters its station.
    /// </summary>
    public int EntrySlot { get { return entrySlot; } set { entrySlot = value; } }
    /// <summary>
    /// The actual angle used when entering for this point.
    /// </summary>
    public float EntryAngleTrue { get { return entryAngleTrue; } set { entryAngleTrue = value; } }

    /// <summary>
    /// The angle which is a multiple of 45 degrees through which the line this point is a part of exits its station.
    /// </summary>
    public int ExitAngleIndex { get { return exitAngleIndex; } set { exitAngleIndex = value; } }
    /// <summary>
    /// The slot of 3 in an access angle through which the line this point is a part of exits its station.
    /// </summary>
    public int ExitSlot { get { return exitSlot; } set { exitSlot = value; } }
    /// <summary>
    /// The actual angle used when exiting for this point.
    /// </summary>
    public float ExitAngleTrue { get { return exitAngleTrue; } set { exitAngleTrue = value; } }

    /// <summary>
    /// The angle from this point's station to the mouse position.
    /// </summary>
    public float MouseAngleTrue { get { return mouseAngleTrue; } set { mouseAngleTrue = value; } }
    /// <summary>
    /// The previous angle from this point's station to the mouse.
    /// Used for calculations of whether lines should bend left or right.
    /// </summary>
    public float MouseAngleOld { get { return mouseAngleOld; } set { mouseAngleOld = value; } }
    /// <summary>
    /// The mouse angle converted to be a multiple of 45. 
    /// Set in reference to the entry / exit angle.
    /// </summary>
    public int MouseAngleFixed { get { return mouseAngleFixed; } set { mouseAngleFixed = value; } }
    #endregion
    #endregion

    #region CONSTRUCTORS
    /// <summary>
    /// Creates a point and links it up with a station and associated line.
    /// </summary>
    /// <param name="_line">The line this point is within.</param>
    /// <param name="_tiedStation">The station to link up with.</param>
    public Point (List<Point> _line, Station _tiedStation)
    {
        line = _line;
        tiedStation = _tiedStation;
        accessEntryAngleIndex = -1;
        accessEntrySlot = -1;
        accessExitAngleIndex = -1;
        accessExitSlot = -1;
        //tiedStation.SetPoint(accessEntryAngleIndex, this, true);
    }
    /// <summary>
    /// When creating a point at a station with both a known entry and exit.
    /// </summary>
    /// <param name="_line">The line this point is within.</param>
    /// <param name="_tiedStation">The station to link up with.</param>
    /// <param name="_accessEntryAngle">The starting angle to use for this point.</param>
    /// <param name="_accessExitAngle">The angle to leave at for this point.</param>
    public Point (List<Point> _line, Station _tiedStation, int _accessEntryAngle, int _accessExitAngle)
    {
        line = _line;
        tiedStation = _tiedStation;
        accessEntryAngleIndex = _accessEntryAngle;
        accessExitAngleIndex = _accessExitAngle;

    }
    #endregion
}

public class Line
{
    #region FIELDS
    // General variables.
    private GameObject accessor;
    private LineManager manager;
    private MouseData mouseData;
    private List<LineRenderer> renderers;
    private List<Point> points;
    private List<Vector2> pointPositions;
    private List<Vector2> bendPositions;
    private List<AngledRectCollider[]> colliders;

    // Mouse specific.
    /// <summary>
    /// The index into the 'points' list that the player's mouse is located at.
    /// If set to -1 then this line doesn't currently contain the player's mouse.
    /// </summary>
    private int mouseIndex;

    /// <summary>
    /// Whether or not the line formed from the previous point to the mouse should bend right.
    /// For the next point it will bend the opposite way as the previous.
    /// </summary>
    private bool bendRight;

    /// <summary>
    /// The point before the mouse in the 'points' list.
    /// </summary>
    private Point previousPoint;
    /// <summary>
    /// The index of the point before the mouse in the 'points' list.
    /// </summary>
    private int previousPointIndex;

    /// <summary>
    /// The point after the mouse in the 'points' list.
    /// </summary>
    private Point nextPoint;
    /// <summary>
    /// The index of the point after the mouse in the 'points' list.
    /// </summary>
    private int nextPointIndex;
    #endregion

    #region PROPERTIES

    #endregion

    #region CONSTRUCTORS
    public Line(LineManager _manager, MouseData _mouseData)
    {
        accessor = null;
        manager = _manager;
        mouseData = _mouseData;

        // STEP 2:
        // Establish lists.
        renderers = new List<LineRenderer>();
        points = new List<Point> ();
        pointPositions = new List<Vector2> ();
        bendPositions = new List<Vector2> ();

        // STEP 3:
        // Establish mouse specific.
        mouseIndex = -1;
    }
    #endregion

    #region METHODS

    public void CreateLine (Point _pointStart, Point _mousePoint)
    {
        // STEP 1:
        // Add point values.
        points.Add(_pointStart);
        pointPositions.Add(_pointStart.TiedStation.StationTruePosition);

        bendPositions.Add(Vector2.zero);
        colliders.Add(new AngledRectCollider[2] { null, null });

        points.Add(_mousePoint);
        _mousePoint.IsMouse = true;
        pointPositions.Add(mouseData.Position);

        // STEP 2:
        // Do extra calculations.
        
    }

    public void MouseCalculations(Point _point, int _bendIndex, bool _isEntry)
    {
        // Calculate the mouse angle.
        _point.MouseAngleOld = _point.MouseAngleTrue;
        _point.MouseAngleTrue =
                Mathf.Floor(Mathf.Repeat(
                    Mathf.Atan2(
                        _point.TiedStation.StationTruePosition.y
                        - mouseData.Position.y,
                        _point.TiedStation.StationTruePosition.x
                        - mouseData.Position.x)
                    * Mathf.Rad2Deg,
                    360.0f));

        // Determine if bend should be left or right.
        if (bendRight)
        {
            if (Mathf.Floor(_point.MouseAngleOld / 45.0f) < Mathf.Floor(_point.MouseAngleTrue / 45.0f) &&
                !(Mathf.Floor(_point.MouseAngleOld / 45.0f) == 0 && Mathf.Floor(_point.MouseAngleTrue / 45.0f) == 7))
            {
                bendRight = false;
            }
        }
        else
        {
            if (Mathf.Floor(_point.MouseAngleOld / 45.0f) > Mathf.Floor(_point.MouseAngleTrue / 45.0f) &&
                !(Mathf.Floor(_point.MouseAngleOld / 45.0f) == 7 && Mathf.Floor(_point.MouseAngleTrue / 45.0f) == 0))
            {
                bendRight = true;
            }
        }

        // Set values for entry or exit.
        if (_isEntry)
        {
            // Calculate the angle using the mouse position.
            _point.EntryAngleTrue = (int)Mathf.Repeat(Mathf.Floor(_point.MouseAngleTrue / 45.0f) * 45.0f, 360);
            if (Mathf.Abs(_point.MouseAngleTrue - _point.EntryAngleTrue) < 5.0f)
            {
                _point.MouseAngleFixed = (int)_point.EntryAngleTrue;
            }
            else if (_point.EntryAngleTrue == 0)
            {
                _point.MouseAngleFixed = (_point.EntryAngleTrue > 270) ? 315 : 45;
            }
            else
            {
                _point.MouseAngleFixed =
                        (_point.MouseAngleTrue < _point.EntryAngleTrue)
                        ? (int)Mathf.Repeat(_point.EntryAngleTrue - 45.0f, 360)
                        : (int)Mathf.Repeat(_point.EntryAngleTrue + 45.0f, 360);
            }

            // Calculate the entry slot.
            _point.EntryAngleIndex = (int)(_point.EntryAngleTrue / 45.0f);
            for (int _i = 0; _i < 3; ++_i)
            {
                if (_point.TiedStation.AccessConnections[_point.EntryAngleIndex, _i] == null)
                {
                    _point.TiedStation.SetPointConnection(_point.EntryAngleIndex, _i, _point);
                    _point.EntrySlot = _i;
                    break;
                }
            }

            // Calculate bend position.
            float _positionX =
                    (_point.EntryAngleIndex != -1)
                    ? _point.TiedStation.AccessConnections
                        [_point.EntryAngleIndex,
                         _point.EntrySlot].x
                    : _point.TiedStation.StationTruePosition.x;
            float _positionY =
                    (_point.EntryAngleIndex != -1)
                    ? _point.TiedStation.AccessConnections
                        [_point.EntryAngleIndex,
                         _point.EntrySlot].y
                    : _point.TiedStation.StationTruePosition.y;
            if (bendRight)
            {
                if (_point.EntryAngleTrue == 0 && _point.MouseAngleFixed == 45)
                {
                    bendPositions[_bendIndex] =
                            new Vector2(_positionX + Mathf.Abs(mouseData.Y - _positionY), mouseData.Y);
                }
                else if (_point.EntryAngleTrue == 45 && _point.MouseAngleFixed == 90)
                {
                    bendPositions[_bendIndex] =
                            new Vector2(_positionX, mouseData.Y - Mathf.Abs(_positionX - mouseData.X));
                }
                else if (_point.EntryAngleTrue == 90 && _point.MouseAngleFixed == 135)
                {
                    bendPositions[_bendIndex] =
                            new Vector2(mouseData.X, _positionY + Mathf.Abs(mouseData.X - _positionX));
                }
                else if (_point.EntryAngleTrue == 135 && _point.MouseAngleFixed == 180)
                {
                    bendPositions[_bendIndex] =
                            new Vector2(mouseData.X + Mathf.Abs(_positionY - mouseData.Y), _positionY);
                }
                else if (_point.EntryAngleTrue == 180 && _point.MouseAngleFixed == 225)
                {
                    bendPositions[_bendIndex] =
                            new Vector2(_positionX - Mathf.Abs(mouseData.Y - _positionY), mouseData.Y);
                }
                else if (_point.EntryAngleTrue == 225 && _point.MouseAngleFixed == 270)
                {
                    bendPositions[_bendIndex] =
                            new Vector2(_positionX, mouseData.Y + Mathf.Abs(_positionX - mouseData.X));
                }
                else if (_point.EntryAngleTrue == 270 && _point.MouseAngleFixed == 315)
                {
                    bendPositions[_bendIndex] =
                            new Vector2(mouseData.X, _positionY - Mathf.Abs(mouseData.X - _positionX));
                }
                else if (_point.EntryAngleTrue == 315 && _point.MouseAngleFixed == 0)
                {
                    bendPositions[_bendIndex] =
                            new Vector2(mouseData.X - Mathf.Abs(_positionY - mouseData.Y), _positionY);
                }
            }
            else
            {
                if (_point.EntryAngleTrue == 0 && _point.MouseAngleFixed == 45)
                {
                    bendPositions[_bendIndex] =
                            new Vector2(mouseData.X - Mathf.Abs(_positionY - mouseData.Y), _positionY);
                }
                else if (_point.EntryAngleTrue == 45 && _point.MouseAngleFixed == 90)
                {
                    bendPositions[_bendIndex] =
                            new Vector2(mouseData.X, _positionY + Mathf.Abs(mouseData.X - _positionX));
                }
                else if (_point.EntryAngleTrue == 90 && _point.MouseAngleFixed == 135)
                {
                    bendPositions[_bendIndex] =
                            new Vector2(_positionX, mouseData.Y - Mathf.Abs(_positionX - mouseData.X));
                }
                else if (_point.EntryAngleTrue == 135 && _point.MouseAngleFixed == 180)
                {
                    bendPositions[_bendIndex] =
                            new Vector2(_positionX - Mathf.Abs(mouseData.Y - _positionY), mouseData.Y);
                }
                else if (_point.EntryAngleTrue == 180 && _point.MouseAngleFixed == 225)
                {
                    bendPositions[_bendIndex] =
                            new Vector2(mouseData.X + Mathf.Abs(_positionY - mouseData.Y), _positionY);
                }
                else if (_point.EntryAngleTrue == 225 && _point.MouseAngleFixed == 270)
                {
                    bendPositions[_bendIndex] =
                            new Vector2(mouseData.X, _positionY - Mathf.Abs(mouseData.X - _positionX));
                }
                else if (_point.EntryAngleTrue == 270 && _point.MouseAngleFixed == 315)
                {
                    bendPositions[_bendIndex] =
                            new Vector2(_positionX, mouseData.Y + Mathf.Abs(_positionX - mouseData.X));
                }
                else if (_point.EntryAngleTrue == 315 && _point.MouseAngleFixed == 0)
                {
                    bendPositions[_bendIndex] =
                            new Vector2(_positionX + Mathf.Abs(mouseData.Y - _positionY), mouseData.Y);
                }
            }
        }
        else
        {
            // TODO - make it so that this just sets values directly and all of above is done with local variables to reduce code length.
        }
    }

    public void InjectLine(GameObject _object)
    {
        accessor = _object;
    }
    #endregion
}

public class LineManager
{
    #region FIELDS
    private bool debugMode;
    private float width;
    private MouseData mouseData;
    private List<Station> stations;
    private LineRenderer[] lineRenderers;
    private List<Point>[] linePoints;
    private List<Vector2>[] bendPositions;

    private int currentNewLine;

    // Line dragging variables
    private bool isLineHeld;
    private bool lineJustCreated;
    private int connectedLineIndex;
    private bool lineGrabbedPreBend;
    private int linesActive;

    // Point at the previous station.
    private Point connectedPreviousPoint;
    private int connectedPreviousPointIndex;
    private float connectedPreviousAngle;
    private float connectedPreviousOldAngle;

    private float connectedPreviousAngleTrue;
    private float connectedPreviousAngleOld;
    private float connectedPreviousAnglePreBend;
    private float connectedPreviousAnglePostBend;

    private float connectedPreviousAnglePreBendFixed; // Account for if bend is right or left.
    private float connectedPreviousAnglePostBendFixed;

    // Point held by the mouse, always between the previous and next.
    private int connectedMousePointIndex;

    // Point at the next station, only relevant when grabbing lines.
    private Point connectedNextPoint;
    private int connectedNextPointIndex;
    private float connectedNextAngle;
    private float connectedNextOldAngle;

    private float connectedNextAngleTrue;
    private float connectedNextAngleOld;
    private float connectedNextAnglePreBend;
    private float connectedNextAnglePostBend;

    private float connectedNextAnglePreBendFixed; // Account for if bend is right or left.
    private float connectedNextAnglePostBendFixed;

    private float connectedAcrossAngle;

    private float playerLineAngle;
    private float oldPlayerLineAngle;

    private float lineStartAngle;
    private float lineEndAngle;

    private float fixedLineStartAngle;
    private float fixedLineEndAngle;
    private bool bendRight;

    private Station lastHoveredStation;

    private List<AngledRectCollider[]>[] colliders;
    #endregion

    #region PROPERTIES
    /// <summary>
    ///     Whether or not the camera is currently in debug mode.
    /// </summary>
    public bool DebugMode
    {
        get { return debugMode; }
        set { debugMode = value; }
    }

    /// <summary>
    /// Should be used exclusively for debug visual drawing.
    /// </summary>
    public List<AngledRectCollider[]>[] Colliders { get { return colliders; } }
    #endregion

    #region CONSTRUCTORS
    public LineManager (MouseData _mouseData, List<Station> _stations, LineRenderer[] _lineRenderers)
    {
        width = 0.1f;
        mouseData = _mouseData;
        stations = _stations;
        lineRenderers = _lineRenderers;
        linePoints = new List<Point>[lineRenderers.Length];
        for (int _i = 0; _i < lineRenderers.Length; ++_i)
        {
            linePoints[_i] = new List<Point> ();
        }
        bendPositions = new List<Vector2>[lineRenderers.Length];
        for (int _i = 0; _i < lineRenderers.Length; ++_i)
        {
            bendPositions[_i] = new List<Vector2>();
        }

        // Line grabbing / making variables
        isLineHeld = false;
        connectedLineIndex = 0;

        connectedPreviousPoint = null;
        connectedPreviousPointIndex = -1;

        currentNewLine = 0;

        playerLineAngle = 0.0f;
        oldPlayerLineAngle = 0.0f;
        lineStartAngle = 0;
        lineEndAngle = 0;
        fixedLineStartAngle = 0;
        fixedLineEndAngle = 0;
        bendRight = false;
        lastHoveredStation = null;

        linesActive = 0;

        colliders = new List<AngledRectCollider[]>[lineRenderers.Length];
        for (int _i = 0; _i < lineRenderers.Length; ++_i)
        {
            colliders[_i] = new List<AngledRectCollider[]>();
        }
    }
    #endregion

    #region METHODS
    public void PlayerInput ()
    {
        oldPlayerLineAngle = playerLineAngle;
        
        if (isLineHeld == false)
        {
            if (mouseData.LeftPressed)
            {
                bool _found = false;

                // STEP 1:
                // Creating a new line by clicking a station.
                if (linesActive < linePoints.Length)
                {
                    for (int _i = 0; _i < stations.Count; ++_i)
                    {
                        if (stations[_i].PositionInStationCollider(mouseData.Position))
                        {
                            // STEP 1:
                            // Establish line has been grabbed.
                            isLineHeld = true;
                            lineJustCreated = true;
                            connectedLineIndex = linesActive;
                            linesActive++;

                            // STEP 2:
                            // Create the previous point.
                            connectedPreviousPoint = new Point(linePoints[connectedLineIndex], stations[_i]);
                            connectedPreviousPointIndex = linePoints[connectedLineIndex].Count;
                            linePoints[connectedLineIndex].Insert(connectedPreviousPointIndex, connectedPreviousPoint);
                            bendPositions[connectedLineIndex].Insert(connectedPreviousPointIndex, Vector2.zero);

                            // STEP 3:
                            // Create and insert the mouse point.
                            linePoints[connectedLineIndex].Add(null);
                            connectedMousePointIndex = connectedPreviousPointIndex + 1;

                            // STEP 4:
                            // Set initial values.
                            SetMouseLineAngles(mouseData.X, mouseData.Y);
                            SetMouseAccessPoints();
                            SetMouseBends(mouseData.X, mouseData.Y);

                            // STEP 5:
                            // Exit.
                            lastHoveredStation = stations[_i];
                            _found = true;
                            break;
                        }
                        else if (stations[_i] == lastHoveredStation)
                        {
                            lastHoveredStation = null;
                        }
                    }
                }

                // STEP 2:
                // Check lines between stations.
                if (!_found)
                {
                    for (int _line = 0; _line < colliders.Length; ++_line)
                    {
                        if (_found) break;
                        for (int _bendColliders = 0; _bendColliders < colliders[_line].Count; ++_bendColliders)
                        {
                            if (colliders[_line][_bendColliders] == null) continue;
                            bool _collisionPreBend = (colliders[_line][_bendColliders][0].PositionInCollider(mouseData.Position));
                            bool _collisionPostBend = (colliders[_line][_bendColliders][1].PositionInCollider(mouseData.Position));
                            if (_collisionPreBend || _collisionPostBend)
                            {
                                // STEP 1:
                                // Establish line that has been grabbed.
                                isLineHeld = true;
                                lineJustCreated = false;
                                connectedLineIndex = _line;

                                // STEP 2:
                                // Get the previous point.
                                connectedPreviousPoint = linePoints[connectedLineIndex][_bendColliders];
                                connectedPreviousPointIndex = _bendColliders;
                                connectedPreviousPoint.TiedStation.SetPointConnection(
                                        connectedPreviousPoint.AccessExitAngleIndex,
                                        connectedPreviousPoint.AccessExitSlot,
                                        null);

                                // STEP 3:
                                // Get the next point
                                connectedNextPoint = linePoints[connectedLineIndex][_bendColliders + 1];
                                connectedNextPointIndex = _bendColliders + 2;
                                connectedNextPoint.TiedStation.SetPointConnection(
                                        connectedNextPoint.AccessEntryAngleIndex,
                                        connectedNextPoint.AccessEntrySlot,
                                        null);

                                // STEP 4:
                                // Create and insert the mouse point.
                                linePoints[connectedLineIndex].Insert(_bendColliders + 1, null);
                                connectedMousePointIndex = _bendColliders + 1;
                                bendPositions[connectedLineIndex].Insert(_bendColliders + 1, Vector2.zero);

                                // STEP 5:
                                // Delete Colliders
                                colliders[connectedLineIndex][_bendColliders] = null;

                                // STEP 6:
                                // Set initial values.
                                SetMouseLineAngles(mouseData.X, mouseData.Y);
                                SetMouseAccessPoints();
                                SetMouseBends(mouseData.X, mouseData.Y);

                                // STEP 6:
                                // Exit.
                                _found = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
        else if (connectedPreviousPoint != null && connectedNextPoint != null)
        {
            // When the mouse has grabbed a line and manages a previous and next point which the mouse is between.
            SetMouseLineAngles(mouseData.X, mouseData.Y);
            bool _accessSuccess = SetMouseAccessPoints();
            SetMouseBends(mouseData.X, mouseData.Y);
            bool _lineDropped = BetweenLineEdit(_accessSuccess);
            
            // Release the line.
            if (!_lineDropped && mouseData.LeftReleased)
            {
                EndMouseGrabbingLine();
                if (linePoints[connectedLineIndex].Count <= 1) linePoints[connectedLineIndex].Clear();
            }
        }
        else if (connectedPreviousPoint != null)
        {
            // When the mouse has just created a line or grabbed the end of a line.
            SetMouseLineAngles(mouseData.X, mouseData.Y);
            bool _accessSuccess = SetMouseAccessPoints();
            SetMouseBends(mouseData.X, mouseData.Y);
            bool _lineDropped = CreatedLineEdit(_accessSuccess);
            
            // Release the line.
            if (!_lineDropped && mouseData.LeftReleased)
            {
                EndMouseGrabbingLine();
                if (linePoints[connectedLineIndex].Count <= 1)
                {
                    linesActive--;
                    linePoints[connectedLineIndex].Clear();
                }
            }
        }
        else
        {
            // When the mouse has grabbed the start of a line.
        }

        UpdateRenderer();
    }

    private bool BetweenLineEdit (bool _accessSuccess)
    {
        for (int _i = 0; _i < stations.Count; ++_i)
        {
            // Make sure within station's collider, this station hasn't been visited last, and that accessing is possible.
            if (stations[_i].PositionInStationCollider(mouseData.Position) && lastHoveredStation != stations[_i] && _accessSuccess)
            {
                // STEP 1:
                // Establish that this station has now been hovered over.
                lastHoveredStation = stations[_i];

                // STEP 2:
                // Determine what kind of edit to the points in the line this will be.+
                int _interaction = 0;
                for (int _j = 0; _j < linePoints[connectedLineIndex].Count; ++_j)
                {
                    if (linePoints[connectedLineIndex][_j] == null) continue;
                    if (stations[_i] == linePoints[connectedLineIndex][_j].TiedStation)
                    {
                        // The point hovered over is on the line.
                        if (linePoints[connectedLineIndex][_j] == connectedPreviousPoint)
                        {
                            // Station found is previous.
                            if (_j == 0)
                            {
                                // Station found is the first one in the line.
                                // Should remove the previous and convert to grabbing start.
                                _interaction = 1;
                            }
                            else
                            {
                                // Should remove the previous.
                                _interaction = 2;
                            }
                        }
                        else if (linePoints[connectedLineIndex][_j] == connectedNextPoint)
                        {
                            // Station found is next.
                            if (_j == linePoints[connectedLineIndex].Count - 1)
                            {
                                // Station found is the last one in the line.
                                // Should convert to grabbing end.
                                _interaction = 3;
                            }
                            else
                            {
                                // Should remove the next.
                                _interaction = 4;
                            }
                        }
                        else
                        {
                            // Not allowed to connect this point.
                            _interaction = 5;
                        }
                    }
                }

                switch (_interaction)
                {
                    case 0:
                        // Point is not on the line at all, and should be added.
                        // STEP 1:
                        // Insert the new point.
                        Point _nextPoint = connectedNextPoint;
                        connectedNextPoint = null;
                        Point _possiblePoint = new Point(linePoints[connectedLineIndex], stations[_i]);
                        if (!SetFinalAccessPoints(_possiblePoint)) return false;
                        SetFinalBends(_possiblePoint);
                        SetFinalCollider(_possiblePoint);
                        connectedPreviousPoint = _possiblePoint;
                        connectedPreviousPointIndex++;
                        linePoints[connectedLineIndex].Insert(connectedPreviousPointIndex, connectedPreviousPoint);
                        bendPositions[connectedLineIndex].Insert(connectedPreviousPointIndex, Vector2.zero);
                        connectedNextPoint = _nextPoint;
                        connectedNextPointIndex++;

                        // STEP 2:
                        // Update other information.
                        connectedMousePointIndex++;
                        SetMouseLineAngles(mouseData.Position.x, mouseData.Position.y);
                        SetMouseAccessPoints();
                        SetMouseBends(mouseData.Position.x, mouseData.Position.y);

                        // STEP 5:
                        // Exit.
                        return false;

                    case 1:
                        // Remove the first station in the line.
                        // STEP 1:
                        // Remove the previous first.
                        linePoints[connectedLineIndex].RemoveAt(connectedPreviousPointIndex);
                        bendPositions[connectedLineIndex].RemoveAt(connectedPreviousPointIndex);
                        colliders[connectedLineIndex].RemoveAt(connectedPreviousPointIndex);
                        connectedPreviousPoint = null;

                        // STEP 2:
                        // Update the new previous line.
                        /*SetMouseLineAngles(mouseData.X, mouseData.Y);
                        SetMouseAccessPoints();
                        SetMouseBends(mouseData.X, mouseData.Y);*/
                        // TODO make this shit works
                        // STEP 3:
                        // Exit.
                        return false;

                    case 2:
                        // Remove the previous station.
                        // STEP 1:
                        // Remove the previous point and hook up previous to previous.
                        linePoints[connectedLineIndex].RemoveAt(connectedPreviousPointIndex);
                        bendPositions[connectedLineIndex].RemoveAt(connectedPreviousPointIndex);
                        colliders[connectedLineIndex].RemoveAt(connectedPreviousPointIndex);
                        connectedPreviousPointIndex--;
                        connectedPreviousPoint = linePoints[connectedLineIndex][connectedPreviousPointIndex];
                        colliders[connectedLineIndex][connectedPreviousPointIndex] = null;
                        connectedNextPointIndex--;
                        connectedMousePointIndex--;

                        // STEP 2:
                        // Update the new previous line.
                        SetMouseLineAngles(mouseData.X, mouseData.Y);
                        SetMouseAccessPoints();
                        SetMouseBends(mouseData.X, mouseData.Y);

                        // STEP 3:
                        // Exit.
                        return false;

                    case 3:
                        // Remove the last station in the line.
                        // Remove the next station.
                        // STEP 1:
                        // Remove the next last.
                        linePoints[connectedLineIndex].RemoveAt(connectedNextPointIndex);
                        connectedNextPoint = null;
                        Debug.Log(connectedPreviousPointIndex);
                        // STEP 2:
                        // Update the new previous line.
                        SetMouseLineAngles(mouseData.X, mouseData.Y);
                        SetMouseAccessPoints();
                        SetMouseBends(mouseData.X, mouseData.Y);

                        // STEP 3:
                        // Exit.
                        return false;

                    case 4:
                        // Remove the next station.
                        // STEP 1:
                        // Remove the next.
                        linePoints[connectedLineIndex].RemoveAt(connectedNextPointIndex);
                        bendPositions[connectedLineIndex].RemoveAt(connectedNextPointIndex - 1);
                        colliders[connectedLineIndex].RemoveAt(connectedNextPointIndex - 1);
                        connectedNextPoint = linePoints[connectedLineIndex][connectedNextPointIndex];

                        // STEP 2:
                        // Update the new previous line.
                        SetMouseLineAngles(mouseData.X, mouseData.Y);
                        SetMouseAccessPoints();
                        SetMouseBends(mouseData.X, mouseData.Y);

                        // STEP 3:
                        // Exit.
                        return false;
                }
            }
        }
        return false;
    }

    private bool CreatedLineEdit (bool _accessSuccess)
    {
        for (int _i = 0; _i < stations.Count; ++_i)
        {
            // Make sure within station's collider, this station hasn't been visited last, and that accessing is possible.
            if (stations[_i].PositionInStationCollider(mouseData.Position) && lastHoveredStation != stations[_i] && _accessSuccess)
            {
                // STEP 1:
                // Establish that this station has now been hovered over.
                lastHoveredStation = stations[_i];

                // STEP 2:
                // Determine what kind of edit to the points in the line this will be.+
                int _interaction = 0;
                for (int _j = 0; _j < linePoints[connectedLineIndex].Count; ++_j)
                {
                    if (linePoints[connectedLineIndex][_j] == null) continue;
                    if (stations[_i] == linePoints[connectedLineIndex][_j].TiedStation)
                    {
                        // The point hovered over is on the line.
                        if (_j == 0)
                        {
                            // Station found is the first one in the line.
                            if (linePoints[connectedLineIndex].Count == 3)
                            {
                                // Returning to first slot after making a line between 2 stations and nothing else.
                                // Ends the current line drawing.
                                _interaction = 1;
                            }
                            else if (linePoints[connectedLineIndex].Count > 3)
                            {
                                // Returning to first point to finish loop.
                                // Ends current line drawing.
                                _interaction = 2;
                            }
                            else
                            {
                                // Not allowed to connect this point.
                                _interaction = 3;
                            }
                        }
                        else
                        {
                            // Not allowed to connect this point.
                            _interaction = 3;
                        }
                    }
                }

                switch (_interaction)
                {
                    case 0:
                        // Point is not on the line at all, and should be added.
                        // STEP 1:
                        // Insert the new point.
                        Point _possiblePoint = new Point(linePoints[connectedLineIndex], stations[_i]);
                        if (!SetFinalAccessPoints(_possiblePoint)) return false;
                        SetFinalBends(_possiblePoint);
                        SetFinalCollider(_possiblePoint);
                        connectedPreviousPoint = _possiblePoint;
                        connectedPreviousPointIndex = linePoints[connectedLineIndex].Count - 1;
                        linePoints[connectedLineIndex].Insert(connectedPreviousPointIndex, connectedPreviousPoint);
                        bendPositions[connectedLineIndex].Insert(connectedPreviousPointIndex, Vector2.zero);

                        // STEP 2:
                        // Update other information.
                        connectedMousePointIndex++;
                        SetMouseLineAngles(mouseData.X, mouseData.Y);
                        SetMouseAccessPoints();
                        SetMouseBends(mouseData.X, mouseData.Y);

                        // STEP 5:
                        // Exit.
                        return false;

                    case 1:
                        // Point is the first in the line when there are only 2 points.
                        // Delete the current mouse line.
                        EndMouseGrabbingLine();
                        return true;

                    case 2:
                        // Point is the first in the line and a loop is being completed.
                        Point _possibleLoopPoint = new Point(linePoints[connectedLineIndex], stations[_i]);
                        if (!SetFinalAccessPoints(_possibleLoopPoint)) return false;
                        SetFinalBends(_possibleLoopPoint);
                        SetFinalCollider(_possibleLoopPoint);
                        connectedPreviousPoint = _possibleLoopPoint;
                        connectedPreviousPointIndex = linePoints[connectedLineIndex].Count - 1;
                        linePoints[connectedLineIndex].Insert(connectedPreviousPointIndex, connectedPreviousPoint);
                        bendPositions[connectedLineIndex].Insert(connectedPreviousPointIndex, Vector2.zero);

                        // STEP 2:
                        // Update other information.
                        connectedMousePointIndex++;
                        EndMouseGrabbingLine();
                        return true;
                }
            }
        }
        return false;
    }

    private void EndMouseGrabbingLine()
    {
        if (connectedPreviousPoint != null && connectedNextPoint != null)
        {
            // Both points exists.
            linePoints[connectedLineIndex].RemoveAt(connectedMousePointIndex);
            bendPositions[connectedLineIndex].RemoveAt(connectedMousePointIndex);
            colliders[connectedLineIndex].Remove(null);

            Point _storedNext = connectedNextPoint;
            connectedNextPoint = null;

            SetMouseLineAngles(_storedNext.TiedStation.StationTruePosition.x, _storedNext.TiedStation.StationTruePosition.y);
            SetMouseAccessPoints();
            SetMouseBends(_storedNext.TiedStation.StationTruePosition.x, _storedNext.TiedStation.StationTruePosition.y);

            SetFinalAccessPoints(_storedNext);
            SetFinalBends(_storedNext);
            SetFinalCollider(_storedNext);
        }
        else if (connectedPreviousPoint != null)
        {
            // Only previous point exists.
            Debug.Log(linePoints[connectedLineIndex].Count);
            Debug.Log(connectedPreviousPointIndex);
            connectedPreviousPoint.AccessExitAngleIndex = -1;
            connectedPreviousPoint.AccessExitSlot = -1;
            bendPositions[connectedLineIndex].RemoveAt(connectedPreviousPointIndex);
        }
        else if (connectedNextPoint != null)
        {
            // Only next point exists.
        }

        // Total handling.
        linePoints[connectedLineIndex].Remove(null);
        connectedPreviousPoint = null;
        connectedNextPoint = null;
        isLineHeld = false;
        lastHoveredStation = null;
    }

    private bool SetFinalAccessPoints(Point _possiblePoint)
    {
        if (connectedPreviousPoint != null && connectedNextPoint != null)
        {
            // Grabbing between two points.
            int _indexedPreviousAnglePreBend = 0;
            int _indexedPreviousAnglePostBend = 0;
            if (connectedPreviousAngleTrue >= 358.0f || connectedPreviousAngleTrue <= 2.0f)
            {
                _indexedPreviousAnglePreBend = 0;
                _indexedPreviousAnglePostBend = 4;
            }
            else if (connectedPreviousAngleTrue >= 43.0f && connectedPreviousAngleTrue <= 47.0f)
            {
                _indexedPreviousAnglePreBend = 1;
                _indexedPreviousAnglePostBend = 5;
            }
            else if (connectedPreviousAngleTrue >= 88.0f && connectedPreviousAngleTrue <= 92.0f)
            {
                _indexedPreviousAnglePreBend = 2;
                _indexedPreviousAnglePostBend = 6;
            }
            else if (connectedPreviousAngleTrue >= 133.0f && connectedPreviousAngleTrue <= 137.0f)
            {
                _indexedPreviousAnglePreBend = 3;
                _indexedPreviousAnglePostBend = 7;
            }
            else if (connectedPreviousAngleTrue >= 178.0f && connectedPreviousAngleTrue <= 182.0f)
            {
                _indexedPreviousAnglePreBend = 4;
                _indexedPreviousAnglePostBend = 0;
            }
            else if (connectedPreviousAngleTrue >= 223.0f && connectedPreviousAngleTrue <= 227.0f)
            {
                _indexedPreviousAnglePreBend = 5;
                _indexedPreviousAnglePostBend = 1;
            }
            else if (connectedPreviousAngleTrue >= 268.0f && connectedPreviousAngleTrue <= 272.0f)
            {
                _indexedPreviousAnglePreBend = 6;
                _indexedPreviousAnglePostBend = 2;
            }
            else if (connectedPreviousAngleTrue >= 312.0f && connectedPreviousAngleTrue <= 317.0f)
            {
                _indexedPreviousAnglePreBend = 7;
                _indexedPreviousAnglePostBend = 3;
            }
            else
            {
                _indexedPreviousAnglePreBend = (int)(connectedPreviousAnglePreBend / 45.0f);
                _indexedPreviousAnglePreBend =
                        (bendRight)
                            ? (_indexedPreviousAnglePreBend == 7) ? 0 : _indexedPreviousAnglePreBend + 1
                            : _indexedPreviousAnglePreBend;

                _indexedPreviousAnglePostBend = (int)(Mathf.Repeat(connectedPreviousAnglePostBend - 180, 360) / 45.0f);
                _indexedPreviousAnglePostBend =
                        (bendRight)
                            ? (_indexedPreviousAnglePostBend == 0) ? 7 : _indexedPreviousAnglePostBend - 1
                            : _indexedPreviousAnglePostBend;
            }

            for (int _i = 0; _i < 3; ++_i)
            {
                int _iInverse = _i;
                switch (_i)
                {
                    case 1:
                        _iInverse = 2;
                        break;

                    case 2:
                        _iInverse = 1;
                        break;
                }
                if (connectedPreviousPoint.TiedStation.GetPointConnection(_indexedPreviousAnglePreBend, _i) == null &&
                    _possiblePoint.TiedStation.GetPointConnection(_indexedPreviousAnglePostBend, _iInverse) == null)
                {
                    connectedPreviousPoint.AccessExitAngleIndex = _indexedPreviousAnglePreBend;
                    connectedPreviousPoint.AccessExitSlot = _i;
                    connectedPreviousPoint.TiedStation.SetPointConnection(_indexedPreviousAnglePreBend, _i, connectedPreviousPoint);
                    _possiblePoint.AccessEntryAngleIndex = _indexedPreviousAnglePostBend;
                    _possiblePoint.AccessEntrySlot = _iInverse;
                    _possiblePoint.TiedStation.SetPointConnection(_indexedPreviousAnglePostBend, _iInverse, _possiblePoint);
                    return true;
                }
            }
            // Attempt failed
            connectedPreviousPoint.AccessExitAngleIndex = -1;
            connectedPreviousPoint.AccessExitSlot = -1;
            _possiblePoint.AccessEntryAngleIndex = -1;
            _possiblePoint.AccessEntrySlot = -1;
            return false;
        }
        else if (connectedPreviousPoint == null)
        {
            // Grabbing the line from its creation point.
            int _indexedNextAngle = (int)(Mathf.Repeat(connectedPreviousAnglePostBend - 180.0f, 360.0f) / 45.0f);
            for (int _i = 0; _i < 3; ++_i)
            {
                if (connectedNextPoint.TiedStation.GetPointConnection(_indexedNextAngle, _i) == null)
                {
                    connectedNextPoint.AccessExitAngleIndex = _indexedNextAngle;
                    connectedNextPoint.AccessExitSlot = _i;
                    return true;
                }
            }

            // Attempt failed
            connectedNextPoint.AccessExitAngleIndex = -1;
            connectedNextPoint.AccessExitSlot = -1;
            return false;
        }
        else
        {
            // Grabbing the line from its final point.
            int _indexedPreviousAnglePreBend = 0;
            int _indexedPreviousAnglePostBend = 0;
            if (connectedPreviousAngleTrue >= 358.0f || connectedPreviousAngleTrue <= 2.0f)
            {
                _indexedPreviousAnglePreBend = 0;
                _indexedPreviousAnglePostBend = 4;
            }
            else if (connectedPreviousAngleTrue >= 43.0f && connectedPreviousAngleTrue <= 47.0f)
            {
                _indexedPreviousAnglePreBend = 1;
                _indexedPreviousAnglePostBend = 5;
            }
            else if (connectedPreviousAngleTrue >= 88.0f && connectedPreviousAngleTrue <= 92.0f)
            {
                _indexedPreviousAnglePreBend = 2;
                _indexedPreviousAnglePostBend = 6;
            }
            else if (connectedPreviousAngleTrue >= 133.0f && connectedPreviousAngleTrue <= 137.0f)
            {
                _indexedPreviousAnglePreBend = 3;
                _indexedPreviousAnglePostBend = 7;
            }
            else if (connectedPreviousAngleTrue >= 178.0f && connectedPreviousAngleTrue <= 182.0f)
            {
                _indexedPreviousAnglePreBend = 4;
                _indexedPreviousAnglePostBend = 0;
            }
            else if (connectedPreviousAngleTrue >= 223.0f && connectedPreviousAngleTrue <= 227.0f)
            {
                _indexedPreviousAnglePreBend = 5;
                _indexedPreviousAnglePostBend = 1;
            }
            else if (connectedPreviousAngleTrue >= 268.0f && connectedPreviousAngleTrue <= 272.0f)
            {
                _indexedPreviousAnglePreBend = 6;
                _indexedPreviousAnglePostBend = 2;
            }
            else if (connectedPreviousAngleTrue >= 312.0f && connectedPreviousAngleTrue <= 317.0f)
            {
                _indexedPreviousAnglePreBend = 7;
                _indexedPreviousAnglePostBend = 3;
            }
            else
            {
                _indexedPreviousAnglePreBend = (int)(connectedPreviousAnglePreBend / 45.0f);
                _indexedPreviousAnglePreBend =
                        (bendRight)
                            ? (_indexedPreviousAnglePreBend == 7) ? 0 : _indexedPreviousAnglePreBend + 1
                            : _indexedPreviousAnglePreBend;

                _indexedPreviousAnglePostBend = (int)(Mathf.Repeat(connectedPreviousAnglePostBend - 180, 360) / 45.0f);
                _indexedPreviousAnglePostBend =
                        (bendRight)
                            ? (_indexedPreviousAnglePostBend == 0) ? 7 : _indexedPreviousAnglePostBend - 1
                            : _indexedPreviousAnglePostBend;
            }

            for (int _i = 0; _i < 3; ++_i)
            {
                int _iInverse = _i;
                switch (_i)
                {
                    case 1:
                        _iInverse = 2;
                        break;

                    case 2:
                        _iInverse = 1;
                        break;
                }
                if (connectedPreviousPoint.TiedStation.GetPointConnection(_indexedPreviousAnglePreBend, _i) == null &&
                    _possiblePoint.TiedStation.GetPointConnection(_indexedPreviousAnglePostBend, _iInverse) == null)
                {
                    connectedPreviousPoint.AccessExitAngleIndex = _indexedPreviousAnglePreBend;
                    connectedPreviousPoint.AccessExitSlot = _i;
                    connectedPreviousPoint.TiedStation.SetPointConnection(_indexedPreviousAnglePreBend, _i, connectedPreviousPoint);
                    _possiblePoint.AccessEntryAngleIndex = _indexedPreviousAnglePostBend;
                    _possiblePoint.AccessEntrySlot = _iInverse;
                    _possiblePoint.TiedStation.SetPointConnection(_indexedPreviousAnglePostBend, _iInverse, _possiblePoint);
                    return true;
                }
            }
        }
        // Attempt failed
        return false;
    }

    /// <summary>
    /// Sets the angled and slot from which lines should come through the stations they are tied through
    /// when the player is controlling them via the mouse.
    /// </summary>
    /// <returns>Whether or not the line was able to be placed into a slot within an angle for a station.</returns>
    private bool SetMouseAccessPoints ()
    {
        if (connectedPreviousPoint != null && connectedNextPoint != null)
        {
            // Grabbing between two points.
            int _indexedPreviousAngle = (int)(connectedPreviousAnglePreBend / 45.0f);
            _indexedPreviousAngle =
                    (bendRight)
                        ? (_indexedPreviousAngle == 7) ? 0 : _indexedPreviousAngle + 1
                        : _indexedPreviousAngle;
            int _indexedNextAngle = (int)(Mathf.Repeat(connectedPreviousAnglePostBend - 180.0f, 360.0f) / 45.0f);
            /*_indexedNextAngle =
                    (bendRight)
                        ? (_indexedNextAngle == 0) ? 7 : _indexedNextAngle - 1
                        : _indexedNextAngle;*/
            for (int _i = 0; _i < 3; ++_i)
            {
                int _iInverse = _i;
                switch (_i)
                {
                    case 1:
                        _iInverse = 2;
                        break;

                    case 2:
                        _iInverse = 1;
                        break;
                }
                
                if (connectedPreviousPoint.TiedStation.GetPointConnection(_indexedPreviousAngle, _i) == null &&
                    connectedPreviousPoint.TiedStation.GetPointConnection(_indexedNextAngle, _iInverse) == null)
                {
                    connectedPreviousPoint.AccessExitAngleIndex = _indexedPreviousAngle;
                    connectedPreviousPoint.AccessExitSlot = _i;
                    connectedNextPoint.AccessEntryAngleIndex = _indexedNextAngle;
                    connectedNextPoint.AccessEntrySlot = _iInverse;
                    return true;

                }
            }

            // Attempt failed
            connectedPreviousPoint.AccessExitAngleIndex = -1;
            connectedPreviousPoint.AccessExitSlot = -1;
            connectedNextPoint.AccessExitAngleIndex = -1;
            connectedNextPoint.AccessExitSlot = -1;
            return false;
        }
        else if (connectedPreviousPoint == null)
        {
            // Grabbing the line from its creation point.
            int _indexedNextAngle = (int)(Mathf.Repeat(connectedPreviousAnglePostBend - 180.0f, 360.0f) / 45.0f);
            for (int _i = 0; _i < 3; ++_i)
            {
                if (connectedNextPoint.TiedStation.GetPointConnection(_indexedNextAngle, _i) == null)
                {
                    connectedNextPoint.AccessExitAngleIndex = _indexedNextAngle;
                    connectedNextPoint.AccessExitSlot = _i;
                    return true;
                }
            }

            // Attempt failed
            connectedNextPoint.AccessExitAngleIndex = -1;
            connectedNextPoint.AccessExitSlot = -1;
            return false;
        }
        else
        {
            // Grabbing the line from its final point.
            int _indexedPreviousAngle = (int)(connectedPreviousAnglePreBend / 45.0f);
            _indexedPreviousAngle =
                    (bendRight)
                        ? (_indexedPreviousAngle == 7) ? 0 : _indexedPreviousAngle + 1
                        : _indexedPreviousAngle;
            for (int _i = 0; _i < 3; ++_i)
            {
                if (connectedPreviousPoint.TiedStation.GetPointConnection(_indexedPreviousAngle, _i) == null &&
                    (connectedPreviousPoint.AccessEntryAngleIndex != connectedPreviousPoint.AccessExitAngleIndex ||
                     connectedPreviousPoint.AccessEntryAngleIndex == -1))
                {
                    connectedPreviousPoint.AccessExitAngleIndex = _indexedPreviousAngle;
                    connectedPreviousPoint.AccessExitSlot = _i;
                    return true;
                }
            }

            // Attempt failed
            connectedPreviousPoint.AccessExitAngleIndex = -1;
            connectedPreviousPoint.AccessExitSlot = -1;
            return false;
        }
    }

    /// <summary>
    /// Sets the all actual angles to be used for the lines formed around the player's mouse.
    /// </summary>
    private void SetMouseLineAngles(float _mouseX, float _mouseY)
    {
        // STEP 1:
        // Set old angles.
        connectedPreviousAngleOld = connectedPreviousAngleTrue;
        connectedNextAngleOld = connectedNextAngleTrue;

        // Angle from previous start to next end, used for when line is dropped.
        if (connectedPreviousPoint != null && connectedNextPoint != null)
        {
            connectedAcrossAngle =
                    Mathf.Floor(Mathf.Repeat(
                        Mathf.Atan2(
                            connectedNextPoint.TiedStation.StationTruePosition.y 
                            - connectedPreviousPoint.TiedStation.StationTruePosition.y,
                            connectedNextPoint.TiedStation.StationTruePosition.y 
                            - connectedPreviousPoint.TiedStation.StationTruePosition.x)
                        * Mathf.Rad2Deg,
                        360.0f));
        }

        // STEP 1:
        // Calculate the true angle from the previous point if possible.
        if (connectedPreviousPoint != null)
        {
            connectedPreviousAngleTrue =
                    Mathf.Floor(Mathf.Repeat(
                        Mathf.Atan2(
                            _mouseY - connectedPreviousPoint.TiedStation.StationTruePosition.y,
                            _mouseX - connectedPreviousPoint.TiedStation.StationTruePosition.x)
                        * Mathf.Rad2Deg,
                        360.0f));

            // STEP 2:
            // Calculate the angles for the previous point.
            // Pre angle is the one coming from the station point.
            // The Post angle is the one coming from the bend.
            connectedPreviousAnglePreBend =
                    (int)Mathf.Repeat(
                        Mathf.Floor(
                            (connectedPreviousAngleTrue) / 45.0f)
                        * 45.0f, 360);

            if (connectedPreviousAnglePreBend == 0)
            {
                connectedPreviousAnglePostBend = (connectedPreviousAngleTrue > 270) ? 315 : 45;
            }
            else
            {
                if (connectedPreviousAngleTrue == connectedPreviousAnglePreBend)
                {
                    connectedPreviousAnglePostBend =
                            (bendRight)
                            ? (int)Mathf.Repeat(connectedPreviousAnglePreBend - 45.0f, 360)
                            : (int)Mathf.Repeat(connectedPreviousAnglePreBend + 45.0f, 360);
                }
                else
                {
                    connectedPreviousAnglePostBend =
                            (connectedPreviousAngleTrue < connectedPreviousAnglePreBend)
                            ? (int)Mathf.Repeat(connectedPreviousAnglePreBend - 45.0f, 360)
                            : (int)Mathf.Repeat(connectedPreviousAnglePreBend + 45.0f, 360);
                }
            }
        }

        // STEP 3:
        // Calculate the true angle from the next point if possible.
        if (connectedNextPoint != null)
        {
            connectedNextAngleTrue =
                    Mathf.Floor(Mathf.Repeat(
                        Mathf.Atan2(
                            _mouseY - connectedNextPoint.TiedStation.StationTruePosition.y,
                            _mouseX - connectedNextPoint.TiedStation.StationTruePosition.x)
                        * Mathf.Rad2Deg,
                        360.0f));

            // STEP 4:
            // Calculate the angles for the next point.
            // Pre angle is the one coming from the mouse point.
            // The Post angle is the one coming from the bend, NOT the station point.
            connectedNextAnglePreBend =
                    (int)Mathf.Repeat(
                        Mathf.Floor(
                            (connectedNextAngleTrue) / 45.0f)
                        * 45.0f, 360);

            if (connectedNextAnglePreBend == 0)
            {
                connectedNextAnglePostBend = (connectedNextAngleTrue > 270) ? 315 : 45;
            }
            else
            {
                if (connectedNextAngleTrue == connectedNextAnglePreBend)
                {
                    connectedNextAnglePostBend =
                            (bendRight)
                            ? (int)Mathf.Repeat(connectedNextAnglePreBend - 45.0f, 360)
                            : (int)Mathf.Repeat(connectedNextAnglePreBend + 45.0f, 360);
                }
                else
                {
                    connectedNextAnglePostBend =
                            (connectedNextAngleTrue < connectedNextAnglePreBend)
                            ? (int)Mathf.Repeat(connectedNextAnglePreBend - 45.0f, 360)
                            : (int)Mathf.Repeat(connectedNextAnglePreBend + 45.0f, 360);
                }
            }
        }
    }

    /// <summary>
    /// Creates the bending points for lines.
    /// </summary>
    private void SetFinalBends (Point _connectedPoint)
    {
        // STEP 1:
        // Set bend position for the previous line if possible.
        float _connectedX = 
                _connectedPoint.TiedStation.AccessConnections
                    [_connectedPoint.AccessEntryAngleIndex,
                     _connectedPoint.AccessEntrySlot].x;
        float _connectedY =
                _connectedPoint.TiedStation.AccessConnections
                    [_connectedPoint.AccessEntryAngleIndex,
                     _connectedPoint.AccessEntrySlot].y;
        if (connectedPreviousPoint != null)
        {
            float _previousX =
                (connectedPreviousPoint.AccessExitAngleIndex != -1)
                ? connectedPreviousPoint.TiedStation.AccessConnections
                    [connectedPreviousPoint.AccessExitAngleIndex,
                     connectedPreviousPoint.AccessExitSlot].x
                : connectedPreviousPoint.TiedStation.StationTruePosition.x;
            float _previousY =
                    (connectedPreviousPoint.AccessExitAngleIndex != -1)
                    ? connectedPreviousPoint.TiedStation.AccessConnections
                        [connectedPreviousPoint.AccessExitAngleIndex,
                         connectedPreviousPoint.AccessExitSlot].y
                    : connectedPreviousPoint.TiedStation.StationTruePosition.y;

            if (bendRight)
            {
                if (connectedPreviousAnglePreBend == 0 && connectedPreviousAnglePostBend == 45)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] =
                            new Vector2(_previousX + Mathf.Abs(_connectedY - _previousY), _connectedY);
                }
                else if (connectedPreviousAnglePreBend == 45 && connectedPreviousAnglePostBend == 90)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] =
                            new Vector2(_previousX, _connectedY - Mathf.Abs(_previousX - _connectedX));
                }
                else if (connectedPreviousAnglePreBend == 90 && connectedPreviousAnglePostBend == 135)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] =
                            new Vector2(_connectedX, _previousY + Mathf.Abs(_connectedX - _previousX));
                }
                else if (connectedPreviousAnglePreBend == 135 && connectedPreviousAnglePostBend == 180)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] =
                            new Vector2(_connectedX + Mathf.Abs(_previousY - _connectedY), _previousY);
                }
                else if (connectedPreviousAnglePreBend == 180 && connectedPreviousAnglePostBend == 225)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] =
                            new Vector2(_previousX - Mathf.Abs(_connectedY - _previousY), _connectedY);
                }
                else if (connectedPreviousAnglePreBend == 225 && connectedPreviousAnglePostBend == 270)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] =
                            new Vector2(_previousX, _connectedY + Mathf.Abs(_previousX - _connectedX));
                }
                else if (connectedPreviousAnglePreBend == 270 && connectedPreviousAnglePostBend == 315)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] =
                            new Vector2(_connectedX, _previousY - Mathf.Abs(_connectedX - _previousX));
                }
                else if (connectedPreviousAnglePreBend == 315 && connectedPreviousAnglePostBend == 0)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] =
                            new Vector2(_connectedX - Mathf.Abs(_previousY - _connectedY), _previousY);
                }

                // Set the proper angle for calculations.
                connectedPreviousAnglePreBendFixed = connectedPreviousAnglePostBend;
                connectedPreviousAnglePostBendFixed = connectedPreviousAnglePreBend;
            }
            else
            {
                if (connectedPreviousAnglePreBend == 0 && connectedPreviousAnglePostBend == 45)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] =
                            new Vector2(_connectedX - Mathf.Abs(_previousY - _connectedY), _previousY);
                }
                else if (connectedPreviousAnglePreBend == 45 && connectedPreviousAnglePostBend == 90)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] =
                            new Vector2(_connectedX, _previousY + Mathf.Abs(_connectedX - _previousX));
                }
                else if (connectedPreviousAnglePreBend == 90 && connectedPreviousAnglePostBend == 135)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] =
                            new Vector2(_previousX, _connectedY - Mathf.Abs(_previousX - _connectedX));
                }
                else if (connectedPreviousAnglePreBend == 135 && connectedPreviousAnglePostBend == 180)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] =
                            new Vector2(_previousX - Mathf.Abs(_connectedY - _previousY), _connectedY);
                }
                else if (connectedPreviousAnglePreBend == 180 && connectedPreviousAnglePostBend == 225)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] =
                            new Vector2(_connectedX + Mathf.Abs(_previousY - _connectedY), _previousY);
                }
                else if (connectedPreviousAnglePreBend == 225 && connectedPreviousAnglePostBend == 270)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] =
                            new Vector2(_connectedX, _previousY - Mathf.Abs(_connectedX - _previousX));
                }
                else if (connectedPreviousAnglePreBend == 270 && connectedPreviousAnglePostBend == 315)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] =
                            new Vector2(_previousX, _connectedY + Mathf.Abs(_previousX - _connectedX));
                }
                else if (connectedPreviousAnglePreBend == 315 && connectedPreviousAnglePostBend == 0)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] =
                            new Vector2(_previousX + Mathf.Abs(_connectedY - _previousY), _connectedY);
                }

                // Set the proper angle for calculations.
                connectedPreviousAnglePreBendFixed = connectedPreviousAnglePreBend;
                connectedPreviousAnglePostBendFixed = connectedPreviousAnglePostBend;
            }
        }

        // STEP 2:
        // Set bend position for the next line if possible.
        if (connectedNextPoint != null)
        {
            float _nextX =
                (connectedNextPoint.AccessExitAngleIndex != -1)
                ? connectedNextPoint.TiedStation.AccessConnections
                    [connectedNextPoint.AccessExitAngleIndex,
                     connectedNextPoint.AccessExitSlot].x
                : connectedNextPoint.TiedStation.StationTruePosition.x;
            float _nextY =
                    (connectedNextPoint.AccessExitAngleIndex != -1)
                    ? connectedNextPoint.TiedStation.AccessConnections
                        [connectedNextPoint.AccessExitAngleIndex,
                         connectedNextPoint.AccessExitSlot].y
                    : connectedNextPoint.TiedStation.StationTruePosition.y;

            if (bendRight)
            {
                if (connectedNextAnglePreBend == 0 && connectedNextAnglePostBend == 45)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_connectedX - Mathf.Abs(_nextY - _connectedY), _nextY);
                }
                else if (connectedNextAnglePreBend == 45 && connectedNextAnglePostBend == 90)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_connectedX, _nextY + Mathf.Abs(_connectedX - _nextX));
                }
                else if (connectedNextAnglePreBend == 90 && connectedNextAnglePostBend == 135)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_nextX, _connectedY - Mathf.Abs(_nextX - _connectedX));
                }
                else if (connectedNextAnglePreBend == 135 && connectedNextAnglePostBend == 180)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_nextX - Mathf.Abs(_connectedY - _nextY), _connectedY);
                }
                else if (connectedNextAnglePreBend == 180 && connectedNextAnglePostBend == 225)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_connectedX + Mathf.Abs(_nextY - _connectedY), _nextY);
                }
                else if (connectedNextAnglePreBend == 225 && connectedNextAnglePostBend == 270)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_connectedX, _nextY - Mathf.Abs(_connectedX - _nextX));
                }
                else if (connectedNextAnglePreBend == 270 && connectedNextAnglePostBend == 315)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_nextX, _connectedY + Mathf.Abs(_nextX - _connectedX));
                }
                else if (connectedNextAnglePreBend == 315 && connectedNextAnglePostBend == 0)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_nextX + Mathf.Abs(_connectedY - _nextY), _connectedY);
                }

                // Set the proper angle for calculations.
                connectedNextAnglePreBendFixed = connectedNextAnglePostBend;
                connectedNextAnglePostBendFixed = connectedNextAnglePreBend;
            }
            else
            {
                if (connectedNextAnglePreBend == 0 && connectedNextAnglePostBend == 45)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_nextX + Mathf.Abs(mouseData.Position.y - _nextY), mouseData.Position.y);
                }
                else if (connectedNextAnglePreBend == 45 && connectedNextAnglePostBend == 90)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_nextX, mouseData.Position.y - Mathf.Abs(_nextX - mouseData.Position.x));
                }
                else if (connectedNextAnglePreBend == 90 && connectedNextAnglePostBend == 135)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(mouseData.Position.x, _nextY + Mathf.Abs(mouseData.Position.x - _nextX));
                }
                else if (connectedNextAnglePreBend == 135 && connectedNextAnglePostBend == 180)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(mouseData.Position.x + Mathf.Abs(_nextY - mouseData.Position.y), _nextY);
                }
                else if (connectedNextAnglePreBend == 180 && connectedNextAnglePostBend == 225)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_nextX - Mathf.Abs(mouseData.Position.y - _nextY), mouseData.Position.y);
                }
                else if (connectedNextAnglePreBend == 225 && connectedNextAnglePostBend == 270)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_nextX, mouseData.Position.y + Mathf.Abs(_nextX - mouseData.Position.x));
                }
                else if (connectedNextAnglePreBend == 270 && connectedNextAnglePostBend == 315)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(mouseData.Position.x, _nextY - Mathf.Abs(mouseData.Position.x - _nextX));
                }
                else if (connectedNextAnglePreBend == 315 && connectedNextAnglePostBend == 0)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(mouseData.Position.x - Mathf.Abs(_nextY - mouseData.Position.y), _nextY);
                }

                // Set the proper angle for calculations.
                connectedNextAnglePreBendFixed = connectedNextAnglePreBend;
                connectedNextAnglePostBendFixed = connectedNextAnglePostBend;
            }
        }

        // Set the direction for the line.
        SetMouseBendDirection();
    }

    /// <summary>
    /// Creates the bending points for lines.
    /// </summary>
    private void SetMouseBends(float _mouseX, float _mouseY)
    {
        // STEP 1:
        // Set bend position for the previous line if possible.
        if (connectedPreviousPoint != null)
        {
            float _previousX =
                (connectedPreviousPoint.AccessExitAngleIndex != -1)
                ? connectedPreviousPoint.TiedStation.AccessConnections
                    [connectedPreviousPoint.AccessExitAngleIndex,
                     connectedPreviousPoint.AccessExitSlot].x
                : connectedPreviousPoint.TiedStation.StationTruePosition.x;
            float _previousY =
                    (connectedPreviousPoint.AccessExitAngleIndex != -1)
                    ? connectedPreviousPoint.TiedStation.AccessConnections
                        [connectedPreviousPoint.AccessExitAngleIndex,
                         connectedPreviousPoint.AccessExitSlot].y
                    : connectedPreviousPoint.TiedStation.StationTruePosition.y;

            if (bendRight)
            {
                if (connectedPreviousAnglePreBend == 0 && connectedPreviousAnglePostBend == 45)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] = 
                            new Vector2(_previousX + Mathf.Abs(_mouseY - _previousY), _mouseY);
                }
                else if (connectedPreviousAnglePreBend == 45 && connectedPreviousAnglePostBend == 90)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] = 
                            new Vector2(_previousX, _mouseY - Mathf.Abs(_previousX - _mouseX));
                }
                else if (connectedPreviousAnglePreBend == 90 && connectedPreviousAnglePostBend == 135)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] = 
                            new Vector2(_mouseX, _previousY + Mathf.Abs(_mouseX - _previousX));
                }
                else if (connectedPreviousAnglePreBend == 135 && connectedPreviousAnglePostBend == 180)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] = 
                            new Vector2(_mouseX + Mathf.Abs(_previousY - _mouseY), _previousY);
                }
                else if (connectedPreviousAnglePreBend == 180 && connectedPreviousAnglePostBend == 225)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] = 
                            new Vector2(_previousX - Mathf.Abs(_mouseY - _previousY), _mouseY);
                }
                else if (connectedPreviousAnglePreBend == 225 && connectedPreviousAnglePostBend == 270)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] = 
                            new Vector2(_previousX, _mouseY + Mathf.Abs(_previousX - _mouseX));
                }
                else if (connectedPreviousAnglePreBend == 270 && connectedPreviousAnglePostBend == 315)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] = 
                            new Vector2(_mouseX, _previousY - Mathf.Abs(_mouseX - _previousX));
                }
                else if (connectedPreviousAnglePreBend == 315 && connectedPreviousAnglePostBend == 0)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] = 
                            new Vector2(_mouseX - Mathf.Abs(_previousY - _mouseY), _previousY);
                }

                // Set the proper angle for calculations.
                connectedPreviousAnglePreBendFixed = connectedPreviousAnglePostBend;
                connectedPreviousAnglePostBendFixed = connectedPreviousAnglePreBend;
            }
            else
            {
                if (connectedPreviousAnglePreBend == 0 && connectedPreviousAnglePostBend == 45)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] = 
                            new Vector2(_mouseX - Mathf.Abs(_previousY - _mouseY), _previousY);
                }
                else if (connectedPreviousAnglePreBend == 45 && connectedPreviousAnglePostBend == 90)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] = 
                            new Vector2(_mouseX, _previousY + Mathf.Abs(_mouseX - _previousX));
                }
                else if (connectedPreviousAnglePreBend == 90 && connectedPreviousAnglePostBend == 135)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] = 
                            new Vector2(_previousX, _mouseY - Mathf.Abs(_previousX - _mouseX));
                }
                else if (connectedPreviousAnglePreBend == 135 && connectedPreviousAnglePostBend == 180)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] = 
                            new Vector2(_previousX - Mathf.Abs(_mouseY - _previousY), _mouseY);
                }
                else if (connectedPreviousAnglePreBend == 180 && connectedPreviousAnglePostBend == 225)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] = 
                            new Vector2(_mouseX + Mathf.Abs(_previousY - _mouseY), _previousY);
                }
                else if (connectedPreviousAnglePreBend == 225 && connectedPreviousAnglePostBend == 270)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] = 
                            new Vector2(_mouseX, _previousY - Mathf.Abs(_mouseX - _previousX));
                }
                else if (connectedPreviousAnglePreBend == 270 && connectedPreviousAnglePostBend == 315)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] = 
                            new Vector2(_previousX, _mouseY + Mathf.Abs(_previousX - _mouseX));
                }
                else if (connectedPreviousAnglePreBend == 315 && connectedPreviousAnglePostBend == 0)
                {
                    bendPositions[connectedLineIndex][connectedPreviousPointIndex] = 
                            new Vector2(_previousX + Mathf.Abs(_mouseY - _previousY), _mouseY);
                }

                // Set the proper angle for calculations.
                connectedPreviousAnglePreBendFixed = connectedPreviousAnglePreBend;
                connectedPreviousAnglePostBendFixed = connectedPreviousAnglePostBend;
            }
        }

        // STEP 2:
        // Set bend position for the next line if possible.
        if (connectedNextPoint != null)
        {
            float _nextX =
                (connectedNextPoint.AccessExitAngleIndex != -1)
                ? connectedNextPoint.TiedStation.AccessConnections
                    [connectedNextPoint.AccessExitAngleIndex,
                     connectedNextPoint.AccessExitSlot].x
                : connectedNextPoint.TiedStation.StationTruePosition.x;
            float _nextY =
                    (connectedNextPoint.AccessExitAngleIndex != -1)
                    ? connectedNextPoint.TiedStation.AccessConnections
                        [connectedNextPoint.AccessExitAngleIndex,
                         connectedNextPoint.AccessExitSlot].y
                    : connectedNextPoint.TiedStation.StationTruePosition.y;

            if (bendRight)
            {
                if (connectedNextAnglePreBend == 0 && connectedNextAnglePostBend == 45)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_mouseX - Mathf.Abs(_nextY - _mouseY), _nextY);
                }
                else if (connectedNextAnglePreBend == 45 && connectedNextAnglePostBend == 90)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_mouseX, _nextY + Mathf.Abs(_mouseX - _nextX));
                }
                else if (connectedNextAnglePreBend == 90 && connectedNextAnglePostBend == 135)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_nextX, _mouseY - Mathf.Abs(_nextX - _mouseX));
                }
                else if (connectedNextAnglePreBend == 135 && connectedNextAnglePostBend == 180)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_nextX - Mathf.Abs(_mouseY - _nextY), _mouseY);
                }
                else if (connectedNextAnglePreBend == 180 && connectedNextAnglePostBend == 225)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_mouseX + Mathf.Abs(_nextY - _mouseY), _nextY);
                }
                else if (connectedNextAnglePreBend == 225 && connectedNextAnglePostBend == 270)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_mouseX, _nextY - Mathf.Abs(_mouseX - _nextX));
                }
                else if (connectedNextAnglePreBend == 270 && connectedNextAnglePostBend == 315)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_nextX, _mouseY + Mathf.Abs(_nextX - _mouseX));
                }
                else if (connectedNextAnglePreBend == 315 && connectedNextAnglePostBend == 0)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_nextX + Mathf.Abs(_mouseY - _nextY), _mouseY);
                }

                // Set the proper angle for calculations.
                connectedNextAnglePreBendFixed = connectedNextAnglePostBend;
                connectedNextAnglePostBendFixed = connectedNextAnglePreBend;
            }
            else
            {
                if (connectedNextAnglePreBend == 0 && connectedNextAnglePostBend == 45)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_nextX + Mathf.Abs(_mouseY - _nextY), _mouseY);
                }
                else if (connectedNextAnglePreBend == 45 && connectedNextAnglePostBend == 90)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_nextX, _mouseY - Mathf.Abs(_nextX - _mouseX));
                }
                else if (connectedNextAnglePreBend == 90 && connectedNextAnglePostBend == 135)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_mouseX, _nextY + Mathf.Abs(_mouseX - _nextX));
                }
                else if (connectedNextAnglePreBend == 135 && connectedNextAnglePostBend == 180)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_mouseX + Mathf.Abs(_nextY - _mouseY), _nextY);
                }
                else if (connectedNextAnglePreBend == 180 && connectedNextAnglePostBend == 225)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_nextX - Mathf.Abs(_mouseY - _nextY), _mouseY);
                }
                else if (connectedNextAnglePreBend == 225 && connectedNextAnglePostBend == 270)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_nextX, _mouseY + Mathf.Abs(_nextX - _mouseX));
                }
                else if (connectedNextAnglePreBend == 270 && connectedNextAnglePostBend == 315)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_mouseX, _nextY - Mathf.Abs(_mouseX - _nextX));
                }
                else if (connectedNextAnglePreBend == 315 && connectedNextAnglePostBend == 0)
                {
                    bendPositions[connectedLineIndex][connectedNextPointIndex - 1] =
                            new Vector2(_mouseX - Mathf.Abs(_nextY - _mouseY), _nextY);
                }

                // Set the proper angle for calculations.
                connectedNextAnglePreBendFixed = connectedNextAnglePreBend;
                connectedNextAnglePostBendFixed = connectedNextAnglePostBend;
            }
        }

        // Set the direction for the line.
        SetMouseBendDirection();
    }

    private void SetMouseBendDirection ()
    {
        // STEP 5:
        // Determine if lines should bend right or left.
        if (connectedPreviousPoint == null)
        {
            if (bendRight)
            {
                if (Mathf.Floor(connectedNextAngleOld / 45.0f) < Mathf.Floor(connectedNextAngleTrue / 45.0f) &&
                    !(Mathf.Floor(connectedNextAngleOld / 45.0f) == 0 && Mathf.Floor(connectedNextAngleTrue / 45.0f) == 7))
                {
                    bendRight = false;
                }
            }
            else
            {
                if (Mathf.Floor(connectedNextAngleOld / 45.0f) > Mathf.Floor(connectedNextAngleTrue / 45.0f) &&
                    !(Mathf.Floor(connectedNextAngleOld / 45.0f) == 7 && Mathf.Floor(connectedNextAngleTrue / 45.0f) == 0))
                {
                    bendRight = true;
                }
            }
        }
        else
        {
            if (bendRight)
            {
                if (Mathf.Floor(connectedPreviousAngleOld / 45.0f) < Mathf.Floor(connectedPreviousAngleTrue / 45.0f) &&
                    !(Mathf.Floor(connectedPreviousAngleOld / 45.0f) == 0 && Mathf.Floor(connectedPreviousAngleTrue / 45.0f) == 7)) 
                {
                    bendRight = false;
                }
            }
            else
            {
                if (Mathf.Floor(connectedPreviousAngleOld / 45.0f) > Mathf.Floor(connectedPreviousAngleTrue / 45.0f) &&
                    !(Mathf.Floor(connectedPreviousAngleOld / 45.0f) == 7 && Mathf.Floor(connectedPreviousAngleTrue / 45.0f) == 0))
                {
                    bendRight = true;
                }
            }
        }
    }

    private void SetFinalCollider (Point _connectedPoint)
    {
        float _connectedX =
                _connectedPoint.TiedStation.AccessConnections
                    [_connectedPoint.AccessEntryAngleIndex,
                     _connectedPoint.AccessEntrySlot].x;
        float _connectedY =
                _connectedPoint.TiedStation.AccessConnections
                    [_connectedPoint.AccessEntryAngleIndex,
                     _connectedPoint.AccessEntrySlot].y;

        if (connectedPreviousPoint != null)
        {
            // STEP 1:
            // Set values.
            colliders[connectedLineIndex].Insert(connectedPreviousPointIndex, new AngledRectCollider[2]);

            float _previousX =
                (connectedPreviousPoint.AccessExitAngleIndex != -1)
                ? connectedPreviousPoint.TiedStation.AccessConnections
                    [connectedPreviousPoint.AccessExitAngleIndex,
                     connectedPreviousPoint.AccessExitSlot].x
                : connectedPreviousPoint.TiedStation.StationTruePosition.x;
            float _previousY =
                    (connectedPreviousPoint.AccessExitAngleIndex != -1)
                    ? connectedPreviousPoint.TiedStation.AccessConnections
                        [connectedPreviousPoint.AccessExitAngleIndex,
                         connectedPreviousPoint.AccessExitSlot].y
                    : connectedPreviousPoint.TiedStation.StationTruePosition.y;

            // STEP 2:
            // First half pre bend collider.
            float _perpendicularAngle = connectedPreviousAnglePreBendFixed + 90.0f; 
            Vector2 _pointOffset = new Vector2(
                    (width / 2.0f) * Mathf.Cos(_perpendicularAngle * Mathf.Deg2Rad),
                    (width / 2.0f) * Mathf.Sin(_perpendicularAngle * Mathf.Deg2Rad));

            colliders[connectedLineIndex][connectedPreviousPointIndex][0] = new AngledRectCollider(
                    new Vector2(
                        _previousX + _pointOffset.x,
                        _previousY + _pointOffset.y),
                    new Vector2(
                        bendPositions[connectedLineIndex][connectedPreviousPointIndex].x + _pointOffset.x,
                        bendPositions[connectedLineIndex][connectedPreviousPointIndex].y + _pointOffset.y),
                    new Vector2(
                        bendPositions[connectedLineIndex][connectedPreviousPointIndex].x - _pointOffset.x,
                        bendPositions[connectedLineIndex][connectedPreviousPointIndex].y - _pointOffset.y),
                    new Vector2(
                        _previousX - _pointOffset.x,
                        _previousY - _pointOffset.y));

            // STEP 3:
            // Second half after the bend collider.
            _perpendicularAngle = connectedPreviousAnglePostBendFixed + 90.0f;
            _pointOffset = new Vector2(
                    (width / 2.0f) * Mathf.Cos(_perpendicularAngle * Mathf.Deg2Rad),
                    (width / 2.0f) * Mathf.Sin(_perpendicularAngle * Mathf.Deg2Rad));

            colliders[connectedLineIndex][connectedPreviousPointIndex][1] = new AngledRectCollider(
                    new Vector2(
                        bendPositions[connectedLineIndex][connectedPreviousPointIndex].x + _pointOffset.x,
                        bendPositions[connectedLineIndex][connectedPreviousPointIndex].y + _pointOffset.y),
                    new Vector2(
                        _connectedX + _pointOffset.x,
                        _connectedY + _pointOffset.y),
                    new Vector2(
                        _connectedX - _pointOffset.x,
                        _connectedY - _pointOffset.y),
                    new Vector2(
                        bendPositions[connectedLineIndex][connectedPreviousPointIndex].x - _pointOffset.x,
                        bendPositions[connectedLineIndex][connectedPreviousPointIndex].y - _pointOffset.y));
        }
    }

    /// <summary>
    /// Causes all line renderers to have their data updated.
    /// </summary>
    public void UpdateRenderer()
    {
        List<Vector3> _allPointPositions = new List<Vector3>();
        for (int _i = 0; _i < linePoints.Length; ++_i)
        {
            for (int _j = 0; _j < linePoints[_i].Count; ++_j)
            {
                if (linePoints[_i][_j] == null)
                {
                    // Null found, use mouse position instead.
                    _allPointPositions.Add(mouseData.Position);
                }
                else
                {
                    if (linePoints[_i][_j].AccessEntryAngleIndex != -1)
                    {
                        _allPointPositions.Add(
                            linePoints[_i][_j].TiedStation.
                                AccessConnections[linePoints[_i][_j].AccessEntryAngleIndex, linePoints[_i][_j].AccessEntrySlot]);
                    }
                    if (linePoints[_i][_j].AccessExitAngleIndex != -1)
                    {
                        _allPointPositions.Add(
                            linePoints[_i][_j].TiedStation.
                                AccessConnections[linePoints[_i][_j].AccessExitAngleIndex, linePoints[_i][_j].AccessExitSlot]);
                    }
                    else if (_j + 1 < linePoints[_i].Count)
                    {
                        _allPointPositions.Add(linePoints[_i][_j].TiedStation.StationTruePosition);
                    }
                }

                if (_j < linePoints[_i].Count - 1)
                {
                    if (linePoints[_i][_j] == null || linePoints[_i][_j + 1] == null ||
                        linePoints[_i][_j].AccessExitAngleIndex != linePoints[_i][_j + 1].AccessEntryAngleIndex)
                    {
                        _allPointPositions.Add(bendPositions[_i][_j]);
                    }
                }
            }
            lineRenderers[_i].positionCount = _allPointPositions.Count;
            lineRenderers[_i].startWidth = width;
            lineRenderers[_i].endWidth = width;

            lineRenderers[_i].SetPositions(_allPointPositions.ToArray());
            _allPointPositions.Clear();
        }
    }

    public bool MouseTempTieStation (Point _pointStart)
    {
        // Attempt to hook up point to specific slot.
        int _indexedStartAngle = (int)(lineStartAngle / 45);
        for (int _i = 0; _i < 3; ++_i)
        {
            if (_pointStart.TiedStation.GetPointConnection(_indexedStartAngle, _i) == null)
            {
                _pointStart.AccessExitAngleIndex = _indexedStartAngle;
                _pointStart.AccessExitSlot = _i;
                return true;
            }
        }

        // Attempt failed
        _pointStart.AccessExitAngleIndex = -1;
        _pointStart.AccessExitSlot = -1;
        return false;
    }

    public bool TiePointsToStations(Point _pointStart, Point _pointEnd)
    {
        int _indexedStartAngle = (int)(lineStartAngle / 45);
        int _indexedEndAngle = (int)(Mathf.Repeat(lineEndAngle - 180, 360) / 45);

        for (int _i = 0; _i < 3; ++_i)
        {
            int _iInverse = _i;
            switch (_i)
            {
                case 1:
                    _iInverse = 2;
                    break;

                case 2:
                    _iInverse = 1;
                    break;
            }
            if (_pointStart.TiedStation.GetPointConnection(_indexedStartAngle, _i) == null &&
                _pointEnd.TiedStation.GetPointConnection(_indexedEndAngle, _iInverse) == null)
            {
                _pointStart.AccessExitAngleIndex = _indexedStartAngle;
                _pointStart.AccessExitSlot = _i;
                _pointStart.TiedStation.SetPointConnection(_indexedStartAngle, _i, _pointStart);
                _pointEnd.AccessEntryAngleIndex = _indexedEndAngle;
                _pointEnd.AccessEntrySlot = _iInverse;
                _pointEnd.TiedStation.SetPointConnection(_indexedEndAngle, _iInverse, _pointEnd);
                return true;
            }
        }
        return false;
    }
    #endregion
}

#region Station Spawning
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
    public StationGridReference(Vector2 _truePosition, Vector2Int _gridPosition, Vector2Int _removalPosition)
    {
        truePosition = _truePosition;
        gridPosition = _gridPosition;
        removalPosition = _removalPosition;
    }
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
    public bool DebugMode { get { return debugMode; } set { debugMode = value; } }

    /// <summary>
    ///     Returns the dimensions of the 'trueGrid'.
    /// </summary>
    public Vector2Int TrueGridDimensions { get { return trueGridDimensions; } }

    #region STATION SPAWN AREA WITHIN CAMERA
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
    #endregion
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
    /// <param name="_stations">A list that contains all statons, is shared in main.</param>
    public StationGrid (
        CameraData _cameraData, float _distanceBetweenGridIndices,
        int _startingStationTotal, int _stationMaximumTotal,
        float _stationPlacementScreenPercent, float _stationBoundaryRadius,
        List<Station> _stations)
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
        stations = _stations;

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
    ///     Returns a StationGridReference at a given position in the 'trueGrid'.
    /// </summary>
    /// <param name="_x">X position in true grid to pull from.</param>
    /// <param name="_y">Y position in true grid to pull from.</param>
    /// <returns></returns>
    public StationGridReference GetStationGridReference(int _x, int _y)
    {
        return trueGrid[_x, _y];
    }

    /// <summary>
    ///     Removes used indices and updates positions on those after.
    /// </summary>
    private void GridRemoveUsed()
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
    public Station CreateStation()
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
    public Station GetStation(int _index)
    {
        return stations[_index];
    }
    #endregion
}
#endregion

public class Manager : MonoBehaviour
{
    #region STATIC INSTANCE

    public static Manager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple Manager instances found!");
            Destroy(gameObject);
        }
    }

    #endregion

    #region FIELDS

    // Prefabs.
    [SerializeField] private GameObject stationPrefab;
    [SerializeField] private GameObject lineRendererPrefab;
    [SerializeField] private Sprite[] stationSprites = new Sprite[(int)STATION_SHAPE.LENGTH];

    // Basic System Storage.
    private MouseData mouseData;
    private List<Timer> timers;
    private CameraData cameraData;
    [SerializeField] private float maxCameraTime = 600f;
    private Timer cameraZoomOutTime;

    //Station Manager
    [SerializeField] private int startingStationTotal = 3;
    [SerializeField] private int stationMaximumTotal = 20;
    [SerializeField] private float timeBetweenStationSpawns = 15f;
    private Timer stationSpawnTimer;
    private StationGrid stationGrid;

    // Station Handling
    private List<Station> stations;

    // Line Manger
    private LineRenderer[] lineRenderers;
    private LineManager lineManager;

    // Game Pausing.
    private bool isGameRunning = false;
    public bool isGamePaused = false; // Add this to track pause state

    // Train Resources
    public int availableTrains = 3; // Example starting value
    #endregion

    #region UNITY MONOBEHAVIOR
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // STEP 1:
        // Insert data into generic fields.
        isGameRunning = true;
        isGamePaused = false; // Initialize pause state
        mouseData = new MouseData();
        cameraData = new CameraData(Camera.main, 5.0f, 10.0f);
        //cameraData.DebugMode = true;
        timers = new List<Timer>();
        stations = new List<Station>();
        stationGrid = new StationGrid(cameraData, 1.0f, startingStationTotal, stationMaximumTotal, 0.9f, 3.0f, stations);
        stationGrid.DebugMode = true;
        
        // STEP 2:
        // Create starting stations.
        for (int _i = 0; _i < startingStationTotal; ++_i) { CreateStation(); }

        // STEP 3:
        // Set variables for camera size change  and station spawning.
        cameraZoomOutTime = CreateTimer(maxCameraTime, true);
        stationSpawnTimer = CreateTimer(timeBetweenStationSpawns, false);

        // STEP 4:
        // Create the line manager.
        lineRenderers = new LineRenderer[8];
        for (int _i = 0; _i < lineRenderers.Length; ++_i)
        {
            lineRenderers[_i] = Instantiate(lineRendererPrefab).GetComponent<LineRenderer>();
            switch (_i)
            {
                case 0:
                    lineRenderers[_i].startColor = Color.yellow;
                    lineRenderers[_i].endColor = Color.yellow;
                    break;

                case 1:
                    lineRenderers[_i].startColor = Color.red;
                    lineRenderers[_i].endColor = Color.red;
                    break;

                case 2:
                    lineRenderers[_i].startColor = Color.blue;
                    lineRenderers[_i].endColor = Color.blue;
                    break;

            }
        }
        lineManager = new LineManager(mouseData, stations, lineRenderers);
        lineManager.DebugMode = true;
    }

    // Update is called once per frame
    private void Update()
    {
        mouseData.SetData();

        if (!isGamePaused) // Only update game elements if not paused
        {
            // STEP 1:
            // Increment all existing Timer class instances.
            foreach (Timer timer in timers)
            {
                timer.IncrementTimer(Time.deltaTime);
            }

            // STEP 2:
            // TODO - Camera scaling and spawing of stations over time.
            /*cameraData.UpdateCameraSize(cameraZoomOutTime.TimerPercentage);
            if (stationSpawnTimer.Trigger)
            {
                CreateStation();
            }*/
        }

        lineManager.PlayerInput();
    }

    #endregion

    #region METHODS
    /// <summary>
    ///     Creates a station and instantiates its associated object in the scene.
    /// </summary>
    private void CreateStation()
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
    private Timer CreateTimer(float _length, bool _runsOnce)
    {
        Timer _timer = new Timer(_length, _runsOnce);
        timers.Add(_timer);
        return _timer;
    }

    #region UI CONTENT

    // Methods to control game state from UI
    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f; // Stop time for game elements
        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f; // Resume normal time
        Debug.Log("Game Resumed");
    }

    public void ExitGame()
    {
                Debug.Log("Exiting Game...");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void SelectColor(int colorIndex)
    {
        Debug.Log("Selected color index: " + colorIndex);
        // Implement logic for selecting this color for line drawing
    }

    public void SelectTunnel()
    {
        Debug.Log("Selected Tunnel");
        // Implement logic for selecting the tunnel tool
    }

    public void SelectTrain()
    {
        Debug.Log("Selected Train");
        // Implement logic for selecting the train tool
    }

    /// <summary>
    ///     Sets the game speed.
    /// </summary>
    /// <param name="speed">The speed multiplier (1f = normal, 2f = double speed, etc.).</param>
    public void SetGameSpeed(float speed)
    {
        Time.timeScale = speed;
        Debug.Log("Game speed set to: " + speed);
    }

    // Method to get available train count
    public int GetAvailableTrains()
    {
        return availableTrains;
    }

    // Method to reduce available train count
    public void UseTrain()
    {
        availableTrains--;
        if (availableTrains < 0)
        {
            availableTrains = 0; // Prevent going below zero
        }

        Debug.Log("Train used. Available trains: " + availableTrains);
    }

    // Method to increase available train count (if needed)
    public void AddTrain(int amount)
    {
        availableTrains += amount;
        Debug.Log("Trains added. Available trains: " + availableTrains);
    }
    #endregion
    
    #region GIZMOS
    private void OnDrawGizmos()
    {
        if (!isGameRunning) return;

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

        Gizmos.color = Color.white;
        for (int _i = 0; _i < stations.Count; _i++)
        {
            if (stations[_i].DebugMode)
            {
                for (int _j = 0; _j < 8; ++_j)
                {
                    Gizmos.DrawSphere(stations[_i].AccessConnections[_j, 0], 0.05f);
                    Gizmos.DrawSphere(stations[_i].AccessConnections[_j, 1], 0.05f);
                    Gizmos.DrawSphere(stations[_i].AccessConnections[_j, 2], 0.05f);
                }
            }
        }

        if (lineManager.DebugMode)
        {
            for (int _i = 0; _i < 8; ++_i)
            {
                for (int _j = 0; _j < lineManager.Colliders[_i].Count; ++_j)
                {
                    if (lineManager.Colliders[_i][_j] == null) continue;
                    Gizmos.DrawLine(lineManager.Colliders[_i][_j][0].PointA, lineManager.Colliders[_i][_j][0].PointB);
                    Gizmos.DrawLine(lineManager.Colliders[_i][_j][0].PointB, lineManager.Colliders[_i][_j][0].PointC);
                    Gizmos.DrawLine(lineManager.Colliders[_i][_j][0].PointC, lineManager.Colliders[_i][_j][0].PointD);
                    Gizmos.DrawLine(lineManager.Colliders[_i][_j][0].PointD, lineManager.Colliders[_i][_j][0].PointA);

                    Gizmos.DrawLine(lineManager.Colliders[_i][_j][1].PointA, lineManager.Colliders[_i][_j][1].PointB);
                    Gizmos.DrawLine(lineManager.Colliders[_i][_j][1].PointB, lineManager.Colliders[_i][_j][1].PointC);
                    Gizmos.DrawLine(lineManager.Colliders[_i][_j][1].PointC, lineManager.Colliders[_i][_j][1].PointD);
                    Gizmos.DrawLine(lineManager.Colliders[_i][_j][1].PointD, lineManager.Colliders[_i][_j][1].PointA);

                    /*Gizmos.DrawSphere(lineManager.Colliders[_i][_j][0].PointA, 0.05f);
                    Gizmos.DrawSphere(lineManager.Colliders[_i][_j][0].PointB, 0.05f);
                    Gizmos.DrawSphere(lineManager.Colliders[_i][_j][0].PointC, 0.05f);
                    Gizmos.DrawSphere(lineManager.Colliders[_i][_j][0].PointD, 0.05f);

                    Gizmos.DrawSphere(lineManager.Colliders[_i][_j][1].PointA, 0.05f);
                    Gizmos.DrawSphere(lineManager.Colliders[_i][_j][1].PointB, 0.05f);
                    Gizmos.DrawSphere(lineManager.Colliders[_i][_j][1].PointC, 0.05f);
                    Gizmos.DrawSphere(lineManager.Colliders[_i][_j][1].PointD, 0.05f);*/
                }
            }
        }
    }
    #endregion
    #endregion
}