using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceObjectsOnPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }
    public Camera camera; 
    
    public enum CubeFace {
        Left,
        Bottom,
        Back,
        Right,
        Top,
        Front,
    }
    /// <summary>
    /// Invoked whenever an object is placed in on a plane.
    /// </summary>
    public static event Action onPlacedObject;

    ARRaycastManager m_RaycastManager;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    
    [SerializeField]
    int m_MaxNumberOfObjectsToPlace = 1;

    int m_NumberOfPlacedObjects = 0;

    [SerializeField]
    bool m_CanReposition = true;

    public bool canReposition
    {
        get => m_CanReposition;
        set => m_CanReposition = value;
    }

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = s_Hits[0].pose;

                    if (m_NumberOfPlacedObjects < m_MaxNumberOfObjectsToPlace)
                    {
                        spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                        GetFaceToward(spawnedObject, camera.transform.position);
                        m_NumberOfPlacedObjects++;
                    }
                    else
                    {
                        if (m_CanReposition)
                        {
                            spawnedObject.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                            GetFaceToward(spawnedObject, camera.transform.position);
                        }
                    }
                    
                    if (onPlacedObject != null)
                    {
                        onPlacedObject();
                    }
                }
            }
        }
        if(spawnedObject != null) {
            GetFaceToward(spawnedObject, camera.transform.position);
        }
    }
    /// <summary>
    /// Change the Color of Cube Object
    /// </summary>
    void ChangeTexture(GameObject parent, CubeFace face) {
        GameObject cube = parent.transform.Find("Cube").gameObject;
        GameObject face_bottom = cube.transform.Find("Bottom").gameObject;
        GameObject face_top = cube.transform.Find("Top").gameObject;
        GameObject face_left = cube.transform.Find("Left").gameObject;
        GameObject face_front = cube.transform.Find("Front").gameObject;
        GameObject face_right = cube.transform.Find("Right").gameObject;
        GameObject face_back = cube.transform.Find("Back").gameObject;
        face_front.GetComponent<Renderer>().material.color = Color.blue;
        face_right.GetComponent<Renderer>().material.color = Color.blue;
        face_left.GetComponent<Renderer>().material.color = Color.blue;           
        face_back.GetComponent<Renderer>().material.color = Color.blue;
        face_top.GetComponent<Renderer>().material.color = Color.blue;          
        face_bottom.GetComponent<Renderer>().material.color = Color.blue;
        switch(face) {
            case CubeFace.Left: 
                face_left.GetComponent<Renderer>().material.color = Color.red;
                break;
            case CubeFace.Bottom: 
                face_bottom.GetComponent<Renderer>().material.color = Color.red;
                break;
            case CubeFace.Back: 
                face_back.GetComponent<Renderer>().material.color = Color.red;
                break;
            case CubeFace.Right: 
                face_right.GetComponent<Renderer>().material.color = Color.red;
                break;
            case CubeFace.Top: 
                face_top.GetComponent<Renderer>().material.color = Color.red;
                break;
            case CubeFace.Front: 
                face_front.GetComponent<Renderer>().material.color = Color.red;
                break;
            default: break;
        }    
    }
    /// <summary>
    /// Realtime Call this function for getting the closed face of Cube from Camera
    /// </summary>
    void GetFaceToward(GameObject cube, Vector3 observerPosition) {
        var toObserver = cube.transform.InverseTransformPoint(observerPosition);
        var absolute = new Vector3(
                        Mathf.Abs(toObserver.x),
                        Mathf.Abs(toObserver.y),
                        Mathf.Abs(toObserver.z)
                    );                          
        if (absolute.x >= absolute.y) {
            if (absolute.x >= absolute.z) {
                ChangeTexture(cube, toObserver.x > 0 ? CubeFace.Right : CubeFace.Left);
                return;
            } else {
                ChangeTexture(cube, toObserver.x > 0 ? CubeFace.Front : CubeFace.Back);
                return;
            }
        } else if (absolute.y >= absolute.z) {
            ChangeTexture(cube, toObserver.y > 0 ? CubeFace.Top : CubeFace.Bottom);
            return;
        } else {
            ChangeTexture(cube, toObserver.x > 0 ? CubeFace.Front : CubeFace.Back);
            return;
        }
    }
}
