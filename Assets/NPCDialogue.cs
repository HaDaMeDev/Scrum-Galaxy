using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Añadido para cambiar de escena

public class NPCDialogue : MonoBehaviour
{
    public string dialogueText = "¡Hola, soy un NPC! Entrando a mi mundo...";
    public TextMeshProUGUI dialogueUI; // Asigna este campo en el Inspector
    public string newSceneName = "Mundo1"; // Cambiar en el Inspector para cada NPC

    void Start()
    {
        if (dialogueUI != null)
        {
            dialogueUI.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowDialogue();
            Invoke("LoadNewWorld", 2f); // Muestra el diálogo 2 segundos y luego carga la escena
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideDialogue();
        }
    }

    void ShowDialogue()
    {
        if (dialogueUI != null)
        {
            dialogueUI.text = dialogueText;
            dialogueUI.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("DialogueUI no está asignado en " + gameObject.name);
        }
    }

    void HideDialogue()
    {
        if (dialogueUI != null)
        {
            dialogueUI.gameObject.SetActive(false);
        }
    }

    void LoadNewWorld()
    {
        SceneManager.LoadScene(newSceneName); // Carga la nueva escena
    }
}