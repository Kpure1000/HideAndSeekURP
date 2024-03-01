using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class TouchPad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    [SerializeField] private float moveThreshold = 1;
    [SerializeField] private float deadZone = 0;
    [SerializeField] protected RectTransform tracer = null;

    public float Horizontal { get => input.x; } 
    public float Vertical { get => input.y; }

    public Vector2 Position;
    public Vector2 DeltaPosition { get => deltaPosition; }

    public float MoveThreshold
    {
        get { return moveThreshold; }
        set { moveThreshold = Mathf.Abs(value); }
    }

    public float DeadZone
    {
        get { return deadZone; }
        set { deadZone = Mathf.Abs(value); }
    }

    private RectTransform baseRect = null;

    private Canvas canvas;
    private Camera cam;

    private Vector2 input = Vector2.zero;
    private Vector2 lastPosition = Vector2.zero;
    private Vector2 deltaPosition = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        DeadZone = deadZone;
        baseRect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
            Debug.LogError("The TouchLook is not placed inside a canvas");

        tracer.gameObject.SetActive(false);
        Vector2 center = new Vector2(0.5f, 0.5f);
        tracer.pivot = center;
    }

    
    public void OnDrag(PointerEventData eventData)
    {
        cam = null;
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            cam = canvas.worldCamera;

        Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, tracer.position);

        Vector2 radius = tracer.sizeDelta / 2;
        input = (eventData.position - position) / (radius * canvas.scaleFactor);

        HandleInput(input.magnitude, input.normalized, radius, cam);

        position = RectTransformUtility.WorldToScreenPoint(cam, eventData.position);
        Position = position;
    }

    private void Update()
    {
        deltaPosition = Position - lastPosition;

        lastPosition = Position;
    }

    protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > MoveThreshold)
        {
            Vector2 difference = normalised * (magnitude - MoveThreshold) * radius;
            tracer.anchoredPosition += difference;
        }
        if (magnitude > deadZone)
        {
            if (magnitude > 1)
            {
                input = normalised;
            }
        }
        else
        {
            input = Vector2.zero;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        lastPosition = RectTransformUtility.WorldToScreenPoint(cam, eventData.position);
        deltaPosition = Vector2.zero;

        tracer.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        tracer.gameObject.SetActive(true);
        
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        tracer.gameObject.SetActive(false);
        input = Vector2.zero;

        deltaPosition = Vector2.zero;
    }
    private Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        Vector2 localPoint = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint))
        {
            Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
            return localPoint - (tracer.anchorMax * baseRect.sizeDelta) + pivotOffset;
        }
        return Vector2.zero;
    }

}
