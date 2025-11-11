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
        client.DefaultRequestHeaders.Add("apikey","eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyAgCiAgICAicm9sZSI6ICJhbm9uIiwKICAgICJpc3MiOiAic3VwYWJhc2UtZGVtbyIsCiAgICAiaWF0IjogMTY0MTc2OTIwMCwKICAgICJleHAiOiAxNzk5NTM1NjAwCn0.dc_X5iR_VP_qT0zsiyj_I_OZ2T9FtRU2BBNWN8Bu4GE");
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
    
    public async Task<bool> EliminarProducto(ProductModel producto)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, 
            "rest/v1/producto_ejemplo?id=eq."+producto.Id);
        request.Headers.Add("Prefer","return=representation");
        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error al borrar producto Status: " + response.StatusCode);
        }
        
        var body = await response.Content.ReadAsStringAsync();
        
        if (string.IsNullOrEmpty(body))
        {
            throw new Exception("Error al borrar producto Status: " + response.StatusCode);
        }
        
        return true;
    }
    
    public async Task<bool> ModificarProducto(ProductModel producto)
    {
        var jsonProduct = JsonConvert.SerializeObject(producto);
        var request = new HttpRequestMessage(HttpMethod.Patch, 
            "rest/v1/producto_ejemplo?id=eq."+producto.Id)
        {
            Content = new StringContent(jsonProduct, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("Prefer","return=representation");
        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error al actualizar producto Status: " + response.StatusCode);
        }
        
        var body = await response.Content.ReadAsStringAsync();
        
        if (string.IsNullOrEmpty(body))
        {
            throw new Exception("Error al actualizar producto Status: " + response.StatusCode);
        }
        
        return true;
    }
    

    public async Task<AvaloniaList<ProductModel>> ObtenerProductos()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "rest/v1/producto_ejemplo");
        var response = await client.SendAsync(request);
        var listaString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AvaloniaList<ProductModel>>(listaString);
        
    }
    
}