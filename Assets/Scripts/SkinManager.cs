using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public Sprite[] skins; // Массив, хранящий все доступные спрайты для скинов

    public void ApplyRandomSkin()
    {
        int randomSkinIndex = Random.Range(0, skins.Length); // Выбираем случайный индекс из массива спрайтов
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && randomSkinIndex < skins.Length)
        {
            spriteRenderer.sprite = skins[randomSkinIndex]; // Применяем выбранный случайный спрайт к объекту
        }
    }
}
