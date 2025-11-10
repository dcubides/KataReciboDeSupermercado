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
    

    [Theory]
    [ClassData(typeof(DatosProductosTest))]
    public void Si_AgregarProductosAlReciboElTotal_Debe_SerLaSumaDeTodosLosPrecios(ProductosTestDatos productosTestDatos)
    {
        var recibo = new Recibo();

        foreach (var producto in productosTestDatos.Productos)
        {
            recibo.AgregarProducto(producto.Nombre, producto.Precio);
        }
        
        recibo.Total.Should().Be(productosTestDatos.TotalEsperado);
    }

    [Fact]
    public void Si_AgregoElMismoProductoDosVeces_Debe_ExistirEnElReciboConCantidadDos()
    {
        var recibo = new Recibo();
        
        recibo.AgregarProducto("Cepillo de dientes", 0.99m);
        recibo.AgregarProducto("Cepillo de dientes", 0.99m);
        
        recibo.Productos.Should().ContainSingle(p => p.Nombre == "Cepillo de dientes" && p.Cantidad == 2);
    }
    
}

public class Recibo
{
    private decimal _total;
    public decimal Total => _total;
    public List<Producto> Productos { get; set; }

    public void AgregarProducto(string productoDescripcion, decimal precio)
    {
        _total += precio;
    }
}