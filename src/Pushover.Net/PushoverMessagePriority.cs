namespace Pushover.Net;

public enum PushoverMessagePriority
{
    Lowest = -2,
    Low = -1,
    Normal = 0,
    High = 1,
    Emergency = 2,

    Default = Normal,
}