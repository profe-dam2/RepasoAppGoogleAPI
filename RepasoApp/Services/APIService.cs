using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Collections;
using Newtonsoft.Json;
using RepasoApp.Models;

namespace RepasoApp.Services;

public class APIService
{
    private HttpClient client;
    public APIService()
    {
        client = new HttpClient();
        client.BaseAddress = new Uri("http://IP:7000/");
        client.DefaultRequestHeaders.Add("apikey","");
    }

    public async Task CrearProducto(ProductModel producto)
    {
        var jsonProduct = JsonConvert.SerializeObject(producto);
        var request = new HttpRequestMessage(HttpMethod.Post, "rest/v1/producto_ejemplo")
        {
            Content = new StringContent(jsonProduct, Encoding.UTF8, "application/json")
        };
        var response = await client.SendAsync(request);
        
    }

    public async Task<AvaloniaList<ProductModel>> ObtenerProductos()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "rest/v1/producto_ejemplo");
        var response = await client.SendAsync(request);
        var listaString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AvaloniaList<ProductModel>>(listaString);
        
    }
    
}