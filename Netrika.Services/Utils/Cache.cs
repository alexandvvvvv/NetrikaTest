using System.Diagnostics.CodeAnalysis;

namespace Netrika.Services.Utils
{
    public class Cached<T> where T : class
    {
        private readonly int _retentionInSeconds;
        private readonly AsyncLock _lock = new();

        private T? _cachedValue; // ensure cannot mutate outside
        private DateTime _cachedTime;

        public Cached(int retentionInSeconds)
        {
            _retentionInSeconds = retentionInSeconds;
        }

        public async Task<T> GetOrAdd(Func<Task<T>> factory)
        {
            if (IsUpToDate(out var result))
            {
                return result;
            }

            var updateTask = _lock.Execute(async () =>
            {
                if (!IsUpToDate(out _))
                {
                    _cachedValue = await factory();
                    _cachedTime = DateTime.UtcNow;
                }
                return _cachedValue!;
            });

            bool shouldAwait = result is null;

            return shouldAwait ? await updateTask : result!;
        }

        public bool IsUpToDate([NotNullWhen(true)] out T? cache)
        {
            if (_cachedValue is not null)
            {
                cache = _cachedValue;
                return DateTime.UtcNow < (_cachedTime + TimeSpan.FromSeconds(_retentionInSeconds));
            }
            else
            {
                cache = null;
                return false;
            }
        }
    }
}
