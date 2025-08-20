using UnityEngine;

public interface ICard
{
    int id { get; set; }
    bool IsMatched { get; set; }
    bool IsRevealed { get; }
    void ShowFrontSprite();
    void HideFront();
    void UpdateSpriteSize(Vector2 cellSize);
}
