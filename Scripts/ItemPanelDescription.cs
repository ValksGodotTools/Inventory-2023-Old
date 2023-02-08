namespace Inventory;

public partial class ItemPanelDescription : Control
{
	private static Control ItemPanelDescriptionParent { get; set; }

	public static void ToggleVisiblity(bool v) => ItemPanelDescriptionParent.Visible = v;

	public static void Clear()
	{
		ItemPanelDescriptionParent.QueueFreeChildren();
		ItemPanelDescriptionParent.SetPhysicsProcess(false);
	}

	public static void Display(Item item)
	{
		ItemPanelDescriptionParent.SetPhysicsProcess(true);
		var panelContainer = new PanelContainer
		{
			ZIndex = 1
		};

		var marginContainer = new MarginContainer();
		marginContainer.AddMargin(5);

		panelContainer.AddChild(marginContainer);

		var vbox = new VBoxContainer();
		marginContainer.AddChild(vbox);

		var labelName = new Label
		{
			Text = item.Type.Name,
			HorizontalAlignment = HorizontalAlignment.Center
		};

		var labelDescription = new Label
		{
			Text = item.Type.Description,
			HorizontalAlignment = HorizontalAlignment.Center
		};

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
