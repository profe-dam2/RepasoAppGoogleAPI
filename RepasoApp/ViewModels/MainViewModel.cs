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
    public async Task ObtenerProductoAsync()
    {
        var listaProductos = await apiService.ObtenerProductos();
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