using System;
using System.Threading.Tasks;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RepasoApp.Data;
using RepasoApp.Models;
using RepasoApp.Services;

namespace RepasoApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private string _greeting = "Welcome to Avalonia!";
    [ObservableProperty] private string imageURL;
    [ObservableProperty] private AvaloniaList<Usuario> listaUsuarios=new();
    [ObservableProperty] private AvaloniaList<ProductModel> listaProductos=new();
    
    private APIService apiService { get; set; } = new();

    [RelayCommand]
    public async Task CrearProductoAsync()
    {
        var p = new ProductModel()
        {
           Ref = "443342",
           Diametro = 7.38m,
           Peso = 8.62m,
           Color = "Negro",
        };
        await apiService.CrearProducto(p);
    }
    
    [RelayCommand]
    public async Task EliminarProductoAsync(ProductModel producto)
    {
        if (producto == null)
        {
            Console.WriteLine("No has seleccionado nada");
            return;
        }
        try
        {
            bool okEliminar = await apiService.EliminarProducto(producto);
            if (okEliminar)
            {
                Console.WriteLine("Producto Eliminar con éxito");
                await ObtenerProductoAsync(); //actualiza la lista
            }
        }
        catch (Exception ex)
        {
                Console.WriteLine("Error al eliminar producto" + ex.Message);
        }
    }

    [RelayCommand]
    public async Task ModificarProductoAsync(ProductModel producto)
    {
        if (producto == null)
        {
            Console.WriteLine("No has seleccionado nada");
            return;
        }
        try
        {
            producto.Ref = "REF MODIFICADA";
            bool okModificar = await apiService.ModificarProducto(producto);
            if (okModificar)
            {
                Console.WriteLine("Producto Modificado con éxito");
                await ObtenerProductoAsync();
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al actualizar producto" + ex.Message);
        }
    }
    
    [RelayCommand]
    public async Task ObtenerProductoAsync()
    {
        ListaProductos = await apiService.ObtenerProductos();
    }
    
    [RelayCommand]
    public async Task ObtenerUsuariosAsync()
    {
        ListaUsuarios = await new DBService().ObtenerUsuarios();
    }

    [RelayCommand]
    public async Task LoginUsuarioAsync(Usuario user)
    {
        var authservice = new GoogleAuthService();
        await authservice.LoginAsync(user);
    }
    
    [RelayCommand]
    public async Task RegisterUserAsync()
    {
        var authservice = new GoogleAuthService();
        Usuario usuario = await authservice.LoginAsync(new Usuario());
        ImageURL = usuario.ImageUrl;
    }
}