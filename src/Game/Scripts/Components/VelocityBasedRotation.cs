using Godot;

namespace LabAutobattler.Components;

public partial class VelocityBasedRotation : Node
{
    [Export]
    private bool enabled = true;

    [Export]
    public Node2D Target { get; private set; } = null!;

    [Export(PropertyHint.Range, "0.25,1.5")]
    private float lerpSeconds = 0.4f;

    [Export]
    private float rotationMultiplier = 120;

    [Export]
    private float xVelocityThreshold = 3;

    private Vector2 _lastPosition;
    private Vector2 _velocity;
    private float _angle;
    private float _progress;
    private float _timeElapsed;

    public bool Enabled
    {
        get => enabled;
        set
        {
            enabled = value;
            if (enabled == false)
            {
                Target.Rotation = 0;
            }
        }
    }

    public override void _Process(double delta)
    {
        if (Enabled == false)
            return;

        _velocity = Target.GlobalPosition - _lastPosition;
        _lastPosition = Target.GlobalPosition;
        _progress = _timeElapsed / lerpSeconds;

        if (Mathf.Abs(_velocity.X) > xVelocityThreshold)
        {
            _angle = (float)(_velocity.Normalized().X * rotationMultiplier * delta);
        }
        else
        {
            _angle = 0;
        }

        Target.Rotation = Mathf.LerpAngle(Target.Rotation, _angle, _progress);
        _timeElapsed += (float)delta;

        if (_progress > 1)
        {
            _timeElapsed = 0;
        }
    }
}