using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public RectTransform playerHealthFillRect;

    public void UpdatePlayerHealth(int currentHealth, int maxHealth)
    {
        if (playerHealthFillRect != null)
        {
            float healthPercent = (float)currentHealth / maxHealth;

            if (healthPercent < 0)
            {
                healthPercent = 0;
            }

            playerHealthFillRect.localScale = new Vector3(healthPercent, 1, 1);
        }
    }
}