using Newtonsoft.Json.Linq;
using System.Net.Http.Json;

namespace Netrika.Services.MedicalOrganizations
{
    public class Organization
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
    }

    public interface IMedicalOrganizationsClient
    {
        Task<IReadOnlyCollection<Organization>> List();
    }

    public class MedicalOrganizationsClient : IMedicalOrganizationsClient
    {
        public async Task<IReadOnlyCollection<Organization>> List()
        {
            //todo extract params to configuration ?
            var requestBody = JsonContent.Create(new
            {
                resourceType = "Parameters",
                parameter = new[]
                {
                    new
                    {
                        name = "system",
                        valueString = "urn:oid:1.2.643.2.69.1.1.1.64"
                    }
                }
            });

            var res = await new HttpClient().PostAsync("http://r78-rc.zdrav.netrika.ru/nsi/fhir/term/ValueSet/$expand?_format=json", requestBody);
            var stringContent = await res.Content.ReadAsStringAsync();

            var jObject = JObject.Parse(stringContent);
            var containsToken = jObject.SelectToken("parameter[0].resource.expansion.contains");
            var result = new List<Organization>();

            foreach (var x in containsToken!)
            {
                result.Add(new()
                {
                    Id = Guid.Parse((string)x["code"]!),
                    Name = (string)x["display"]!
                });
            }

            return result;
        }
    }
}
