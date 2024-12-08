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
                    text: "Qual   a capital do Brasil?",
                    items: new List<string>
                    {
                        "S o Paulo",
                        "Rio de Janeiro",
                        "Bras lia",
                        "Salvador"
                    },
                    answer: 2,
                    difficulty: Difficulty.Easy
                ),
                new(
                    id: 2,
                    text: "Qual   o maior planeta do nosso sistema solar?",
                    items: new List<string> { "Terra", "J piter", "Marte", "V nus" },
                    answer: 1,
                    difficulty: Difficulty.Easy
                ),
                new(
                    id: 3,
                    text: "Qual   o maior animal terrestre?",
                    items: new List<string> { "Elefante", "Girafa", "Le o", "Baleia" },
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
                    text: "Qual   o nome do sat lite natural da Terra?",
                    items: new List<string> { "Lua", "Sol", "Marte", "V nus" },
                    answer: 0,
                    difficulty: Difficulty.Easy
                ),
                new(
                    id: 6,
                    text: "Qual   o maior oceano do mundo?",
                    items: new List<string> { "Atl ntico", " ndico", " rtico", "Pac fico" },
                    answer: 3,
                    difficulty: Difficulty.Medium
                ),
                new(
                    id: 7,
                    text: "Em que ano o homem chegou   Lua pela primeira vez?",
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
                    text: "Qual   o nome do maior deserto do mundo?",
                    items: new List<string>
                    {
                        "Deserto do Saara",
                        "Deserto de Gobi",
                        "Deserto de Kalahari",
                        "Ant rtida"
                    },
                    answer: 3,
                    difficulty: Difficulty.Medium
                ),
                new(
                    id: 10,
                    text: "Qual   a f rmula qu mica da  gua?",
                    items: new List<string> { "CO2", "O2", "H2O", "H2O2" },
                    answer: 2,
                    difficulty: Difficulty.Medium
                ),
                new(
                    id: 11,
                    text: "Qual   a cidade brasileira conhecida como 'A Terra da Luz'?",
                    items: new List<string> { "Recife", "Fortaleza", "Salvador", "Rio de Janeiro" },
                    answer: 1,
                    difficulty: Difficulty.Hard
                ),
                new(
                    id: 12,
                    text: "Qual   o nome do famoso teatro de Fortaleza, inaugurado em 1910, e um dos principais marcos culturais da cidade?",
                    items: new List<string>
                    {
                        "Teatro Jos  de Alencar",
                        "Teatro da Paz",
                        "Teatro S o Jo o",
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
                    text: "Qual   o nome do sistema operacional mais utilizado em smartphones?",
                    items: new List<string> { "Windows Phone", "Android", "Mac OS", "Linux" },
                    answer: 1,
                    difficulty: Difficulty.Hard
                ),
                new(
                    id: 15,
                    text: "O que   um 'algoritmo de criptografia'?",
                    items: new List<string>
                    {
                        "Uma t cnica para esconder informa  es, tornando-as ileg veis para quem n o tem a chave correta",
                        "Um sistema de armazenamento de dados em nuvem",
                        "Um m todo de aumentar a velocidade de uma conex o de internet",
                        "Um programa para melhorar a qualidade de imagens"
                    },
                    answer: 0,
                    difficulty: Difficulty.Hard
                ),
                new(
                    id: 16,
                    text: "Qual   a teoria que descreve a gravita  o como uma curvatura do espa o-tempo, proposta por Albert Einstein?",
                    items: new List<string>
                    {
                        "Teoria das Cordas",
                        "Teoria da Relatividade Geral",
                        "Teoria Qu ntica de Campos",
                        "Teoria da Gravita  o Universal"
                    },
                    answer: 1,
                    difficulty: Difficulty.Expert
                ),
                new(
                    id: 17,
                    text: "Qual   o nome da maior cadeia montanhosa do mundo, localizada no oeste da Am rica do Norte?",
                    items: new List<string> { "Andes", "Himalaia", "Alpes", "Rocky Mountains" },
                    answer: 3,
                    difficulty: Difficulty.Expert
                ),
                new(
                    id: 18,
                    text: "Quem   considerado o 'pai da gen tica moderna' por suas descobertas sobre a hereditariedade?",
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
                    text: "Qual elemento qu mico   essencial para a produ  o de horm nios tireoidianos e   frequentemente adicionado ao sal de cozinha?",
                    items: new List<string> { "F sforo", "Iodo", "Pot ssio", "C lcio" },
                    answer: 1,
                    difficulty: Difficulty.Expert
                ),
                new(
                    id: 20,
                    text: "Quem escreveu a famosa Alegoria da Caverna, que discute a percep  o da realidade e o conhecimento?",
                    items: new List<string> { "Arist teles", "S crates", "Plat o", "Pitagoras" },
                    answer: 2,
                    difficulty: Difficulty.Expert
                )
            }
        );
    }

    /// <summary>
    /// Obt m uma quest o pelo ID fornecido.
    /// </summary>
    /// <param name="id">O identificador  nico da quest o a ser obtida.</param>
    /// <returns>A quest o correspondente ao ID fornecido, ou <c>null</c> se nenhuma quest o for encontrada.</returns>
    public Question GetQuestionById(int id)
    {
        return questions.FirstOrDefault(q => q.Id == id);
    }

    /// <summary>
    /// Marca uma quest o como respondida, adicionando-a ao hist rico, caso ainda n o esteja.
    /// </summary>
    /// <param name="question">A quest o a ser marcada como respondida.</param>
    public void MarkAsAnswered(Question question)
    {
        if (!history.Contains(question))
        {
            history.Add(question);
        }
    }

    /// <summary>
    /// Obt m o hist rico de quest es que j  foram respondidas.
    /// </summary>
    /// <returns>Uma lista contendo as quest es respondidas.</returns>
    public IList<Question> GetHistory()
    {
        return new List<Question>(history);
    }

    /// <summary>
    /// Retorna as quest es que ainda n o foram respondidas,
    /// excluindo aquelas presentes no hist rico.
    /// </summary>
    /// <returns>Uma lista contendo as quest es que ainda n o foram respondidas.</returns>
    public IList<Question> GetUnansweredQuestions()
    {
        return questions.Except(history).ToList();
    }

    /// <summary>
    /// Obt m a resposta correspondente a um  ndice em uma quest o.
    /// </summary>
    /// <param name="questionId">O ID da quest o.</param>
    /// <param name="answerIndex">O  ndice da resposta na lista de itens.</param>
    /// <returns>O texto da resposta correspondente ao  ndice.</returns>
    /// <exception cref="ArgumentException">Lan ado se a quest o n o for encontrada.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Lan ado se o  ndice estiver fora dos limites da lista de itens.</exception>
    public string GetAnswer(int questionId, int answer)
    {
        var question = GetQuestionById(questionId);

        if (question == null)
        {
            throw new ArgumentException(
                $"Nenhuma quest o encontrada com o ID {questionId}.",
                nameof(questionId)
            );
        }

        if (answer < 0 || answer >= question.Items.Count)
        {
            throw new ArgumentOutOfRangeException(
                nameof(answer),
                $" ndice {answer} est  fora dos limites da lista de itens."
            );
        }

        return question.Items[answer];
    }

    /// <summary>
    /// Seleciona aleatoriamente uma quest o que ainda n o foi respondida.
    /// </summary>
    /// <returns>
    /// Uma quest o n o respondida selecionada aleatoriamente.
    /// Retorna <c>null</c> se todas as quest es j  tiverem sido respondidas.
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
    /// Remove todas as quest es do hist rico.
    /// </summary>
    public void ClearHistory()
    {
        history.Clear();
    }

    /// <summary>
    /// Seleciona aleatoriamente uma quest o com base na dificuldade especificada,
    /// excluindo as que j  foram respondidas.
    /// </summary>
    /// <param name="difficulty">A dificuldade da quest o, representada por um valor inteiro.</param>
    /// <returns>
    /// Uma quest o aleat ria que corresponde   dificuldade especificada e que ainda n o foi respondida.
    /// Retorna <c>null</c> se n o houver quest es correspondentes ou se todas j  tiverem sido respondidas.
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
    /// Verifica se a resposta escolhida pelo usu rio est  correta.
    /// </summary>
    /// <param name="questionId">O ID da quest o para verificar.</param>
    /// <param name="userAnswer">A resposta escolhida pelo usu rio.</param>
    /// <returns>True se a resposta estiver correta, caso contr rio, False.</returns>
    public bool IsCorrectAnswer(int questionId, int answer)
    {
        var question = GetQuestionById(questionId);

        if (question == null)
        {
            throw new ArgumentException(
                $"Nenhuma quest o encontrada com o ID {questionId}.",
                nameof(questionId)
            );
        }

        if (answer < 0 || answer >= question.Items.Count)
        {
            throw new ArgumentOutOfRangeException(
                nameof(answer),
                $" ndice {answer} est  fora dos limites da lista de itens."
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
