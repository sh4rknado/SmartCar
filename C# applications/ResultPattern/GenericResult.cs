using System.Diagnostics.CodeAnalysis;

namespace ResultPattern
{
    public class Result<T> : Result
    {
        private readonly T? _value;

        protected internal Result(T? value, bool isSuccess, Error error) : base(isSuccess, error)
            => _value = value;

        [NotNull]
        public T Value => _value! ?? throw new InvalidOperationException("Result has no value");

        public static implicit operator Result<T>(T? value) => Create(value);
    }
}
