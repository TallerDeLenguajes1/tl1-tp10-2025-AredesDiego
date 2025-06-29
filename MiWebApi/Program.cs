using Manejo.CatFacts;
using System.Text.Json;

var gato = await GetGato();

Console.WriteLine("FACTS: " + gato.fact);
Console.WriteLine("LENGTH: " + gato.length);

const string NombreArchivo = "CAT-FACT.json";
var miHelper = new HelperDeJsonGatos();

Console.WriteLine("--Serializando--");
var options = new JsonSerializerOptions { WriteIndented = true };
string gatoJson = JsonSerializer.Serialize(gato, options);
Console.WriteLine("Archivo Serializado:\n" + gatoJson);

Console.WriteLine("--Guardando--");
miHelper.GuardarArchivoTexto(NombreArchivo, gatoJson);

Console.WriteLine("--Abriendo--");
string jsonDocument = miHelper.AbrirArchivoTexto(NombreArchivo);

if (string.IsNullOrWhiteSpace(jsonDocument))
{
    Console.WriteLine("El archivo está vacío o no se pudo leer.");
    return;
}

Console.WriteLine("--Deserializando--");
try
{
    var GatoRecuperado = JsonSerializer.Deserialize<CatFacts>(jsonDocument);
    Console.WriteLine("--Mostrando datos recuperados--");
    Console.WriteLine($"{GatoRecuperado.fact} - {GatoRecuperado.length}");
}
catch (JsonException ex)
{
    Console.WriteLine("Error al deserializar: " + ex.Message);
}

static async Task<CatFacts> GetGato()
{
    var url = "https://catfact.ninja/fact";

    try
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();

        CatFacts fact = JsonSerializer.Deserialize<CatFacts>(responseBody);
        return fact;
    }
    catch (HttpRequestException e)
    {
        Console.WriteLine("Problemas de acceso a la API");
        Console.WriteLine("Mensaje: " + e.Message);
        return null;
    }
}
