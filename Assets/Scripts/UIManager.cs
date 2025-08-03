using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI de Salud del Jugador")]
    public RectTransform playerHealthFillRect;

    [Header("Pantallas de Fin de Juego")]
    public GameObject winScreen;
    public GameObject loseScreen;

    [Header("Colores de la Barra de Vida")]
    public Color healthColor = Color.green;
    public Color shieldColor = Color.yellow;
    public void UpdatePlayerHealth(int currentHealth, int maxHealth, int currentShieldHits)
    {
        if (playerHealthFillRect == null) return;

        Image fillImage = playerHealthFillRect.GetComponent<Image>();
        if (fillImage == null) return;

        if (currentShieldHits > 0)
        {
            fillImage.color = shieldColor;
        }
        else
        {
            fillImage.color = healthColor;
        }

        float healthPercent = (float)currentHealth / maxHealth;
        if (healthPercent < 0) healthPercent = 0;
        playerHealthFillRect.localScale = new Vector3(healthPercent, 1, 1);
    }

    public void ShowWinScreen()
    {
        if (winScreen != null) winScreen.SetActive(true);
    }

    public void ShowLoseScreen()
    {
        if (loseScreen != null) loseScreen.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}