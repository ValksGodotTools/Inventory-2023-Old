namespace Inventory;

public partial class ItemPanelDescription : Control
{
	private static Control ItemPanelDescriptionParent { get; set; }

	public static void ToggleVisiblity(bool v) => ItemPanelDescriptionParent.Visible = v;

	public static void Clear()
	{
		foreach (Node child in ItemPanelDescriptionParent.GetChildren())
			child.QueueFree();

		ItemPanelDescriptionParent.SetPhysicsProcess(false);
	}

	public static void Display(Item item)
	{
		ItemPanelDescriptionParent.SetPhysicsProcess(true);
		var panelContainer = new PanelContainer();
		panelContainer.ZIndex = 1;

		var marginContainer = new MarginContainer();
		marginContainer.AddMargin(5);

		panelContainer.AddChild(marginContainer);

		var vbox = new VBoxContainer();
		marginContainer.AddChild(vbox);

		var labelName = new Label();
		labelName.Text = item.Type.Name;
		labelName.HorizontalAlignment = HorizontalAlignment.Center;

		var labelDescription = new Label();
		labelDescription.Text = item.Type.Description;
		labelDescription.HorizontalAlignment = HorizontalAlignment.Center;

		vbox.AddChild(labelName);
		vbox.AddChild(labelDescription);

		ItemPanelDescriptionParent.AddChild(panelContainer);
	}

	public override void _Ready()
	{
		SetPhysicsProcess(false);
		ItemPanelDescriptionParent = this;
	}

	public override void _PhysicsProcess(double delta)
	{
		var offset = new Vector2(20, 0);

		Position = GetViewport().GetMousePosition() + offset;
	}
}
