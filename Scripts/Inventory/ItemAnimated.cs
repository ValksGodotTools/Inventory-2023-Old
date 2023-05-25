namespace Inventory;

public class ItemAnimated : ItemType
{
    public SpriteFrames SpriteFrames { get; set; }

    public override AnimatedSprite2D GenerateGraphic()
    {
        var sprite = new AnimatedSprite2D
        {
            SpriteFrames = SpriteFrames
        };

        sprite.Frame = GD.RandRange(0, sprite.SpriteFrames.GetFrameCount("default") - 1);
        sprite.Play();

        Node2D = sprite;

        return sprite;
    }
}
