namespace ResultPattern
{
    public record Error(string Code, string Message)
    {
        public static Error None = new(string.Empty, string.Empty);
        public static Error Null = new("Create error", "unable to create the value");

    }
}
