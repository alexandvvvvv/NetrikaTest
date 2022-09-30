namespace NetrikaTest.Services.Utils
{
    public class AsyncLock
    {
        private readonly SemaphoreSlim _lock = new(1, 1);

        public async Task<T> Execute<T>(Func<Task<T>> func)
        {
            await _lock.WaitAsync();
            try
            {
                return await func();
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task Execute(Func<Task> func)
        {
            await _lock.WaitAsync();
            try
            {
                await func();
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
