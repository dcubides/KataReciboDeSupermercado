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
    public void Si_AgregarProductosAlReciboElTotal_Debe_SerLaSumaDeTodosLosPrecios(
        ProductosTestDatos productosTestDatos)
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

    [Fact]
    public void CadaProductoDebeTenerUnSubtotal_IgualAlPrecioPorCantidad()
    {
        var producto = new Producto("Arroz", 2.49m);
        producto.IncrementarCantidad();
        producto.IncrementarCantidad();

        producto.Subtotal.Should().Be(2.49m * 3);
    }

    [Fact]
    public void Si_CreoUnRecibo_NoDebe_TenerProductosPeroDebeEstarListoParaAgregarlos()
    {
        var recibo = new Recibo();

        recibo.Productos.Should().BeEmpty();
        recibo.Total.Should().Be(0);
    }

    [Fact]
    public void Si_AgregoUnProductoElRecibo_Debe_ContenerElproducto()
    {
        var recibo = new Recibo();

        recibo.AgregarProducto("Leche", 0.99m);

        recibo.Productos.Should().ContainSingle(p => p.Nombre == "Leche");
    }

    [Fact]
    public void Si_AgregoUnProductoSinDescripcion_Debe_ArrojarExcepcion()
    {
        var recibo = new Recibo();

        Action resultado = () => recibo.AgregarProducto("", 0.99m);

        resultado.Should().Throw<ArgumentException>()
            .WithMessage(Producto.LA_DESCRIPCION_DEL_PRODUCTO_NO_PUEDE_ESTAR_VACIA);
    }

    [Fact]
    public void Si_AgregoUnProductoConValorMenoroIgualACero_Debe_ArrojarExcepcion()
    {
        var recibo = new Recibo();

        Action resultado = () => recibo.AgregarProducto("Leche", 0m);

        resultado.Should().Throw<ArgumentException>()
            .WithMessage(Producto.EL_PRECIO_DEL_PRODUCTO_DEBE_SER_MAYOR_A_CERO);
    }

    [Fact]
    public void Si_AgregoUnProductoArrozConDescuentoPorcentualDel10_Debe_AplicarElDescuentoAlTotal()
    {
        var recibo = new Recibo();
        
        recibo.AgregarProducto("Arroz", 2.49m);
        recibo.AplicarPromocion("Arroz", porcentaje:10);
        
        recibo.Total.Should().Be(2.241m);
    }

    [Fact]
    public void Si_Agrego3SacosDeArrosConDescuentoPorcentualDel10_Debe_AplicarElDescuentoAlTotal()
    {
        var recibo = new Recibo();
        
        recibo.AgregarProducto("Arroz", 2.49m);
        recibo.AgregarProducto("Arroz", 2.49m);
        recibo.AgregarProducto("Arroz", 2.49m);
        
        recibo.AplicarPromocion("Arroz", 10);
        
        recibo.Total.Should().Be(6.723m);
    }
}

public class Recibo
{
    private readonly List<Producto> _productos = new();
    private decimal _descuentoTotal = 0m;
    public IReadOnlyCollection<Producto> Productos => _productos.AsReadOnly();
    public decimal Total => Productos.Sum(p => p.Subtotal) - _descuentoTotal;
    

    public void AgregarProducto(string productoDescripcion, decimal precio)
    {
        var productoExistente = _productos.Find(p => p.Nombre == productoDescripcion);

        if (productoExistente != null)
        {
            productoExistente.IncrementarCantidad();
        }
        else
        {
            _productos.Add(new Producto(productoDescripcion, precio));
        }
    }

    public void AplicarPromocion(string nombreProducto, int porcentaje)
    {
        var producto = _productos.Find(p => p.Nombre == nombreProducto);

        if (producto != null)
        {
            _descuentoTotal += producto.Subtotal * (porcentaje / 100m);
        }
    }
}