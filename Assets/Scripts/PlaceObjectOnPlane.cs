using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

public class PlaceObjectOnPlane : MonoBehaviour
{
    public GameObject objectPrefab;

    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    private GameObject spawnedObject;
    public UnityEvent onObjectSpawned;

    void Start()
    {
        // Grab the raycast manager
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        // If there are no touches on the screen
        if (Input.touchCount == 0)
            // Nothing to see here
            return;

        // Get the first touch position
        var touchPosition = Input.GetTouch(0).position;

        // Check if there is a detected plane at this touch position
        if(raycastManager.Raycast(touchPosition, raycastHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            // Get the hit pose on the closest detected plane
            var hitPose = raycastHits[0].pose;

            // If we haven't already spawned an object
            if(spawnedObject == null)
            {
                // Spawn the object at the touched position on the detected plane
                spawnedObject = Instantiate(objectPrefab, hitPose.position, hitPose.rotation);
                spawnedObject.AddComponent<ARAnchor>();

                // Trigger the on object spawned event
                onObjectSpawned.Invoke();
            }
            // Otherwise, we've already spawned an object
            else
            {
                // Move the spawned object to the touched position on the detected plane
                spawnedObject.transform.position = hitPose.position;
                spawnedObject.transform.rotation = hitPose.rotation;
            }
        }
    }
}
