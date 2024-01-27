namespace WebpWhack.Logging
{
    public record LogMsg
    {
        public DateTime Time { get; set; }
        public required string AppName { get; set; }
        public int ProcessId { get; set; }
        public int ThreadId { get; set; }
        public required string Msg { get; set; }
        public LogMessageType MsgType { get; set; }
    }
}
