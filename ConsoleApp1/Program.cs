// See https://aka.ms/new-console-template for more information
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

Console.WriteLine("Hello, World!");

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
var res = await new HttpClient().PostAsync("http://r78-rc.zdrav.netrika.ru/nsi/fhir/term/ValueSet/$expand?_format=json", content);


var stringContent = await res.Content.ReadAsStringAsync();

var jObject = JObject.Parse(stringContent);
var contains = jObject.SelectToken("parameter[0].resource.expansion.contains");

foreach (var x in contains)
{
    Console.WriteLine(x["code"]);   
}
contains.Select(x => x.SelectToken("code"));
;