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

        yield return new object[]
        {
            new ProductosTestDatos()
            {
                Productos = new List<Producto>
                {
                    new() { Nombre = "Cepillo de dientes", Precio = 0.99m },
                    new () { Nombre = "Arroz", Precio = 2.49m }
                },
                TotalEsperado = 0.99m + 2.49m
            },
        };

        yield return new object[]
        {
            new ProductosTestDatos()
            {
                Productos = new List<Producto>
                {
                    new() { Nombre = "Cepillo de dientes", Precio = 0.99m },
                    new () { Nombre = "Arroz", Precio = 2.49m },
                    new () {Nombre = "Tubo para pasta de dientes",  Precio = 1.79m }
                },
                TotalEsperado = 0.99m + 2.49m + 1.79m
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