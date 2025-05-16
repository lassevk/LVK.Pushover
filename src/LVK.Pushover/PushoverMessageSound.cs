#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace LVK.Pushover;

/// <summary>
/// Represents the available notification sounds for a Pushover message.
/// </summary>
public enum PushoverMessageSound
{
    Pushover,
    Bike,
    Bugle,
    CashRegister,
    Classical,
    Cosmic,
    Falling,
    Gamelan,
    Incoming,
    Intermission,
    Magic,
    Mechanical,
    Pianobar,
    Siren,
    SpaceAlarm,
    TugBoat,
    AlienAlarm,
    Climb,
    Persistent,
    Echo,
    UpDown,
    Vibrate,
    None,

    Default = Pushover,
}