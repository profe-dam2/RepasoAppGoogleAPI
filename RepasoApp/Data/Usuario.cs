using System;

namespace RepasoApp.Data;

public class Usuario
{
    public int Id { get; set; }
    public string GoogleSub  { get; set; }
    public string Email { get; set; }
    public string Nombre { get; set; }
    public string ImageUrl { get; set; }
    public string RefreshToken { get; set; }
    public DateTime UltimoLogin { get; set; }
    public DateTime FechaRegistro { get; set; }= DateTime.Now;
}