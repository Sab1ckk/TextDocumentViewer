using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFormatController : MonoBehaviour
{
    [Header("Основное текстовое поле")]
    public TMP_InputField mainInput;

    [Header("Размер текста")]
    public TMP_InputField textSize;

    [Header("Выбор шрифта")]
    public TMP_Dropdown fontDropdown;

    [Header("Доступные шрифты")]
    [SerializeField] private List<TMP_FontAsset> fonts = new List<TMP_FontAsset>();

    private bool isBold = false;
    private bool isItalic = false;
    private bool isUnderline = false;

    private void Start()
    {
        InitializeFontDropdown();

        fontDropdown.onValueChanged.AddListener(ChangeFont);
        textSize.onEndEdit.AddListener(ChangeTextSize);

        //boldButton.onClick.AddListener(ToggleBold);
        //italicButton.onClick.AddListener(ToggleItalic);
        //underlineButton.onClick.AddListener(ToggleUnderline);

        // Начальный размер
        textSize.text = mainInput.textComponent.fontSize.ToString();
    }

    private void InitializeFontDropdown()
    {
        fontDropdown.ClearOptions();

        List<string> fontNames = new List<string>();

        foreach (TMP_FontAsset font in fonts)
        {
            fontNames.Add(font.name);
        }

        fontDropdown.AddOptions(fontNames);

        if (fonts.Count > 0)
        {
            fontDropdown.value = 0;
            ChangeFont(0);
        }
    }

    public void ChangeFont(int index)
    {
        if (index < 0 || index >= fonts.Count)
            return;

        TMP_FontAsset selectedFont = fonts[index];

        mainInput.textComponent.font = selectedFont;
    }

    private void ChangeTextSize(string value)
    {
        if (float.TryParse(value, out float size))
        {
            if (size <= 0)
                return;

            mainInput.textComponent.fontSize = size;
        }
    }

    public void ToggleBold()
    {
        isBold = !isBold;

        UpdateFontStyle();
    }

    public void ToggleItalic()
    {
        isItalic = !isItalic;

        UpdateFontStyle();
    }

    public void ToggleUnderline()
    {
        isUnderline = !isUnderline;

        UpdateFontStyle();
    }

    private void UpdateFontStyle()
    {
        FontStyles style = FontStyles.Normal;

        if (isBold)
            style |= FontStyles.Bold;

        if (isItalic)
            style |= FontStyles.Italic;

        if (isUnderline)
            style |= FontStyles.Underline;

        mainInput.textComponent.fontStyle = style;
    }
}
