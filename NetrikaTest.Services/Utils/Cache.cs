using System.Diagnostics.CodeAnalysis;

namespace NetrikaTest.Services.Utils
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

        public async Task<T> GetOrAdd(Func<Task<T>> factory) //todo: always return but trigger reload
        {
            if (TryGet(out var result))
            {
                return result;
            }

            return await _lock.Execute(async () =>
            {
                if (TryGet(out result))
                {
                    return result;
                }

                _cachedValue = await factory();
                _cachedTime = DateTime.UtcNow;
                return _cachedValue;
            });
        }

        public bool TryGet([NotNullWhen(true)] out T? cache)
        {
            if (_cachedValue is not null && DateTime.UtcNow < (_cachedTime + TimeSpan.FromSeconds(_retentionInSeconds)))
            {
                cache = _cachedValue;
                return true;
            }
            else
            {
                cache = null;
                return false;
            }
        }
    }
}
