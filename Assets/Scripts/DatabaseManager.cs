using UnityEngine;
using UnityEngine.UI; // Nécessaire pour gérer les éléments UI
using TMPro; // Nécessaire pour les champs de saisie TMP
using UnityEngine.Networking; // Nécessaire pour les requêtes HTTP
using Newtonsoft.Json.Linq; // Utilisé pour analyser les données JSON
using System.Collections;

public class DatabaseManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInput; // Champ pour le nom d'utilisateur
    [SerializeField] private TMP_InputField passwordInput; // Champ pour le mot de passe
    private string apiUrl = "http://localhost:3000/users"; // URL de l'API

    // Méthode appelée par le bouton Connexion
    public void OnLoginButtonPressed()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.Log("Veuillez remplir tous les champs !");
            return;
        }

        StartCoroutine(LoginUser(username, password));
    }

    // Méthode appelée par le bouton Inscription
    public void OnRegisterButtonPressed()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.Log("Veuillez remplir tous les champs !");
            return;
        }

        StartCoroutine(RegisterUser(username, password));
    }

    // Méthode pour enregistrer un utilisateur via l'API
    private IEnumerator RegisterUser(string username, string password)
    {
        string jsonData = $"{{\"username\": \"{username}\", \"password\": \"{password}\"}}";
        byte[] body = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Inscription réussie pour l'utilisateur {username} !");
        }
        else
        {
            Debug.Log($"Erreur lors de l'inscription : {request.error}");
        }
    }

    // Méthode pour vérifier un utilisateur via l'API
    private IEnumerator LoginUser(string username, string password)
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            JArray users = JArray.Parse(response); // Utilisation de Newtonsoft.Json pour analyser la réponse JSON

            bool isUserValid = false;

            foreach (JObject user in users)
            {
                if (user["username"]?.ToString() == username && user["password"]?.ToString() == password)
                {
                    isUserValid = true;
                    break;
                }
            }

            if (isUserValid)
            {
                PlayerPrefs.SetString("username", username);
                PlayerPrefs.Save();
                Debug.Log($"Connexion réussie pour l'utilisateur {username} !");
                UnityEngine.SceneManagement.SceneManager.LoadScene("StateScene");
            }
            else
            {
                Debug.Log("Échec de la connexion : Identifiants incorrects.");
            }
        }
        else
        {
            Debug.Log($"Erreur lors de la connexion : {request.error}");
        }
    }
}
