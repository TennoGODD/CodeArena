using UnityEngine;
using UnityEngine.UI;
using Mono.CSharp;
using System;
using System.IO;
using System.Text;
using TMPro;

public class CodeExecutor : MonoBehaviour
{
    [SerializeField] private TMP_InputField codeInputField; // Поле ввода кода
    [SerializeField] private Button runButton; // Кнопка запуска
    [SerializeField] private TextMeshProUGUI outputText; // Окно вывода результатов
    [SerializeField] private float timeLimit = 60f; // Время на выполнение
    [SerializeField] private TMP_Text timerText; // Текст таймера

    private float timer;
    private bool isRunning = false;

    void Start()
    {
        runButton.onClick.AddListener(ExecuteCode);
        timer = 0f; // Таймер будет отсчитывать время с момента запуска
        UpdateTimerText();
    }

    void Update()
    {
        if (isRunning)
        {
            timer += Time.deltaTime; // Увеличиваем прошедшее время
            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        // Показываем прошедшее время
        timerText.text = $"⏱ Прошло: {Mathf.FloorToInt(timer)} сек";
    }

    void ExecuteCode()
    {
        // Если код уже выполняется, не даем запустить его снова
        if (isRunning)
            return;

        outputText.text = "";
        isRunning = true;
        runButton.interactable = false; // Блокируем кнопку, пока код выполняется

        var sb = new StringBuilder();
        var writer = new StringWriter(sb);
        Console.SetOut(writer);
        Console.SetError(writer);

        try
        {
            var evaluator = new Evaluator(new CompilerContext(new CompilerSettings(), new ConsoleReportPrinter()));
            evaluator.ReferenceAssembly(typeof(UnityEngine.Vector3).Assembly);
            evaluator.Run("using System;");
            evaluator.Run("public class Script { public static void Run() { " + codeInputField.text + " } }");
            evaluator.Run("Script.Run();");

            writer.Flush();
            outputText.text = sb.ToString();
        }
        catch (Exception ex)
        {
            outputText.text = $"❌ Ошибка: {ex.Message}";
        }
        finally
        {
            isRunning = false; // Завершаем выполнение кода
            runButton.interactable = true; // Разблокируем кнопку
        }
    }
}
