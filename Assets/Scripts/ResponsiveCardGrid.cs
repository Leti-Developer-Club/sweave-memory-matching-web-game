// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using UnityEngine.UI;

// public class ResponsiveCardGrid : MonoBehaviour
// {
//     [Header("Grid Settings")]
//     public GridLayoutGroup gridLayout;
//     public RectTransform gridContainer;
//     public GameObject cardPrefab;

//     [Header("Responsive Settings")]
//     public float minCardSize = 80f;
//     public float maxCardSize = 150f;
//     public float aspectRatio = 1f; // Width/Height ratio for cards

//     public List<Sprite> frontSprites = new List<Sprite>();
//     private int rows,
//         cols;

//     public GameSettingsScriptableObject gameSettings;

//     void Start()
//     {
//         if (gridLayout == null)
//             gridLayout = GetComponent<GridLayoutGroup>();

//         if (gridContainer == null)
//             gridContainer = GetComponent<RectTransform>();
//     }

//     public void CreateGrid(List<Sprite> cardSprites)
//     {
//         rows = gameSettings.rows;
//         cols = gameSettings.cols;

//         // Clear existing cards
//         ClearGrid();

//         // Create shuffled card pairs
//         int[] cardIds = CreateShuffledPairs(rows * cols);

//         // Calculate optimal card size
//         CalculateCardSize();

//         // Create cards
//         for (int i = 0; i < cardIds.Length; i++)
//         {
//             GameObject cardObj = Instantiate(cardPrefab, gridContainer);
//             Sprite card = cardObj.GetComponent<Sprite>();

//             int spriteIndex = cardIds[i] % cardSprites.Count;
//             card.Initialize(cardSprites[spriteIndex], cardIds[i]);
//             cards.Add(card);
//         }
//     }

//     private void CalculateCardSize()
//     {
//         // Get available space
//         Vector2 containerSize = gridContainer.rect.size;

//         // Account for grid spacing
//         float totalSpacingX = gridLayout.spacing.x * (cols - 1);
//         float totalSpacingY = gridLayout.spacing.y * (rows - 1);

//         // Account for padding
//         float paddingX = gridLayout.padding.left + gridLayout.padding.right;
//         float paddingY = gridLayout.padding.top + gridLayout.padding.bottom;

//         // Calculate available space for cards
//         float availableWidth = containerSize.x - totalSpacingX - paddingX;
//         float availableHeight = containerSize.y - totalSpacingY - paddingY;

//         // Calculate card size based on constraints
//         float cardWidth = availableWidth / cols;
//         float cardHeight = availableHeight / rows;

//         // Maintain aspect ratio
//         float targetSize = Mathf.Min(cardWidth, cardHeight / aspectRatio);

//         // Clamp to min/max sizes
//         targetSize = Mathf.Clamp(targetSize, minCardSize, maxCardSize);

//         // Set cell size
//         gridLayout.cellSize = new Vector2(targetSize, targetSize * aspectRatio);

//         Debug.Log($"Card size calculated: {gridLayout.cellSize} for {rows}x{cols} grid");
//     }

//     private int[] CreateShuffledPairs(int totalCards)
//     {
//         int pairCount = totalCards / 2;
//         List<int> cardIds = new List<int>();

//         // Create pairs
//         for (int i = 0; i < pairCount; i++)
//         {
//             cardIds.Add(i);
//             cardIds.Add(i);
//         }

//         // Shuffle using Fisher-Yates
//         for (int i = cardIds.Count - 1; i > 0; i--)
//         {
//             int randomIndex = Random.Range(0, i + 1);
//             int temp = cardIds[i];
//             cardIds[i] = cardIds[randomIndex];
//             cardIds[randomIndex] = temp;
//         }

//         return cardIds.ToArray();
//     }

//     private void ClearGrid()
//     {
//         foreach (Sprite card in cards)
//         {
//             if (card != null)
//                 DestroyImmediate(card.gameObject);
//         }
//         cards.Clear();
//     }

//     // Call this when screen size changes
//     public void RefreshLayout()
//     {
//         if (rows > 0 && cols > 0)
//         {
//             CalculateCardSize();
//         }
//     }

//     void Update()
//     {
//         // Simple screen size change detection
//         if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
//         {
//             RefreshLayout();
//             lastScreenWidth = Screen.width;
//             lastScreenHeight = Screen.height;
//         }
//     }

//     private int lastScreenWidth,
//         lastScreenHeight;
// }
