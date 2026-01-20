using Godot;
using System;

public partial class ErrorMessage : Control
{
    public Label ErrorMessageTextLabel => GetChild<Label>(1);


    private Timer _messageTimeout;
    private float duration = 3.0f;

    public override void _Ready()
    {
        _messageTimeout = GetChild<Timer>(2);
        _messageTimeout.Timeout += Hide;
    }


    /// <summary>
    /// Displays an error message for a set duration.
    /// </summary>
    /// <param name="message"></param>
    public void DisplayMessage(string message)
    {
        ErrorMessageTextLabel.Text = message;
        Show();
        _messageTimeout.Start(duration);
    }
}
