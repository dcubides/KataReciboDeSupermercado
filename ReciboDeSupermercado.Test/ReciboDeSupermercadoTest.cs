using System.Collections;
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

    [Fact]
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
}

public class DatosProductosTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new ProductosTestDatos
            {
                Productos = new List<Producto>
                {
                    new() { Nombre = "Cepillo de dientes", Precio = 0.99m }
                },
                TotalEsperado = 0.99m
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class ProductosTestDatos
{
    public List<Producto> Productos { get; set; } = new();
    public decimal TotalEsperado { get; set; }
}

public class Producto
{
    public string Nombre { get; set; }
    public decimal Precio { get; set; }
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