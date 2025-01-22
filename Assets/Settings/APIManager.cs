using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    private string apiUrl = "http://localhost:3000/users";

    // Méthode pour vérifier les identifiants (connexion)
    public void LoginUser(string username, string password, System.Action<bool> callback)
    {
        StartCoroutine(LoginUserCoroutine(username, password, callback));
    }

    private IEnumerator LoginUserCoroutine(string username, string password, System.Action<bool> callback)
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            bool isValidUser = ValidateUser(json, username, password);
            callback(isValidUser);
        }
        else
        {
            Debug.LogError("Erreur lors de la connexion à l'API : " + request.error);
            callback(false);
        }
    }

    private bool ValidateUser(string json, string username, string password)
    {
        // Recherche basique dans le JSON des utilisateurs
        return json.Contains($"\"username\":\"{username}\"") && json.Contains($"\"password\":\"{password}\"");
    }

    // Méthode pour inscrire un nouvel utilisateur
    public void RegisterUser(string username, string password, System.Action<bool> callback)
    {
        StartCoroutine(RegisterUserCoroutine(username, password, callback));
    }

    private IEnumerator RegisterUserCoroutine(string username, string password, System.Action<bool> callback)
    {
        string jsonData = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";
        byte[] body = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Utilisateur inscrit avec succès !");
            callback(true);
        }
        else
        {
            Debug.LogError("Erreur lors de l'inscription : " + request.error);
            callback(false);
        }
    }
}
