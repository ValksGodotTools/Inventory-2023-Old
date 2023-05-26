global using Godot;
global using GodotUtils;
global using System;
global using System.Collections.Generic;
global using System.Collections.Concurrent;
global using System.Diagnostics;
global using System.Linq;
global using System.Runtime.CompilerServices;
global using System.Threading;
global using System.Text.RegularExpressions;
global using System.Threading.Tasks;

namespace Inventory;

public partial class Global : Node
{
    public static Window Root { get; private set; }

    private static Global instance;

	public override void _Ready()
	{
        Root = GetTree().Root;
        instance = this;
    }

	public override void _PhysicsProcess(double delta)
	{
		Logger.Update();
	}

    public override void _Notification(int what)
	{
		if (what == NotificationWMCloseRequest)
		{
			GetTree().AutoAcceptQuit = false;
			Quit();
		}
	}

	public static void Quit()
	{
        // Handle cleanup here

        instance.GetTree().Quit();
	}
}
