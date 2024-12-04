using UnityEngine;
using TMPro;

public class QuestionDisplay : MonoBehaviour
{
    public TextMeshProUGUI questionText; // Referência para o texto da pergunta
    public TextMeshProUGUI answerText1;  // Referência para o texto da resposta 1
    public TextMeshProUGUI answerText2;  // Referência para o texto da resposta 2
    public TextMeshProUGUI answerText3;  // Referência para o texto da resposta 3
    public TextMeshProUGUI answerText4;  // Referência para o texto da resposta 4

    public void DisplayQuestion(Question currentQuestion)
    {
        if (currentQuestion != null)
        {
            // Exibe a pergunta
            questionText.text = currentQuestion.Text;

            // Exibe as respostas
            answerText1.text = "A) " + currentQuestion.Items[0];
            answerText2.text = "B) " + currentQuestion.Items[1];
            answerText3.text = "C) " + currentQuestion.Items[2];
            answerText4.text = "D) " + currentQuestion.Items[3];
        }
        else
        {
            Debug.LogError("Nenhuma pergunta foi passada para exibição!");
        }

        
    }
}