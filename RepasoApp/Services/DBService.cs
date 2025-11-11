using System.Linq;
using System.Threading.Tasks;
using Avalonia.Collections;
using Microsoft.EntityFrameworkCore;
using RepasoApp.Data;

namespace RepasoApp.Services;

public class DBService
{
    // Método asícrono que retorna una lista de usuarios
    // Retorna un Task por que las opereraciones dentro del método
    // se ejecutan de forma asíncrona
    public async Task<AvaloniaList<Usuario>> ObtenerUsuarios()
    {
        // Crea la instancia del contexto de la base de datos y asegura su
        // correcta liberación de forma asíncrona
        await using var db = new AppDbContext();
        
        // Comprueba si la base de datos existe. Si no existe, la crea.
        // Esta llamada no bloquea el hilo principal.
        // await indica que además se debe esperar a realizar la llamda para
        // continuar con el resto del método
        await db.Database.EnsureCreatedAsync();

        // Ejecuta la consulta a la base de datos de forma asíncrona
        // Retorna en forma de List<Usuario>
        var lista = await db.Usuarios.ToListAsync();
        
        // Crea y devuelve un objeto tipo AvaloniaList a partir de la lista
        // obtenida
        return new AvaloniaList<Usuario>(lista);

    }
}