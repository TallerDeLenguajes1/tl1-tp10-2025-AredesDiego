using System.Text.Json;
using Manejo.Usuarios;

var ListadoUsuarios = await GetUsuarios();
var primeros5Usuarios = new List<UsuarioReducido>();

Console.WriteLine("-----------------------------");
Console.WriteLine("Empresas");
Console.WriteLine("-----------------------------");

// Mostrar datos de las empresas
for (int i = 0; i < 5; i++)
{
    Console.WriteLine($"NAME: {ListadoUsuarios[i].name}");
    Console.WriteLine($"ADDRESS: {ListadoUsuarios[i].address}");
    Console.WriteLine($"EMAIL: {ListadoUsuarios[i].email}\n\n");

    var usuarioReducido = new UsuarioReducido
    {
        Name = ListadoUsuarios[i].name,
        Address = ListadoUsuarios[i].address,
        Email = ListadoUsuarios[i].email
    };

    primeros5Usuarios.Add(usuarioReducido);
}

// Guardar en archivo JSON
const string NombreArchivo = "Usuarios.json";
var miHelperdeArchivos = new HelperDeJsonUsuarios();

Console.WriteLine("--Serializando--");
var options = new JsonSerializerOptions { WriteIndented = true };
string UsuariosJSON = JsonSerializer.Serialize(primeros5Usuarios, options);
Console.WriteLine("Archivo Serializado:\n" + UsuariosJSON);

Console.WriteLine("--Guardando--");
miHelperdeArchivos.GuardarArchivoTexto(NombreArchivo, UsuariosJSON);

// Leer y mostrar el archivo guardado
Console.WriteLine("--Abriendo--");
string jsonDocument = miHelperdeArchivos.AbrirArchivoTexto(NombreArchivo);

Console.WriteLine("--Deserializando--");
try
{
    var UsuariosRecuperados = JsonSerializer.Deserialize<List<UsuarioReducido>>(jsonDocument);

}
catch (JsonException ex)
{
    Console.WriteLine("Error al deserializar: " + ex.Message);
}

static async Task<List<Usuarios>> GetUsuarios()
{
    var url = "https://jsonplaceholder.typicode.com/users";

    try
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        List<Usuarios> listTarea = JsonSerializer.Deserialize<List<Usuarios>>(responseBody);
        return listTarea;
    }
    catch (HttpRequestException e)
    {
        Console.WriteLine("Problemas de acceso a la API");
        Console.WriteLine("Message :{0} ", e.Message);
        return null;
    }
}

static void ImprimirUsuario(Usuarios usuario)
{
    Console.WriteLine(usuario.name + " - " + usuario.address + " - " + usuario.email);
}
static void ImprimirUsuarios(List<Usuarios> usuarios )
{
    foreach (var usuario in usuarios)
    {
        ImprimirUsuario(usuario);
    }
}
