using UnityEngine;
using UnityEngine.XR.MagicLeap;
using UISpatialSelectors;

public class HandTrackingUICollision : MonoBehaviour
{
    [SerializeField] private UICollisionSelector _fingerPrefab;
    [SerializeField] private float _hoverRadius = 0.3f;
    [SerializeField] private float _clickerRadius = 0.1f;
    [SerializeField] private bool _indexEnabled = true;
    [SerializeField] private bool _thumbEnabled;
    [SerializeField] private bool _middleEnabled;
    [SerializeField] private bool _ringEnabled;
    [SerializeField] private bool _pinkyEnabled;

    private struct Hand
    {
        public UICollisionSelector index;
        public UICollisionSelector thumb;
        public UICollisionSelector middle;
        public UICollisionSelector ring;
        public UICollisionSelector pinky;
    }

    private Hand _leftHand;
    private Hand _rightHand;

    private void Start()
    {
        InitializeHands();
    }

    private void Update()
    {
        if (MLHands.IsStarted)
        {
            UpdateHand(MLHands.Left, _leftHand);
            UpdateHand(MLHands.Right, _rightHand);
        }
    }

    private void UpdateHand(MLHand mlHand, Hand hand)
    {
        // Index
        hand.index.transform.position = mlHand.Index.Tip.Position;
        hand.index.gameObject.SetActive(mlHand.IsVisible && _indexEnabled);
        // Thumb
        hand.thumb.transform.position = mlHand.Thumb.Tip.Position;
        hand.thumb.gameObject.SetActive(mlHand.IsVisible && _thumbEnabled);
        // Middle
        hand.middle.transform.position = mlHand.Middle.Tip.Position;
        hand.middle.gameObject.SetActive(mlHand.IsVisible && _middleEnabled);
        // Ring
        hand.ring.transform.position = mlHand.Ring.Tip.Position;
        hand.ring.gameObject.SetActive(mlHand.IsVisible && _ringEnabled);
        // Pinky
        hand.pinky.transform.position = mlHand.Pinky.Tip.Position;
        hand.pinky.gameObject.SetActive(mlHand.IsVisible && _pinkyEnabled);
    }

    private void InitializeHands()
    {
        MLResult result = MLHands.Start();
        if (!result.IsOk)
        {
            Debug.LogErrorFormat("Error: HandTrackingUICollision failed starting MLHands, disabling script. Reason: {0}", result);
            enabled = false;
            return;
        }

        _leftHand = CreateHand();
        _rightHand = CreateHand();
    }

    private Hand CreateHand()
    {
        Hand hand = new Hand
        {
            index = CreateFinger(_indexEnabled),
            thumb = CreateFinger(_thumbEnabled),
            middle = CreateFinger(_middleEnabled),
            ring = CreateFinger(_ringEnabled),
            pinky = CreateFinger(_pinkyEnabled),
        };
        return hand;
    }

    private UICollisionSelector CreateFinger(bool isEnabled)
    {
        UICollisionSelector finger = Instantiate(_fingerPrefab);
        finger.Setup(_hoverRadius, _clickerRadius);
        finger.gameObject.SetActive(isEnabled);
        return finger;
    }
}
