namespace SmartCarProtocol
{
    public record ProtocolMessage(MessageType Type, ActionType Action, object Data);
}
