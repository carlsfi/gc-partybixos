using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Questions : MonoBehaviour
{
    private readonly List<Question> questions = new List<Question>();
    private readonly IList<Question> history = new List<Question>();

    void Awake()
    {
        LoadQuestions();
    }

    private void LoadQuestions()
    {
        questions.AddRange(
            new List<Question>()
            {
                new(
                    id: 1,
                    text: "Qual é a capital do Brasil?",
                    items: new List<string>
                    {
                        "São Paulo",
                        "Rio de Janeiro",
                        "Brasília",
                        "Salvador"
                    },
                    answer: 2,
                    difficulty: Difficulty.Easy
                ),
                new(
                    id: 2,
                    text: "Qual é o maior planeta do nosso sistema solar?",
                    items: new List<string> { "Terra", "Júpiter", "Marte", "Vênus" },
                    answer: 1,
                    difficulty: Difficulty.Easy
                ),
                new(
                    id: 3,
                    text: "Qual é o maior animal terrestre?",
                    items: new List<string> { "Elefante", "Girafa", "Leão", "Baleia" },
                    answer: 0,
                    difficulty: Difficulty.Easy
                ),
                new(
                    id: 4,
                    text: "Quantos continentes existem no mundo?",
                    items: new List<string> { "5", "6", "7", "8" },
                    answer: 1,
                    difficulty: Difficulty.Easy
                ),
                new(
                    id: 5,
                    text: "Qual é o nome do satélite natural da Terra?",
                    items: new List<string> { "Lua", "Sol", "Marte", "Vênus" },
                    answer: 0,
                    difficulty: Difficulty.Easy
                ),
                new(
                    id: 6,
                    text: "Qual é o maior oceano do mundo?",
                    items: new List<string> { "Atlântico", "Índico", "Ártico", "Pacífico" },
                    answer: 3,
                    difficulty: Difficulty.Medium
                ),
                new(
                    id: 7,
                    text: "Em que ano o homem chegou à Lua pela primeira vez?",
                    items: new List<string> { "1959", "1965", "1969", "1973" },
                    answer: 2,
                    difficulty: Difficulty.Medium
                ),
                new(
                    id: 8,
                    text: "Quem pintou a famosa obra 'Mona Lisa'?",
                    items: new List<string>
                    {
                        "Vincent van Gogh",
                        "Pablo Picasso",
                        "Leonardo da Vinci",
                        "Michelangelo"
                    },
                    answer: 2,
                    difficulty: Difficulty.Medium
                ),
                new(
                    id: 9,
                    text: "Qual é o nome do maior deserto do mundo?",
                    items: new List<string>
                    {
                        "Deserto do Saara",
                        "Deserto de Gobi",
                        "Deserto de Kalahari",
                        "Antártida"
                    },
                    answer: 3,
                    difficulty: Difficulty.Medium
                ),
                new(
                    id: 10,
                    text: "Qual é a fórmula química da água?",
                    items: new List<string> { "CO2", "O2", "H2O", "H2O2" },
                    answer: 2,
                    difficulty: Difficulty.Medium
                ),
                new(
                    id: 11,
                    text: "Qual é a cidade brasileira conhecida como 'A Terra da Luz'?",
                    items: new List<string> { "Recife", "Fortaleza", "Salvador", "Rio de Janeiro" },
                    answer: 1,
                    difficulty: Difficulty.Hard
                ),
                new(
                    id: 12,
                    text: "Qual é o nome do famoso teatro de Fortaleza?",
                    items: new List<string>
                    {
                        "Teatro José de Alencar",
                        "Teatro da Paz",
                        "Teatro São João",
                        "Teatro do Centro Cultural"
                    },
                    answer: 0,
                    difficulty: Difficulty.Hard
                ),
                new(
                    id: 13,
                    text: "Em que ano foi fundada a Universidade de Fortaleza (UNIFOR)?",
                    items: new List<string> { "1965", "1973", "1980", "1990" },
                    answer: 1,
                    difficulty: Difficulty.Hard
                ),
                new(
                    id: 14,
                    text: "Qual é o nome do sistema operacional mais utilizado em smartphones?",
                    items: new List<string> { "Windows Phone", "Android", "Mac OS", "Linux" },
                    answer: 1,
                    difficulty: Difficulty.Hard
                ),
                new(
                    id: 15,
                    text: "O que é um 'algoritmo de criptografia'?",
                    items: new List<string>
                    {
                        "Uma técnica para esconder informações, tornando-as ilegíveis para quem não tem a chave correta",
                        "Um sistema de armazenamento de dados em nuvem",
                        "Um método de aumentar a velocidade de uma conexão de internet",
                        "Um programa para melhorar a qualidade de imagens"
                    },
                    answer: 0,
                    difficulty: Difficulty.Hard
                ),
                new(
                    id: 16,
                    text: "Qual é a teoria que descreve a gravitação como uma curvatura do espaço-tempo, proposta por Albert Einstein?",
                    items: new List<string>
                    {
                        "Teoria das Cordas",
                        "Teoria da Relatividade Geral",
                        "Teoria Quântica de Campos",
                        "Teoria da Gravitação Universal"
                    },
                    answer: 1,
                    difficulty: Difficulty.Expert
                ),
                new(
                    id: 17,
                    text: "Qual é o nome da maior cadeia montanhosa do mundo, localizada no oeste da América do Norte?",
                    items: new List<string> { "Andes", "Himalaia", "Alpes", "Rocky Mountains" },
                    answer: 3,
                    difficulty: Difficulty.Expert
                ),
                new(
                    id: 18,
                    text: "Quem é considerado o 'pai da genética moderna' por suas descobertas sobre a hereditariedade?",
                    items: new List<string>
                    {
                        "Charles Darwin",
                        "Gregor Mendel",
                        "Louis Pasteur",
                        "Albert Einstein"
                    },
                    answer: 1,
                    difficulty: Difficulty.Expert
                ),
                new(
                    id: 19,
                    text: "Qual elemento químico é essencial para a produção de hormônios tireoidianos e é frequentemente adicionado ao sal de cozinha?",
                    items: new List<string> { "Fósforo", "Iodo", "Potássio", "Cálcio" },
                    answer: 1,
                    difficulty: Difficulty.Expert
                ),
                new(
                    id: 20,
                    text: "Quem escreveu a famosa Alegoria da Caverna, que discute a percepção da realidade e o conhecimento?",
                    items: new List<string> { "Aristóteles", "Sócrates", "Platão", "Pitagoras" },
                    answer: 2,
                    difficulty: Difficulty.Expert
                )
            }
        );
    }

    /// <summary>
    /// Obtém uma questão pelo ID fornecido.
    /// </summary>
    /// <param name="id">O identificador único da questão a ser obtida.</param>
    /// <returns>A questão correspondente ao ID fornecido, ou <c>null</c> se nenhuma questão for encontrada.</returns>
    public Question GetQuestionById(int id)
    {
        return questions.FirstOrDefault(q => q.Id == id);
    }

    /// <summary>
    /// Marca uma questão como respondida, adicionando-a ao histórico, caso ainda não esteja.
    /// </summary>
    /// <param name="question">A questão a ser marcada como respondida.</param>
    public void MarkAsAnswered(Question question)
    {
        if (!history.Contains(question))
        {
            history.Add(question);
        }
    }

    /// <summary>
    /// Obtém o histórico de questões que já foram respondidas.
    /// </summary>
    /// <returns>Uma lista contendo as questões respondidas.</returns>
    public IList<Question> GetHistory()
    {
        return new List<Question>(history);
    }

    /// <summary>
    /// Retorna as questões que ainda não foram respondidas,
    /// excluindo aquelas presentes no histórico.
    /// </summary>
    /// <returns>Uma lista contendo as questões que ainda não foram respondidas.</returns>
    public IList<Question> GetUnansweredQuestions()
    {
        return questions.Except(history).ToList();
    }

    /// <summary>
    /// Obtém a resposta correspondente a um índice em uma questão.
    /// </summary>
    /// <param name="questionId">O ID da questão.</param>
    /// <param name="answerIndex">O índice da resposta na lista de itens.</param>
    /// <returns>O texto da resposta correspondente ao índice.</returns>
    /// <exception cref="ArgumentException">Lançado se a questão não for encontrada.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Lançado se o índice estiver fora dos limites da lista de itens.</exception>
    public string GetAnswer(int questionId, int answer)
    {
        var question = GetQuestionById(questionId);

        if (question == null)
        {
            throw new ArgumentException(
                $"Nenhuma questão encontrada com o ID {questionId}.",
                nameof(questionId)
            );
        }

        if (answer < 0 || answer >= question.Items.Count)
        {
            throw new ArgumentOutOfRangeException(
                nameof(answer),
                $"Índice {answer} está fora dos limites da lista de itens."
            );
        }

        return question.Items[answer];
    }

    /// <summary>
    /// Seleciona aleatoriamente uma questão que ainda não foi respondida.
    /// </summary>
    /// <returns>
    /// Uma questão não respondida selecionada aleatoriamente.
    /// Retorna <c>null</c> se todas as questões já tiverem sido respondidas.
    /// </returns>
    public Question GetRandomUnansweredQuestion()
    {
        var unansweredQuestions = questions.Except(history).ToList();

        if (unansweredQuestions.Count == 0)
        {
            return null;
        }

        return unansweredQuestions[Random.Range(0, unansweredQuestions.Count)];
    }

    /// <summary>
    /// Remove todas as questões do histórico.
    /// </summary>
    public void ClearHistory()
    {
        history.Clear();
    }

    /// <summary>
    /// Seleciona aleatoriamente uma questão com base na dificuldade especificada,
    /// excluindo as que já foram respondidas.
    /// </summary>
    /// <param name="difficulty">A dificuldade da questão, representada por um valor inteiro.</param>
    /// <returns>
    /// Uma questão aleatória que corresponde à dificuldade especificada e que ainda não foi respondida.
    /// Retorna <c>null</c> se não houver questões correspondentes ou se todas já tiverem sido respondidas.
    /// </returns>
    public Question GetRandomQuestionByDifficulty(int difficulty)
    {
        var filteredQuestions = questions
            .Where(q => q.Difficulty == (Difficulty)difficulty && !history.Contains(q))
            .ToList();

        if (filteredQuestions.Count == 0)
        {
            return null;
        }

        return filteredQuestions[Random.Range(0, filteredQuestions.Count)];
    }

    /// <summary>
    /// Verifica se a resposta escolhida pelo usuário está correta.
    /// </summary>
    /// <param name="questionId">O ID da questão para verificar.</param>
    /// <param name="userAnswer">A resposta escolhida pelo usuário.</param>
    /// <returns>True se a resposta estiver correta, caso contrário, False.</returns>
    public bool IsCorrectAnswer(int questionId, int answer)
    {
        var question = GetQuestionById(questionId);

        if (question == null)
        {
            throw new ArgumentException(
                $"Nenhuma questão encontrada com o ID {questionId}.",
                nameof(questionId)
            );
        }

        if (answer < 0 || answer >= question.Items.Count)
        {
            throw new ArgumentOutOfRangeException(
                nameof(answer),
                $"Índice {answer} está fora dos limites da lista de itens."
            );
        }

        return answer == question.Answer;
    }
}

public class Question
{
    public Question(int id, string text, IList<string> items, int answer, Difficulty difficulty)
    {
        Id = id;
        Text = text;
        Items = items;
        Answer = answer;
        Difficulty = difficulty;
    }

    public int Id { get; private set; }

    public string Text { get; private set; }

    public IList<string> Items { get; private set; }

    public int Answer { get; private set; }

    public Difficulty Difficulty { get; private set; }
}

public enum Difficulty
{
    Easy,
    Medium,
    Hard,
    Expert
}