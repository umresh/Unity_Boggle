using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Grid tile
/// </summary>
public class LettersTile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform answerRectTransform;
    [HideInInspector]
    public char assignedLetter;
    Transform initialParent;
    public Vector3 initialPosition;
    RectTransform draggingRect;
    int siblingIndex = 0;
    GameObject _instantiated;
    bool isDraging = false;

    void Start() => SetResetOriginalPositions();
    void OnEnable() => SetResetOriginalPositions();
    public void SetResetOriginalPositions()
    {
        draggingRect = GetComponent<RectTransform>();
        initialParent = transform.parent;
        initialPosition = transform.position;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_instantiated)
        {
            siblingIndex = _instantiated.transform.GetSiblingIndex();
            _instantiated.transform.SetParent(transform.parent.parent);
            _instantiated.transform.SetAsLastSibling();
            _instantiated.transform.localScale = Vector3.one;
            isDraging = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_instantiated == null)
            return;
        bool _bool = IsRectTransformInsideSreen(_instantiated.GetComponent<RectTransform>());
        if (_bool && Manager.Instance.AddCharaterToWord(assignedLetter))
        {
            _instantiated.transform.SetParent(answerRectTransform.transform);
        }
        else
        {
            if (_instantiated.transform.parent != initialParent)
                Manager.Instance.removeCharaterToWord(siblingIndex);
            _instantiated.transform.SetParent(initialParent);
            _instantiated.transform.position = initialPosition;
            Destroy(_instantiated);
        }
        isDraging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_instantiated == null)
            return;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(draggingRect, eventData.position, eventData.pressEventCamera, out Vector3 desiredPosition))
            _instantiated.transform.position = desiredPosition;
        else
        {
            _instantiated.transform.SetParent(initialParent);
            _instantiated.transform.position = initialPosition;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (transform.parent == initialParent)
        {
            _instantiated = Instantiate(this.gameObject) as GameObject;
            _instantiated.transform.SetParent(transform.parent, false);
            _instantiated.transform.SetAsLastSibling();
        }
        else
        {
            _instantiated = this.gameObject;
            _instantiated.transform.SetParent(transform.parent, false);
            _instantiated.transform.SetAsLastSibling();
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (_instantiated == null)
            return;
        if (!isDraging && _instantiated.transform.parent == initialParent)
            Destroy(_instantiated);
    }
    private bool IsRectTransformInsideSreen(RectTransform _rectTransform) => answerRectTransform.IsRectOverlapping(_rectTransform);
    public void ResetToInitialPositionTile()
    {
        assignedLetter = ' ';
    }

}
public static class RectTransformExtensions
{
    public static bool IsRectOverlapping(this RectTransform firstRect, RectTransform secondRect)
    {
        Rect rect1 = new Rect(firstRect.localPosition.x, firstRect.localPosition.y, firstRect.rect.width, firstRect.rect.height);
        Rect rect2 = new Rect(secondRect.localPosition.x, secondRect.localPosition.y, secondRect.rect.width, secondRect.rect.height);
        return rect1.Overlaps(rect2);
    }
}