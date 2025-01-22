using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json.Linq;

public class CommentsManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField commentInput; // Champ pour écrire un commentaire
    [SerializeField] private Button postButton; // Bouton pour poster le commentaire
    [SerializeField] private Transform commentsContainer; // Conteneur pour afficher les commentaires
    [SerializeField] private GameObject commentPrefab; // Prefab pour un commentaire
    private string apiUrl = "http://localhost:3000/comments";

    private void Start()
    {
        // Charger les commentaires existants
        StartCoroutine(LoadComments());

        // Ajouter un listener au bouton pour poster un commentaire
        postButton.onClick.AddListener(OnPostButtonPressed);
    }

    private void OnPostButtonPressed()
    {
        string username = PlayerPrefs.GetString("username", "Invité");
        string content = commentInput.text;

        if (string.IsNullOrEmpty(content))
        {
            Debug.Log("Le commentaire est vide !");
            return;
        }

        StartCoroutine(PostComment(username, content));
    }

    private IEnumerator LoadComments()
    {
        ClearComments(); // Supprime les anciens commentaires

        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            JArray comments = JArray.Parse(response);

            foreach (JObject comment in comments)
            {
                string username = comment["username"]?.ToString();
                string content = comment["content"]?.ToString();

                // Afficher chaque commentaire
                AddCommentToUI(username, content);
            }
        }
        else
        {
            Debug.LogError($"Erreur lors du chargement des commentaires : {request.error}");
        }
    }

    private IEnumerator PostComment(string username, string content)
    {
        string jsonData = $"{{\"username\":\"{username}\",\"content\":\"{content}\"}}";
        byte[] body = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Commentaire ajouté avec succès !");
            AddCommentToUI(username, content); // Ajouter le commentaire dans l'UI
            commentInput.text = ""; // Réinitialiser le champ de saisie
        }
        else
        {
            Debug.LogError($"Erreur lors de l'ajout du commentaire : {request.error}");
        }
    }

    private void AddCommentToUI(string username, string content)
    {
        GameObject commentObject = Instantiate(commentPrefab, commentsContainer);
        TMP_Text commentText = commentObject.GetComponentInChildren<TMP_Text>();

        if (commentText != null)
        {
            commentText.text = $"{username}: {content}";
        }
    }

    private void ClearComments()
    {
        foreach (Transform child in commentsContainer)
        {
            Destroy(child.gameObject); // Supprime tous les enfants du conteneur
        }
    }
}
