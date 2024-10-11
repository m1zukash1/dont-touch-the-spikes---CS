using Godot;
using System;

public partial class DebugController : Control
{
    [Export]
    private bool ShowDebugInfo = false;

    [ExportGroup("Node References")]
    [Export]
    private Bird Bird;
    
    [Export]
    private Label DebugLabel;

    [Export]
    private int PrintsPerSecond = 32;

    private float timeElapsed = 0f;
    private float printInterval;

    public override void _Ready()
    {
        if(!ShowDebugInfo)
        {
            return;
        }

        if (PrintsPerSecond > 0)
        {
            printInterval = 1f / PrintsPerSecond;
        }
        Show();
    }

    public override void _PhysicsProcess(double delta)
    {
        if(!ShowDebugInfo)
        {
            return;
        }
        if (PrintsPerSecond == 0)
        {
            DebugLabel.Text = Bird.ToString();
            return;
        }

        timeElapsed += (float)delta;

        if (timeElapsed >= printInterval)
        {
            DebugLabel.Text = Bird.ToString();
            timeElapsed = 0f; 
        }
    }
}
