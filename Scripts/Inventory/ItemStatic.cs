namespace Inventory;

public class ItemStatic : ItemType
{
    public Texture2D Texture { get; set; }

    public override Sprite2D GenerateGraphic()
    {
        var sprite = new Sprite2D { Texture = Texture };
        Node2D = sprite;
        return sprite;
    }
}