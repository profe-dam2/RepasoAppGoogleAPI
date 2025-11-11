using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Duende.IdentityModel.Jwk;
using Duende.IdentityModel.OidcClient;
using RepasoApp.Data;
using RepasoApp.Utils;

namespace RepasoApp.Services;

public class GoogleAuthService
{

    public async Task<Usuario> LoginAsync(Usuario user)
    {
        
        using (var ddbb = new AppDbContext())
            ddbb.Database.EnsureCreated();
        var client = await CreateClient();
        
        // Entra si el usuario tiene un token de refresco, es decir,
        // se ha logueado en el sistema alguna vez
        if (!string.IsNullOrEmpty(user.RefreshToken))
        {
            var refreshResult = await client.RefreshTokenAsync(user.RefreshToken);
            if (!refreshResult.IsError)
            {
                Console.WriteLine("Sesión restaurada");
                // Hay que actualizar el token de refresco.
                
                return user;
            }
            Console.WriteLine("No se pudo comprobar el token, se pedirá login nuevamente");
        }
        
        var result = await client.LoginAsync();
        if (result.IsError)
        {
            Console.WriteLine("ERROR AL REGISTAR USAURIO");
            return null;
        }

        var googlesub = result.User.FindFirst("sub")?.Value;
        var email = result.User.FindFirst("email")?.Value;
        var imagen =  result.User.FindFirst("picture")?.Value;
        var nombre =  result.User.FindFirst("name")?.Value;
        Console.WriteLine(googlesub +  " " + email + " " + imagen + " " + nombre);
        var db = new AppDbContext();
        var usuario = db.Usuarios.FirstOrDefault(u => u.GoogleSub == googlesub);
        if (usuario == null)
        {
            usuario = new Usuario()
            {
                GoogleSub = googlesub,
                Email = email,
                Nombre = nombre,
                ImageUrl = imagen,
                RefreshToken = result.RefreshToken
            };
            db.Usuarios.Add(usuario);
            await db.SaveChangesAsync();
            
            Console.WriteLine("USUARIO REGISTRADO CON ÉXITO");
        }
        else
        {
            Console.WriteLine("EL USUARIO YA EXISTE");
        }
        db.Dispose();
        return usuario;
    }
    
    public async Task<OidcClient> CreateClient()
    {
        using var http = new HttpClient();
        var keySet = await http.GetStringAsync("https://www.googleapis.com/oauth2/v3/certs");
        var jwks = new JsonWebKeySet(keySet);

        var options = new OidcClientOptions
        {
            Authority = "https://accounts.google.com",
            ClientId = "CLIENT-ID",
            ClientSecret = "CLIENT-SECRET",
            Scope = "openid profile email",
            RedirectUri = "http://127.0.0.1:7890/",
            Browser = new SystemBrowser(7890),
            ProviderInformation = new ProviderInformation
            {
                AuthorizeEndpoint = "https://accounts.google.com/o/oauth2/v2/auth",
                TokenEndpoint = "https://oauth2.googleapis.com/token",
                UserInfoEndpoint = "https://openidconnect.googleapis.com/v1/userinfo",
                IssuerName = "https://accounts.google.com",
                KeySet = jwks
            }
        };

        return new OidcClient(options);
    }
    
}