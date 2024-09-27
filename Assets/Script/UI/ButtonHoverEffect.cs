using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject leftDecor;   // Thanh decor bên trái
    public GameObject rightDecor;  // Thanh decor bên phải
    public TextMeshProUGUI buttonText;

    public Color hoverColor = new Color(1f, 1f, 1f, 1f);  // Màu phát sáng
    private Color originalColor;   // Màu gốc của Button


    private RectTransform rectTransform;
    private Vector3 originalScale;
    public float hoverScale = 1.2f;
    public float animationDuration = 0.2f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
        // Tắt hai thanh decor ban đầu
        leftDecor.SetActive(false);
        rightDecor.SetActive(false);

        // Lưu màu gốc của Button
        originalColor = buttonText.color;
    }

    // Khi chuột vào Button
    public void OnPointerEnter(PointerEventData eventData)
    {
        leftDecor.SetActive(true);
        rightDecor.SetActive(true);

        // Tạo hiệu ứng phát sáng
        buttonText.color = hoverColor;
        rectTransform.DOScale(originalScale * hoverScale, animationDuration).SetEase(Ease.OutBounce);
    }

    // Khi chuột rời khỏi Button
    public void OnPointerExit(PointerEventData eventData)
    {
        leftDecor.SetActive(false);
        rightDecor.SetActive(false);
        rectTransform.DOScale(originalScale, animationDuration).SetEase(Ease.InBounce);
        // Trả về màu gốc
        buttonText.color = originalColor;
    }
}
