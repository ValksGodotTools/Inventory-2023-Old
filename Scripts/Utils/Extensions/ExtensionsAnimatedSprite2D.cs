namespace Inventory;

public static class ExtensionsAnimatedSprite2D
{
	/// <summary>
	/// Instantly switch to the next animation without waiting for the current animation to finish
	/// </summary>
	public static void InstantPlay(this AnimatedSprite2D animatedSprite2D, string animation)
	{
		animatedSprite2D.Animation = animation;
		animatedSprite2D.Play(animation);
	}
}
