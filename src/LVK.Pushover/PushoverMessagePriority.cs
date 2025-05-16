namespace LVK.Pushover;

/// <summary>
/// Represents the different priority levels for a Pushover message.
/// </summary>
public enum PushoverMessagePriority
{
    /// <summary>
    /// Lowest priority messages will not generate any notification.
    /// On iOS, the application badge number will be increased.
    /// </summary>
    Lowest = -2,

    /// <summary>
    /// Low priority messages will not generate any sound or vibration, but will still generate a popup/scrolling notification depending on the client operating system.
    /// Messages delivered during a user's quiet hours are sent as though they had a priority of low.
    /// </summary>
    Low = -1,

    /// <summary>
    /// Normal priority messages trigger sound, vibration, and display an alert according to the user's
    /// device settings.
    /// </summary>
    Normal = 0,

    /// <summary>
    /// High priority messages bypass a user's quiet hours. These messages will always play a sound
    /// and vibrate (if the user's device is configured to) regardless of the delivery time.
    /// High-priority should only be used when necessary and appropriate.
    /// </summary>
    High = 1,

    /// <summary>
    /// Emergency-priority notifications are similar to high-priority notifications,
    /// but they are repeated until the notification is acknowledged by the user.
    /// </summary>
    Emergency = 2,

    /// <summary>
    /// The default is <see cref="Normal"/>.
    /// </summary>
    Default = Normal,
}