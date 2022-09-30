namespace Netrika.Services.MedicalOrganizations
{
    public interface IMedicalOrganizationsValidator
    {
        void Validate(string? filter);
    }

    public class MedicalOrganizationsValidator : IMedicalOrganizationsValidator
    {
        public void Validate(string? filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return;
            }
            if (filter.Length < 2 || filter.Length > 150)
            {
                throw new Exception("Filter value must be from 2 to 150 characters");
            }
            if (filter.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).All(x => x.Length < 2))
            {
                throw new Exception("Words in filter must be from 2 to 150 characters");
            }
        }
    }
}
