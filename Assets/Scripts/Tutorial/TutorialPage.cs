using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

[RequireComponent(typeof(ChildObjectsActivator))]
public class TutorialPage : MonoBehaviour
{
    private ChildObjectsActivator childObjectsActivator;

    //[SerializeField] private VideoClip videoClip;
    [SerializeField] private string videoClipName = null;

    public UnityEvent Activated;
    public UnityEvent Deactivated;

    void Awake()
    {
        childObjectsActivator = GetComponent<ChildObjectsActivator>();
        
    }

    public void Activate(VideoPlayer videoPlayer)
    {
        if (videoClipName != null)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoClipName);
            videoPlayer.url = videoPath;
            //videoPlayer.clip = videoClip;
        }
        childObjectsActivator.SetActive(true);
        Activated?.Invoke();
    }

    public void Deactivate(VideoPlayer videoPlayer)
    {
        videoPlayer.clip = null;
        childObjectsActivator.SetActive(false);
        Deactivated?.Invoke();
    }
}
