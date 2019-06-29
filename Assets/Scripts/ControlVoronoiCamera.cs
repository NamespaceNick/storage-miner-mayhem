using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlVoronoiCamera : MonoBehaviour {

    // camera offset
    public Vector3 offset = new Vector3(0, 0, -10f);
    // distance players must be apart to trigger splitscreen
    public float splitscreenDistanceMultiplier = 1.5f;
    // distance by which splitscreen cameras are centered on their players
    public float centerCameraDistanceMultiplier = 2.5f;
    // amount of parallax with 1 representing no parallax and infinity being maximum
    public float parallaxModifier = 1.5f;
    // size of all cameras
    public float cameraSize = 20f;
    // affects how wide the splitscreen divider will be
    public float dividerScaleMultiplier = 0.01f;
    public float maxDividerSize = 2f;
    // speed at which camera will lerp to reticles
    public float lerpSpeed = 0.05f;
    // cameras and canvas layer renderer
    public GameObject backgroundCameraPrefab, voronoiCameraPrefab, cameraContainerPrefab, voronoiCanvasPrefab, voronoiRendererPrefab, splitscreenDividerPrefab;
    // names of objects to search for
    public string playerName1 = "Avatar 1";
    public string playerName2 = "Avatar 2";

    GameObject cam1, cam2, player1, player2, vRenderer, divider;
    GrapplingHook grapple1, grapple2;
    Vector3 screenCenter1, screenCenter2, aimOffset1, aimOffset2;

    void Start() {
        // find players and reticles
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players) {
            if(player.name == playerName1) {
                player1 = player;
            }
            if(player.name == playerName2) {
                player2 = player;
            }
        }
        grapple1 = player1.GetComponent<GrapplingHook>();
        grapple2 = player2.GetComponent<GrapplingHook>();

        // create main camera
        GameObject mainCamObj = Instantiate(voronoiCameraPrefab, transform);
        Camera mainCam = mainCamObj.GetComponent<Camera>();
        mainCam.orthographicSize = cameraSize;

        // set up main camera parallax background
        GameObject mainBCamObj = Instantiate(backgroundCameraPrefab, mainCamObj.transform);
        Camera mainBCam = mainBCamObj.GetComponent<Camera>();
        mainBCam.orthographicSize = cameraSize * parallaxModifier;
        mainBCam.enabled = true;

        // create camera container for screen shake
        cam1 = Instantiate(cameraContainerPrefab);
        cam2 = Instantiate(cameraContainerPrefab);

        // create cameras for splitscreen
        GameObject vCamObj1 = Instantiate(voronoiCameraPrefab, cam1.transform);
        GameObject vCamObj2 = Instantiate(voronoiCameraPrefab, cam2.transform);
        Camera vCam1 = vCamObj1.GetComponent<Camera>();
        Camera vCam2 = vCamObj2.GetComponent<Camera>();
        vCam1.enabled = true;
        vCam2.enabled = true;
        // create render textures
        RenderTexture mat1 = new RenderTexture(Screen.width, Screen.height, 24);
        RenderTexture mat2 = new RenderTexture(Screen.width, Screen.height, 24);
        // tell cameras to render to render textures
        vCam1.targetTexture = mat1;
        vCam2.targetTexture = mat2;
        // set camera sizes
        vCam1.orthographicSize = cameraSize;
        vCam2.orthographicSize = cameraSize;

        // set up split screen camera parallax backgrounds
        GameObject vBCamObj1 = Instantiate(backgroundCameraPrefab, vCamObj1.transform);
        GameObject vBCamObj2 = Instantiate(backgroundCameraPrefab, vCamObj2.transform);
        Camera vBCam1 = vBCamObj1.GetComponent<Camera>();
        Camera vBCam2 = vBCamObj2.GetComponent<Camera>();
        vBCam1.orthographicSize = cameraSize * parallaxModifier;
        vBCam2.orthographicSize = cameraSize * parallaxModifier;
        vBCam1.targetTexture = mat1;
        vBCam2.targetTexture = mat2;
        vBCam1.enabled = true;
        vBCam2.enabled = true;

        // put renderer in canvas
        GameObject canvas = Instantiate(voronoiCanvasPrefab);
        vRenderer = Instantiate(voronoiRendererPrefab, canvas.transform);
        divider = Instantiate(splitscreenDividerPrefab, canvas.transform);
        // tell renderers to render textures
        RawImage[] renderers = vRenderer.GetComponentsInChildren<RawImage>();
        renderers[0].texture = mat1;
        renderers[1].texture = mat2;

        // give players cameras for screen screen shake
        ControlScreenShake shake1 = player1.GetComponent<ControlScreenShake>();
        ControlScreenShake shake2 = player2.GetComponent<ControlScreenShake>();
        shake1.screens.Add(mainCamObj.GetComponent<ShakeScreen>());
        shake2.screens.Add(mainCamObj.GetComponent<ShakeScreen>());
        shake1.screens.Add(vCamObj1.GetComponent<ShakeScreen>());
        shake2.screens.Add(vCamObj2.GetComponent<ShakeScreen>());
    }

    void Update() {
        // calculate average position of each player and grappling hook reticles
        int sources = 0;
        Vector3 newAimOffset1 = Vector3.zero;
        if(grapple1.GetReticle(1) != null && grapple1.GetReticle(1).activeSelf) {
            newAimOffset1 += (grapple1.GetReticle(1).transform.position - player1.transform.position);
            sources++;
        }
        if(grapple1.GetReticle(2) != null && grapple1.GetReticle(2).activeSelf) {
            newAimOffset1 += (grapple1.GetReticle(2).transform.position - player1.transform.position);
            sources++;
        }
        if (sources > 0)
        {
            newAimOffset1 /= sources;
        }
        sources = 0;
        Vector3 newAimOffset2 = Vector3.zero;
        if(grapple2.GetReticle(1) != null && grapple2.GetReticle(1).activeSelf) {
            newAimOffset2 += (grapple2.GetReticle(1).transform.position - player2.transform.position);
            sources++;
        }
        if(grapple2.GetReticle(2) != null && grapple2.GetReticle(2).activeSelf) {
            newAimOffset2 += (grapple2.GetReticle(2).transform.position - player2.transform.position);
            sources++;
        }
        if (sources > 0)
        {
            newAimOffset2 /= sources;
        }
        // lerp screen centers to new screen centers
        aimOffset1 = Vector3.Lerp(aimOffset1, newAimOffset1, lerpSpeed);
        aimOffset2 = Vector3.Lerp(aimOffset2, newAimOffset2, lerpSpeed);
        screenCenter1 = player1.transform.position + (aimOffset1 / 2);
        screenCenter2 = player2.transform.position + (aimOffset2 / 2);
        // center of each split screen
        Vector3 center;
        // ratio of screen width to height
        float widthHeightRatio = 1f * Screen.width / Screen.height;
        // corners of camera
        Vector3 topLeft = new Vector3(-widthHeightRatio, 1f, 0f);
        Vector3 topRight = new Vector3(widthHeightRatio, 1f, 0f);
        Vector3 bottomLeft = new Vector3(-widthHeightRatio, -1f, 0f);
        Vector3 bottomRight = new Vector3(widthHeightRatio, -1f, 0f);
        // point between players
        Vector3 centerPoint = (screenCenter1 + screenCenter2) / 2;
        // direction from one player to the other
        Vector3 splitOrthogonal = screenCenter1 - centerPoint;
        // direction of split
        Vector3 split = Vector3.Cross(splitOrthogonal, Vector3.forward).normalized;
        // if split is intersecting with left and right of screen
        if(Mathf.Abs(split.x) > Mathf.Abs(split.y) * widthHeightRatio) {
            // intersecting points
            Vector3 splitIntersection1 = new Vector3(widthHeightRatio, split.y * (widthHeightRatio / split.x), 0f);
            Vector3 splitIntersection2 = -splitIntersection1;
            // if player1 is on the top
            if(splitOrthogonal.y > 0) {
                // location in center of each screen
                center = cameraSize * Centroid4(topLeft, topRight, splitIntersection1, splitIntersection2);
            }
            // if player1 is on the bottom
            else {
                // location in center of each screen
                center = cameraSize * Centroid4(bottomLeft, bottomRight, splitIntersection1, splitIntersection2);
            }
        }
        // if split is intersecting with top and bottom of screen
        else {
            // intersecting points
            Vector3 splitIntersection1 = new Vector3(split.x * (1f / split.y), 1f, 0f);
            Vector3 splitIntersection2 = -splitIntersection1;
            // if player1 is on the right
            if(splitOrthogonal.x > 0) {
                // location in center of each screen
                center = cameraSize * Centroid4(bottomRight, topRight, splitIntersection1, splitIntersection2);
            }
            // if player1 is on the left
            else {
                // location in center of each screen
                center = cameraSize * Centroid4(bottomLeft, topLeft, splitIntersection1, splitIntersection2);
            }
        }

        // feathered location for split screen cameras
        float featherAmount = ((screenCenter1 - screenCenter2).magnitude - splitscreenDistanceMultiplier * cameraSize) / (centerCameraDistanceMultiplier * cameraSize - splitscreenDistanceMultiplier * cameraSize);
        if(featherAmount < 0) {
            featherAmount = 0;
        }
        if(featherAmount > 1) {
            featherAmount = 1;
        }
        Vector3 feather = (1-featherAmount) * splitscreenDistanceMultiplier * cameraSize/2 * (screenCenter2 - screenCenter1).normalized - featherAmount * center;

        // set positions of all cameras
        // main camera should be between two players
        transform.position = (screenCenter1 + screenCenter2) / 2 + offset;
        // these cameras should smoothly transition according to feather
        cam1.transform.position = screenCenter1 + feather + offset;
        cam2.transform.position = screenCenter2 - feather + offset;
        // set orientation of renderer based on orientation of players, split should be perpendicular to vector between players
        vRenderer.transform.up = screenCenter2 - screenCenter1;
        divider.transform.up = vRenderer.transform.up;
        // scale divider according to distance between players
        float scale = dividerScaleMultiplier * (screenCenter1 - screenCenter2).magnitude;
        if(scale > maxDividerSize) {
            scale = maxDividerSize;
        }
        divider.transform.localScale = new Vector3(1f, scale, 1f);
        // if the screen should split, show the renderer
        if((screenCenter1 - screenCenter2).magnitude > splitscreenDistanceMultiplier * cameraSize) {
            vRenderer.SetActive(true);
            divider.SetActive(true);
        }
        else {
            vRenderer.SetActive(false);
            divider.SetActive(false);
        }
    }

    // centroid of quadrilateral formed by points v1, v2, v3, and v4
    Vector2 Centroid4(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4) {
        Vector2 c1 = Centroid3(v1, v2, v3);
        Vector2 c2 = Centroid3(v2, v3, v4);
        Vector2 c3 = Centroid3(v3, v4, v1);
        Vector2 c4 = Centroid3(v4, v1, v2);
        return Intersection(c1, c3, c2, c4);
    }

    // centroid of triangle formed by points v1, v2, and v3
    Vector2 Centroid3(Vector2 v1, Vector2 v2, Vector2 v3) {
        return Intersection(v1, (v2 + v3) / 2, v3, (v1 + v2) / 2);
    }

    // intersection between lines formed along points v1 v2 and v3 v4
    Vector2 Intersection(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4) {
        if(v1.x == v2.x && v3.x != v4.x) {
            return new Vector2(v1.x, (v3.y+v4.y)/2);
        }
        if(v1.x != v2.x && v3.x == v4.x) {
            return new Vector2(v3.x,  (v1.y+v2.y)/2);
        }
        float m1 = (v2.y-v1.y)/(v2.x-v1.x);
        float m2 = (v4.y-v3.y)/(v4.x-v3.x);
        if(m1 != m2) {
            float x = (m1*v1.x - v1.y - m2*v3.x + v3.y) / (m1 - m2);
            float y = m1*(x - v1.x) + v1.y;
            return new Vector2(x, y);
        }
        return Vector3.zero;
    }
}
