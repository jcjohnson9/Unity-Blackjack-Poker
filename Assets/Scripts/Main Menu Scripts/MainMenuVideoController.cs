using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for manipulating UI elements like RawImage
using UnityEngine.Video; // Required for video playback functionality

public class MainMenuVideoController : MonoBehaviour
{
    [Tooltip("The VideoPlayer component playing the video.")]
    public VideoPlayer videoPlayer;

    [Tooltip("The AudioSource component for playing audio.")]
    public AudioSource audioSource;

    [Tooltip("The RawImage component used to display the video.")]
    public RawImage videoDisplay;

    [Tooltip("Time in seconds to wait before hiding the video display.")]
    public float hideVideoTime = 9f;

    private bool videoSkipped = false; // Flag to indicate if the video has been skipped to prevent multiple skips.

    // Start is called before the first frame update. Initializes video playback and schedules audio start and video display hide.
    void Start()
    {
        videoPlayer.Play(); // Start playing the video at scene start.
        Invoke("StartAudio", 9f); // Schedule the audio to start after a delay.
        Invoke("HideVideoDisplay", hideVideoTime); // Schedule hiding the video display.
    }

    // Update is called once per frame. Checks for user input to skip the video.
    void Update()
    {
        if (Input.anyKeyDown && !videoSkipped) // Allow skipping the video with any key/mouse press.
        {
            SkipVideo();
        }
    }

    // Starts playing the associated audio if it's not already playing.
    void StartAudio()
    {
        if (!audioSource.isPlaying) // Check if the audio is not playing before starting it.
        {
            audioSource.Play();
        }
    }

    // Skips video playback, immediately stops the video, continues playing audio, and hides the video display.
    void SkipVideo()
    {
        videoSkipped = true; // Mark the video as skipped to prevent re-triggering.
        videoPlayer.Stop(); // Stop the video playback.
        StartAudio(); // Ensure audio continues to play.
        HideVideoDisplay(); // Hide the video display immediately.
    }

    //Hides the RawImage component used to display the video.
    void HideVideoDisplay()
    {
        if (videoDisplay != null) // Ensure the RawImage component is assigned.
        {
            videoDisplay.enabled = false; // Disable the RawImage to hide the video.
        }
    }

    //Callback for when the video finishes playing. Utilizes skip functionality to ensure consistent end behavior.
    /// <param name="vp">The VideoPlayer instance that reached the end of the video.</param>
    void EndReached(VideoPlayer vp)
    {
        SkipVideo(); // Utilize the skip functionality to hide the video display and ensure audio continues.
    }
}