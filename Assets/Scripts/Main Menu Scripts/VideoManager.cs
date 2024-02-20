using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign in the inspector
    public RawImage rawImage; // Assign the Raw Image used to display the video
    private static bool videoHasPlayed = false;

    void Start()
    {
        // Check if the video has already been played
        if (videoHasPlayed)
        {
            // If the video has played, hide the Raw Image and do not play the video again
            rawImage.enabled = false;
            videoPlayer.gameObject.SetActive(false); // Optionally disable the VideoPlayer to save resources
        }
        else
        {
            // Play the video and mark it as played
            videoPlayer.Play();
            videoHasPlayed = true;
        }
    }
}