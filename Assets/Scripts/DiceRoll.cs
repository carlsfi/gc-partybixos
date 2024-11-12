using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    public int diceValue; // Valor do dado (1-6)
    public float rotationSpeed = 1000f; // Velocidade de rotação do dado
    private bool isRolling = false; // Verifica se o dado está rolando

    private void Update()
    {
        if (isRolling)
        {
            // Gira o dado em todas as direções
            transform.Rotate(Random.Range(300, 600) * Time.deltaTime, Random.Range(300, 600) * Time.deltaTime, Random.Range(300, 600) * Time.deltaTime);
        }
    }

    private void OnMouseDown()
    {
        if (!isRolling)
        {
            // Inicia o giro do dado
            StartCoroutine(RollDice());
        }
    }

    private System.Collections.IEnumerator RollDice()
    {
        isRolling = true;

        // Define uma duração para o giro (1 segundo, por exemplo)
        yield return new WaitForSeconds(1f);

        isRolling = false;

        // Gera um valor aleatório para o dado entre 1 e 6
        diceValue = Random.Range(1, 7);
        Debug.Log("Valor do dado: " + diceValue);

        // Ajusta a rotação do dado para o valor sorteado
        SetDiceRotation(diceValue);
    }

private void SetDiceRotation(int value)
{
    switch (value)
    {
        case 1:
            transform.rotation = Quaternion.Euler(-90, 0, 180); // Face 1 para cima
            break;
        case 2:
            transform.rotation = Quaternion.Euler(180, 0, 180); // Face 2 para cima 
            break;
        case 3:
            transform.rotation = Quaternion.Euler(180, 0, 90); // Face 3 para cima
            break;
        case 4:
            transform.rotation = Quaternion.Euler(0, 0, 90); // Face 4 para cima
            break;
        case 5:
            transform.rotation = Quaternion.Euler(0, 0, 180); // Face 5 para cima
            break;
        case 6:
            transform.rotation = Quaternion.Euler(90, 0, 180); // Face 6 para cima
            break;
    }
}



}
