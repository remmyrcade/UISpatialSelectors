using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpatialSelectableEventHandlerExample : MonoBehaviour
{
    [SerializeField] private AudioClip _hoverBeginAudio;
    [SerializeField] private AudioClip _hoverEndAudio;
    [SerializeField] private AudioClip _clickAudio;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayHoverEndSFX()
    {
        Debug.Log(string.Format("{0} button hover end!", gameObject.name));

        if (_hoverEndAudio != null) _audioSource.PlayOneShot(_hoverEndAudio);
    }

    public void PlayHoverBeginSFX()
    {
        Debug.Log(string.Format("{0} button hover begin!", gameObject.name));

        if (_hoverBeginAudio != null) _audioSource.PlayOneShot(_hoverBeginAudio);
    }

    public void PlayClickedSFX()
    {
        Debug.Log(string.Format("{0} button clicked!", gameObject.name));

        if (_clickAudio != null) _audioSource.PlayOneShot(_clickAudio);
    }
}
