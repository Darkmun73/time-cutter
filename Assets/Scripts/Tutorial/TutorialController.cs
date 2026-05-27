using UnityEngine;
using UnityEngine.Video;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    void Awake()
    {
        videoPlayer.clip = null;
    }
}
