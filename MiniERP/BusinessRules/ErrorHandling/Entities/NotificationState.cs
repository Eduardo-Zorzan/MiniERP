public class NotificationState
{
    public required List<State> Running { get; set; }
}

public class State
{
    public required NotificationType Type { get; set; }
    public required string Message { get; set; }
}

public enum NotificationType
{
    Info,
    Warning,
    Error,
    Success
}