using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

[RequireComponent(typeof(ChildObjectsActivator))]
public class TutorialPage : MonoBehaviour
{
    private ChildObjectsActivator childObjectsActivator;

    [SerializeField] private VideoClip videoClip;

    public UnityEvent Activated;
    public UnityEvent Deactivated;

    void Awake()
    {
        childObjectsActivator = GetComponent<ChildObjectsActivator>();    
    }

    public void Activate(VideoPlayer videoPlayer)
    {
        if (videoClip != null)
            videoPlayer.clip = videoClip;
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
