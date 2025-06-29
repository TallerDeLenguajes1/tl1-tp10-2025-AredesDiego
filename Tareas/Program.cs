using System.Text.Json;
using Manejo.Tarea;

var listTarea = await GetTareas();

Console.WriteLine("-----------------------------");
Console.WriteLine("TAREAS");
Console.WriteLine("-----------------------------");

// Mostrar tareas pendientes y realizadas
Console.WriteLine("--------------------------------------Tareas Pendientes-------------------------------------------------");
foreach (var tarea in listTarea)
{
    if (tarea.completed == false)
    {
        Console.WriteLine($"TITLE: {tarea.title}");
        Console.WriteLine($"COMPLETED: {tarea.completed}\n\n");
    }
}
Console.WriteLine("--------------------------------------Tareas Realizadas-------------------------------------------------");
foreach (var tarea in listTarea)
{
    if (tarea.completed == true)
    {
        Console.WriteLine($"TITLE: {tarea.title}");
        Console.WriteLine($"COMPLETED: {tarea.completed}\n\n");
    }
}
// Guardar en archivo JSON
const string NombreArchivo = "Tareas.json";
var miHelperdeArchivos = new HelperDeJson();

Console.WriteLine("--Serializando--");
var options = new JsonSerializerOptions { WriteIndented = true };
string tareasJson = JsonSerializer.Serialize(listTarea, options);
Console.WriteLine("Archivo Serializado:\n" + tareasJson);

Console.WriteLine("--Guardando--");
miHelperdeArchivos.GuardarArchivoTexto(NombreArchivo, tareasJson);

// Leer y mostrar el archivo guardado
Console.WriteLine("--Abriendo--");
string jsonDocument = miHelperdeArchivos.AbrirArchivoTexto(NombreArchivo);

Console.WriteLine("--Deserializando--");
try
{
    var tareasRecuperadas = JsonSerializer.Deserialize<List<Tarea>>(jsonDocument);
    Console.WriteLine("--Mostrando datos recuperados--");
    ImprimirTareas(tareasRecuperadas);
}
catch (JsonException ex)
{
    Console.WriteLine("Error al deserializar: " + ex.Message);
}

static async Task<List<Tarea>> GetTareas()
{
    var url = "https://jsonplaceholder.typicode.com/todos/";

    try
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        List<Tarea> listTarea = JsonSerializer.Deserialize<List<Tarea>>(responseBody);
        return listTarea;
    }
    catch (HttpRequestException e)
    {
        Console.WriteLine("Problemas de acceso a la API");
        Console.WriteLine("Message :{0} ", e.Message);
        return null;
    }
}
static void ImprimirTarea(Tarea tarea)
{
    Console.WriteLine(tarea.title + " - " + tarea.completed + " - " + tarea.userId + " - " + tarea.id);
}

static void ImprimirTareas(List<Tarea> Tareas )
{
    foreach (var item in Tareas)
    {
        ImprimirTarea(item);
    }
}
