using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using Unity.VisualScripting;

public class DocumentManager : MonoBehaviour
{
    [Header("UI Components")]
    public TMP_InputField textInputField;
    public TMP_InputField docNameField;
    public TMP_Text statusText;
    public TMP_Text titleText;

    [Header("Windows")]
    public GameObject createWindow;
    public GameObject mainWindow;

    [Header("Document Settings")]
    public string defaultDocumentsPath = "Documents";

    private TextDocument currentDocument;
    private string currentFilePath = "";

    //void Awake()
    //{
    //    InitializeWindow();   
    //}
    void Start()
    {
        // Создаем новый документ при старте
        CreateNewDocument();


        // Обновляем интерфейс
        UpdateUI();
    }

    //private void InitializeWindow()
    //{
    //    createWindow.SetActive(false);
    //}

    // Создание нового документа

    //public void CreateDocumentDialog()
    //{
    //    createWindow.SetActive(true);
    //}

    public void CreateNewDocument()
    {
        currentDocument = new TextDocument();
        currentFilePath = "";
        UpdateUI();

        Debug.Log("Создан новый документ");

        createWindow.SetActive(false);
    }

    // Загрузка документа
    public void LoadDocument()
    {
        // В реальном приложении здесь будет диалог выбора файла
        string testFilePath = EditorUtility.OpenFilePanel("Overwrite with txt", "", "txt, docx, doc");

        if (File.Exists(testFilePath))
        {
            try
            {
                currentDocument = new TextDocument();
                currentDocument.content = File.ReadAllText(testFilePath);
                currentDocument.documentName = Path.GetFileName(testFilePath);
                currentFilePath = testFilePath;
                currentDocument.UpdateStatistics();

                UpdateUI();
                Debug.Log("Документ загружен: " + testFilePath);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Ошибка загрузки: " + e.Message);
            }
        }
        else
        {
            Debug.LogWarning("Тестовый файл не найден: " + testFilePath);
        }
    }

    // Сохранение документа
    public void SaveDocument()
    {
        if (string.IsNullOrEmpty(currentFilePath))
        {
            SaveDocumentAs();
            return;
        }

        SaveToFile(currentFilePath);
    }

    // Сохранение документа как...
    public void SaveDocumentAs()
    {
        // В реальном приложении здесь будет диалог сохранения
        string newFilePath = EditorUtility.SaveFilePanel(currentDocument.documentName, currentFilePath, "Новый документ", "txt");

        SaveToFile(newFilePath);
    }

    private void SaveToFile(string filePath)
    {
        try
        {
            // Создаем директорию если не существует
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Сохраняем файл
            File.WriteAllText(filePath, currentDocument.content);
            currentFilePath = filePath;
            currentDocument.documentName = Path.GetFileName(filePath);

            UpdateUI();
            Debug.Log("Документ сохранен: " + filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Ошибка сохранения: " + e.Message);
        }
    }

    // Обновление содержимого документа из UI
    public void OnTextChanged()
    {
        if (currentDocument != null && textInputField != null)
        {
            currentDocument.content = textInputField.text;
            currentDocument.UpdateStatistics();
            UpdateStatus();
        }
    }

    // Обновление интерфейса
    private void UpdateUI()
    {
        if (textInputField != null)
        {
            textInputField.text = currentDocument.content;
        }

        if (titleText != null)
        {
            titleText.text = currentDocument.documentName + (string.IsNullOrEmpty(currentFilePath) ? " *" : "");
        }

        UpdateStatus();
    }

    // Обновление статусной строки
    private void UpdateStatus()
    {
        if (statusText != null && currentDocument != null)
        {
            statusText.text = $"Символов: {currentDocument.characterCount} | Слов: {currentDocument.wordCount} | Строк: {currentDocument.lineCount}";
        }
    }

    // Поиск текста
    public void SearchText(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm) || textInputField == null)
            return;

        // Простая реализация поиска - выделение текста
        string content = textInputField.text;
        if (content.Contains(searchTerm))
        {
            int index = content.IndexOf(searchTerm);
            textInputField.Select();
            textInputField.selectionAnchorPosition = index;
            textInputField.selectionFocusPosition = index + searchTerm.Length;
        }
    }
}
