using FluentAssertions;

namespace ReciboDeSupermercado.Test;

public class ReciboDeSupermercadoTest
{
    [Fact]
    public void DeberiaCrearUnReciboVacioConTotalCero()
    {
        var recibo = new Recibo();
        
        Assert.Equal(0m, recibo.Total);
    }

    [Fact]
    public void Si_AdcicionoUnProductoAlReciboElTotal_Debe_MostrarSuPrecio()
    {
        var recibo = new Recibo();

        recibo.AgregarProducto("Cepillo de dientes", 0.99m);

        recibo.Total.Should().Be(0.99m);
    }

    [Fact]
    public void Si_AdicionoDosProductosAlReciboElTotal_Debe_MostrarLaSumaDeAmbos()
    {
        var recibo = new Recibo();

        recibo.AgregarProducto("Cepillo de dientes", 0.99m);
        recibo.AgregarProducto("Arroz", 2.49m);

        recibo.Total.Should().Be(0.99m + 2.49m);
    }
    
    [Fact]
    public void Si_AdicionoTresProductosAlReciboElTotal_Debe_MostrarLaSumaDeTodos()
    {
        var recibo = new Recibo();

        recibo.AgregarProducto("Cepillo de dientes", 0.99m);
        recibo.AgregarProducto("Arroz", 2.49m);
        recibo.AgregarProducto("Tubo para pasta de dientes", 1.79m);

        recibo.Total.Should().Be(0.99m + 2.49m + 1.79m);
    }
}

public class Recibo
{
    private decimal _total;
    public decimal Total => _total;

    public void AgregarProducto(string productoDescripcion, decimal precio)
    {
        _total += precio;
    }
}