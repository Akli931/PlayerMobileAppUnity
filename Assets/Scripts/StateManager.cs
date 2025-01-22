using UnityEngine;
using TMPro; // Nécessaire pour TextMeshPro
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Nécessaire pour les boutons

public class StateManager : MonoBehaviour
{
    [SerializeField] private TMP_Text usernameText; // Champ pour afficher le nom d'utilisateur
    [SerializeField] private TMP_Text friendsCountText; // Champ pour afficher le nombre d'amis
    [SerializeField] private TMP_Text gamesCountText; // Champ pour afficher le nombre de jeux
    [SerializeField] private Button logoutButton; // Bouton de déconnexion

    private void Start()
    {
        // Affiche le nom d'utilisateur stocké dans PlayerPrefs
        string username = PlayerPrefs.GetString("username", "Invité");
        usernameText.text = $"Bienvenue, {username} !";

        // Génère des chiffres aléatoires pour les amis et les jeux
        int randomFriends = Random.Range(0, 101); // Entre 0 et 100 amis
        int randomGames = Random.Range(0, 51); // Entre 0 et 50 jeux

        // Affiche les valeurs aléatoires dans les champs correspondants
        friendsCountText.text = $"Nombre d'amis : {randomFriends}";
        gamesCountText.text = $"Nombre de jeux : {randomGames}";

        // Ajoute un listener au bouton de déconnexion
        logoutButton.onClick.AddListener(OnLogoutButtonPressed);
    }

    private void OnLogoutButtonPressed()
    {
        // Supprime le nom d'utilisateur de PlayerPrefs
        PlayerPrefs.DeleteKey("username");
        PlayerPrefs.Save();

        // Retourne à la scène de connexion
        SceneManager.LoadScene("LoginScene");
    }
}
