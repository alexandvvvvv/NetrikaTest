namespace Netrika.Services.Utils
{
    public interface IPaginatorValidator
    {
        void Validate(int? skip, int? take);
    }

    public class PaginatorValidator : IPaginatorValidator
    {
        public void Validate(int? skip, int? take)
        {
            if (skip.GetValueOrDefault() < 0)
            {
                throw new Exception("Cannot skip negative number of objects");
            }

            take ??= take.GetValueOrDefault();
            if (take < 0)
            {
                throw new Exception("Cannot take negative number of objects");
            }
            const int maxCanTake = 1000;//todo to config or parameter
            if (take > maxCanTake) 
            {
                throw new Exception($"Cannot take more than {maxCanTake} number of objects");
            }
        }
    }
}
