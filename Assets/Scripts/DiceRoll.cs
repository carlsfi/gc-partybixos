using System;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    public int diceValue; // Valor do dado
    private bool isRolling = false; // Impede múltiplas rolagens simultâneas
    public Action OnRollComplete; // Evento acionado após a rolagem

    private void Update()
    {
        // Verifica se o dado está girando
        if (isRolling)
        {
            // Gira o dado em todas as direções
            transform.Rotate(UnityEngine.Random.Range(300, 600) * Time.deltaTime,
                             UnityEngine.Random.Range(300, 600) * Time.deltaTime,
                             UnityEngine.Random.Range(300, 600) * Time.deltaTime);
        }
        else
        {
            // Verifica entrada do teclado ou controle apenas se o dado não está girando
            CheckRollInput();
        }
    }

    private void CheckRollInput()
    {
        // Teclado: Espaço
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartDiceRoll();
        }

        // Controle Xbox: Botão A
        if (Input.GetButtonDown("Submit")) // "Submit" é geralmente associado ao botão A no Xbox
        {
            StartDiceRoll();
        }

        // Controle PlayStation: Botão X
        if (Input.GetKeyDown(KeyCode.JoystickButton1)) // Botão 1 é geralmente o X no PlayStation
        {
            StartDiceRoll();
        }
    }

    private void StartDiceRoll()
    {
        if (!isRolling)
        {
            StartCoroutine(RollDice());
        }
    }

    private System.Collections.IEnumerator RollDice()
    {
        isRolling = true; // Bloqueia novas rolagens

        // Define uma duração para o giro (1 segundo, por exemplo)
        yield return new WaitForSeconds(1f);

        isRolling = false; // Permite novas rolagens

        // Gera um valor aleatório para o dado entre 1 e 6
        diceValue = UnityEngine.Random.Range(1, 7);
        Debug.Log("Valor do dado: " + diceValue);

        // Ajusta a rotação do dado para o valor sorteado
        SetDiceRotation(diceValue);

        // Dispara o evento de rolagem completa
        OnRollComplete?.Invoke();
    }

    private void SetDiceRotation(int value)
    {
        // Rotações definidas para cada face
        switch (value)
        {
            case 1:
                transform.rotation = Quaternion.Euler(-90, 0, 180);
                break;
            case 2:
                transform.rotation = Quaternion.Euler(180, 0, 180);
                break;
            case 3:
                transform.rotation = Quaternion.Euler(180, 0, 90);
                break;
            case 4:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case 5:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case 6:
                transform.rotation = Quaternion.Euler(90, 0, 180);
                break;
        }
    }

    public void Roll()
    {
        StartDiceRoll(); // Chama a rolagem
    }
}
