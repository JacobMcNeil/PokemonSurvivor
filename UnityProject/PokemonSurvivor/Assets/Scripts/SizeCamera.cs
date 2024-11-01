using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeCamera : MonoBehaviour
{
    Camera camera;
    int startHeight;
    int startWidth;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        startWidth = Screen.width;
        startHeight = Screen.height;
        if (camera.scaledPixelHeight > camera.scaledPixelWidth)
        {
            //camera.rect.Set(1, (camera.scaledPixelWidth * 16 / 9) / camera.scaledPixelHeight, 0, 0);
            camera.rect = new Rect(0, (1 - startWidth * 1f / startHeight) * .5f, 1, startWidth * 1f / startHeight);
            //Debug.Log("here");
        }
        if (camera.scaledPixelWidth > camera.scaledPixelHeight)
        {
            //camera.rect.Set(1, (camera.scaledPixelWidth * 16 / 9) / camera.scaledPixelHeight, 0, 0);
            camera.rect = new Rect((1 - startHeight * 1f / startWidth) * .5f, 0, startHeight * 1f / startWidth, 1);
            //Debug.Log("here");
        }
        //camera.aspect = 16 / 9;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(camera.scaledPixelHeight);
        //Debug.Log(camera.scaledPixelWidth);
        startWidth = Screen.width;
        startHeight = Screen.height;
        if (camera.scaledPixelHeight > camera.scaledPixelWidth)
        {
            //camera.rect.Set(1, (camera.scaledPixelWidth * 16 / 9) / camera.scaledPixelHeight, 0, 0);
            camera.rect = new Rect(0, (1 - startWidth * 1f / startHeight) * .5f, 1,startWidth * 1f/startHeight);
            Debug.Log("here");
        }
        if (camera.scaledPixelWidth > camera.scaledPixelHeight)
        {
            //camera.rect.Set(1, (camera.scaledPixelWidth * 16 / 9) / camera.scaledPixelHeight, 0, 0);
            camera.rect = new Rect((1 - startHeight * 1f / startWidth) * .5f, 0, startHeight * 1f / startWidth, 1);
            Debug.Log("here");
        }
    }
    private void OnPreCull()
    {
        GL.Clear(true, true, Color.black);
    }
}
