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

    [Fact]
    public void Si_AgregoElMismoProductoDosVecesElTotal_Debe_MultiplicarSuValorPorLaCantidadDelProducto()
    {
        var recibo = new Recibo();
        
        recibo.AgregarProducto("Cepillo de dientes", 0.99m);
        recibo.AgregarProducto("Cepillo de dientes", 0.99m);
        
        recibo.Total.Should().Be(0.99m * 2);
    }
    
}

public class Recibo
{
    public List<Producto> Productos { get; set; } = new List<Producto>();
    public decimal Total => Productos.Sum(p => p.Precio * p.Cantidad);

    public void AgregarProducto(string productoDescripcion, decimal precio)
    {
        var productoExistente = Productos.Find(p => p.Nombre == productoDescripcion);

        if (productoExistente != null)
        {
            productoExistente.Cantidad++;
        }
        else
        {
            var nuevoProducto = new Producto
            {
                Nombre = productoDescripcion,
                Precio = precio,
                Cantidad = 1
            };
            Productos.Add(nuevoProducto);
        }

    }
}