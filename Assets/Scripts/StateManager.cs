using UnityEngine;
using TMPro; // N�cessaire pour TextMeshPro
using UnityEngine.SceneManagement;
using UnityEngine.UI; // N�cessaire pour les boutons

public class StateManager : MonoBehaviour
{
    [SerializeField] private TMP_Text usernameText; // Champ pour afficher le nom d'utilisateur
    [SerializeField] private TMP_Text friendsCountText; // Champ pour afficher le nombre d'amis
    [SerializeField] private TMP_Text gamesCountText; // Champ pour afficher le nombre de jeux
    [SerializeField] private Button logoutButton; // Bouton de d�connexion

    private void Start()
    {
        // Affiche le nom d'utilisateur stock� dans PlayerPrefs
        string username = PlayerPrefs.GetString("username", "Invit�");
        usernameText.text = $"Bienvenue, {username} !";

        // G�n�re des chiffres al�atoires pour les amis et les jeux
        int randomFriends = Random.Range(0, 101); // Entre 0 et 100 amis
        int randomGames = Random.Range(0, 51); // Entre 0 et 50 jeux

        // Affiche les valeurs al�atoires dans les champs correspondants
        friendsCountText.text = $"Nombre d'amis : {randomFriends}";
        gamesCountText.text = $"Nombre de jeux : {randomGames}";

        // Ajoute un listener au bouton de d�connexion
        logoutButton.onClick.AddListener(OnLogoutButtonPressed);
    }

    private void OnLogoutButtonPressed()
    {
        // Supprime le nom d'utilisateur de PlayerPrefs
        PlayerPrefs.DeleteKey("username");
        PlayerPrefs.Save();

        // Retourne � la sc�ne de connexion
        SceneManager.LoadScene("LoginScene");
    }
}
