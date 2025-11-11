using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace RepasoApp.Models;

public class ProductModel
{
    // [JsonProperty("id")]
    // public string Id { get; set; }
    
    [JsonProperty("ref")]
    public string Ref { get; set; }
    
    [JsonProperty("color")]
    public string Color { get; set; }
    
    [JsonProperty("peso")]
    public decimal Peso { get; set; }
    
    [JsonProperty("diametro")]
    public decimal Diametro { get; set; }
}