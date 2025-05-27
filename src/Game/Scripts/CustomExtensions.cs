using System;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using GTweens.Builders;
using GTweensGodot.Extensions;

namespace LabAutobattler;

public static class CustomExtensions
{
    public static async void Fire(this Task task, Action? onComplete = null, Action<Exception>? onError = null)
    {
        try
        {
            try
            {
                await task;
            }
            catch (Exception e)
            {
                GD.Print("something wrong during fire & forget: ");
                GD.Print(e);
                onError?.Invoke(e);
            }

            onComplete?.Invoke();
        }
        catch (Exception e)
        {
            GD.Print("something wrong on fire & forget complete : ");
            GD.Print(e);
            onError?.Invoke(e);
        }
    }

    public static void SetModulateAlpha(this CanvasItem canvasItem, float alpha)
    {
        canvasItem.Modulate = canvasItem.Modulate with { A = alpha };
    }

    public static async Task ShakeAsync(this Node2D node2D, float strength, float duration = 0.2f)
    {
        var originalPosition = node2D.Position;
        const int shakeCount = 10;

        for (var i = 0; i < shakeCount; i++)
        {
            var offset = GetRandomOffset();
            var targetPosition = originalPosition + offset * strength;
            if (i % 2 == 0)
            {
                targetPosition = originalPosition;
            }

            await node2D.TweenPosition(targetPosition, duration / shakeCount).PlayAsync(CancellationToken.None);
            strength *= 0.75f;
        }

        node2D.Position = originalPosition;

        return;

        Vector2 GetRandomOffset()
        {
            return new Vector2(GD.Randf() * 2 - 1, GD.Randf() * 2 - 1);
        }
    }
}

public static class TaskUtil
{
    public static Task DelayGd(float timeSec)
    {
        return GTweenSequenceBuilder.New().AppendTime(timeSec).Build().PlayAsync(CancellationToken.None);
    }
}