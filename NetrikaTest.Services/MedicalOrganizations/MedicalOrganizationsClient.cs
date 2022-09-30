using Newtonsoft.Json.Linq;
using System.Net.Http.Json;

namespace NetrikaTest.Services.MedicalOrganizations
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
            var content = JsonContent.Create(new
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

            //todo
            var res = await new HttpClient().PostAsync("http://r78-rc.zdrav.netrika.ru/nsi/fhir/term/ValueSet/$expand?_format=json", content);
            var stringContent = await res.Content.ReadAsStringAsync();

            var jObject = JObject.Parse(stringContent);
            var contains = jObject.SelectToken("parameter[0].resource.expansion.contains");
            var result = new List<Organization>();

            foreach (var x in contains!)
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
