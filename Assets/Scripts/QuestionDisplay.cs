using UnityEngine;
using TMPro;

public class QuestionDisplay : MonoBehaviour
{
    public TextMeshProUGUI questionText; // Referência para o texto da pergunta
    public TextMeshProUGUI answerText1;  // Referência para o texto da resposta 1
    public TextMeshProUGUI answerText2;  // Referência para o texto da resposta 2
    public TextMeshProUGUI answerText3;  // Referência para o texto da resposta 3
    public TextMeshProUGUI answerText4;  // Referência para o texto da resposta 4

    private Questions questionsScript;   

    void Start()
    {
        questionsScript = FindObjectOfType<Questions>();  // Localiza o script Questions
        DisplayQuestion();  // Exibe a primeira pergunta
    }

    // Função para exibir a pergunta e as respostas
    public void DisplayQuestion()
    {
        Question question = questionsScript.GetRandomUnansweredQuestion();  // Obtém uma pergunta aleatória

        if (question != null)
        {
            // Exibe a pergunta
            questionText.text = question.Text;

            // Exibe as respostas
            answerText1.text = "A) " + question.Items[0];
            answerText2.text = "B) " + question.Items[1];
            answerText3.text = "C) " + question.Items[2];
            answerText4.text = "D) " + question.Items[3];
        }
    }

    // Função para verificar se a resposta está correta
    public void CheckAnswer(int answerIndex)
    {
        Question question = questionsScript.GetQuestionById(1);  // Para exemplo, use o ID correto aqui

        if (questionsScript.IsCorrectAnswer(question.Id, answerIndex))
        {
            Debug.Log("Resposta correta!");
        }
        else
        {
            Debug.Log("Resposta errada.");
        }
    }
}