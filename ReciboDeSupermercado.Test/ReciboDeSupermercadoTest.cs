using FluentAssertions;
using ReciboDeSupermercado.Core;

namespace ReciboDeSupermercado.Test;

public class ReciboDeSupermercadoTest
{
    private readonly Recibo _recibo;
    public ReciboDeSupermercadoTest()
    {
        _recibo = new Recibo();
    }
    
    [Fact]
    public void DeberiaCrearUnReciboVacioConTotalCero()
    {
        Assert.Equal(0m, _recibo.Total);
    }


    [Theory]
    [ClassData(typeof(DatosProductosTest))]
    public void Si_AgregarProductosAlReciboElTotal_Debe_SerLaSumaDeTodosLosPrecios(
        ProductosTestDatos productosTestDatos)
    {
        foreach (var producto in productosTestDatos.Productos)
        {
            _recibo.AgregarProducto(new ProductoDto(producto.Nombre, producto.Precio));
        }

        _recibo.Total.Should().Be(productosTestDatos.TotalEsperado);
    }

    [Fact]
    public void Si_AgregoElMismoProductoDosVeces_Debe_ExistirEnElReciboConCantidadDos()
    {
        _recibo.AgregarProducto(new ProductoDto("Cepillo de dientes", 0.99m));
        _recibo.AgregarProducto(new ProductoDto("Cepillo de dientes", 0.99m));

        _recibo.Productos.Should().ContainSingle(p => p.Nombre == "Cepillo de dientes" && p.Cantidad == 2);
    }

    [Fact]
    public void Si_AgregoElMismoProductoDosVecesElTotal_Debe_MultiplicarSuValorPorLaCantidadDelProducto()
    {
        _recibo.AgregarProducto(new ProductoDto("Cepillo de dientes", 0.99m));
        _recibo.AgregarProducto(new ProductoDto("Cepillo de dientes", 0.99m));

        _recibo.Total.Should().Be(0.99m * 2);
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
        _recibo.Productos.Should().BeEmpty();
        _recibo.Total.Should().Be(0);
    }

    [Fact]
    public void Si_AgregoUnProductoElRecibo_Debe_ContenerElproducto()
    {
        _recibo.AgregarProducto(new ProductoDto("Leche", 0.99m));

        _recibo.Productos.Should().ContainSingle(p => p.Nombre == "Leche");
    }

    [Fact]
    public void Si_AgregoUnProductoSinDescripcion_Debe_ArrojarExcepcion()
    {
        Action resultado = () => _recibo.AgregarProducto(new ProductoDto("", 0.99m));

        resultado.Should().Throw<ArgumentException>()
            .WithMessage(Producto.LA_DESCRIPCION_DEL_PRODUCTO_NO_PUEDE_ESTAR_VACIA);
    }

    [Fact]
    public void Si_AgregoUnProductoConValorMenoroIgualACero_Debe_ArrojarExcepcion()
    {
        Action resultado = () => _recibo.AgregarProducto(new ProductoDto("Leche", 0m));

        resultado.Should().Throw<ArgumentException>()
            .WithMessage(Producto.EL_PRECIO_DEL_PRODUCTO_DEBE_SER_MAYOR_A_CERO);
    }

    [Fact]
    public void Si_AgregoUnProductoArrozConDescuentoPorcentualDel10_Debe_AplicarElDescuentoAlTotal()
    {
        var recibo = new Recibo();
        
        _recibo.AgregarProducto(new ProductoDto("Arroz", 2.49m));
        _recibo.AplicarPromocion(new PromocionDescuentoPorcentual("Arroz", porcentaje: 10m));
        
        _recibo.Total.Should().Be(2.241m);
    }

    [Fact]
    public void Si_Agrego3SacosDeArrosConDescuentoPorcentualDel10_Debe_AplicarElDescuentoAlTotal()
    {
        _recibo.AgregarProducto(new ProductoDto("Arroz", 2.49m));
        _recibo.AgregarProducto(new ProductoDto("Arroz", 2.49m));
        _recibo.AgregarProducto(new ProductoDto("Arroz", 2.49m));
        
        _recibo.AplicarPromocion(new PromocionDescuentoPorcentual("Arroz", porcentaje: 10m));
        
        _recibo.Total.Should().Be(6.723m);
    }

    [Fact]
    public void Si_AgregoManzanasConDescuentoPorcentualDel20_Debe_AplicarElDescuentoAlTotal()
    {
        _recibo.AgregarProducto(new ProductoDto("Arroz", 2.49m));
        _recibo.AplicarPromocion(new PromocionDescuentoPorcentual("Arroz", porcentaje: 10m));
        
        _recibo.AgregarProducto(new ProductoDto("Manzanas", 1.99m));
        _recibo.AplicarPromocion(new PromocionDescuentoPorcentual("Manzanas", 20m));
        
        _recibo.Total.Should().Be(3.833m); 
    }

    [Fact]
    public void  Si_Agrego3CepillosDeDientesConPromocion2x1Gratis_Debe_PagarSolo2()
    {
        _recibo.AgregarProducto(new ProductoDto("Cepillo de dientes", 0.99m));
        _recibo.AgregarProducto(new ProductoDto("Cepillo de dientes", 0.99m));
        _recibo.AgregarProducto(new ProductoDto("Cepillo de dientes", 0.99m));
    
        _recibo.AplicarPromocion(new PromocionLLeveXPagueX("Cepillo de dientes", compra: 2, lleva: 3));
        
        _recibo.Total.Should().Be(1.98m);
    }

    [Fact]
    public void Si_Compro5TubosDePastaDeDientesConPromocionPackPrecioFijo_Debe_Pagar749()
    {
        _recibo.AgregarProducto(new ProductoDto("Pasta de dientes", 1.79m));
        _recibo.AgregarProducto(new ProductoDto("Pasta de dientes", 1.79m));
        _recibo.AgregarProducto(new ProductoDto("Pasta de dientes", 1.79m));
        _recibo.AgregarProducto(new ProductoDto("Pasta de dientes", 1.79m));
        _recibo.AgregarProducto(new ProductoDto("Pasta de dientes", 1.79m));
        
        _recibo.AplicarPromocion(new PromocionPackPrecioFijo("Pasta de dientes", cantidad: 5, precioFijo: 7.49m));
        
        _recibo.Total.Should().Be(7.49m);
    }

    [Fact]
    public void Si_Compro7TubosDePastaDeDientesConPromocionPackPrecioFijo_Debe_AplicarEnElTotal1PackyDosSueltosConTotalDe1107()
    {
        for (int i = 0; i < 7; i++)
        {
            _recibo.AgregarProducto(new ProductoDto("Pasta de dientes", 1.79m));
        }
        
        _recibo.AplicarPromocion(new PromocionPackPrecioFijo("Pasta de dientes", cantidad:5, precioFijo: 7.49m));
        
        _recibo.Total.Should().Be(11.07m);
    }
    
    [Fact]
    public void Si_Compro10TubosDePasta_ConPromocionDe5_Debe_Aplicar2PacksEnElDescuento()
    {
        for (int i = 0; i < 10; i++)
        {
            _recibo.AgregarProducto(new ProductoDto("Pasta de dientes", 1.79m));
        }
        _recibo.AplicarPromocion(new PromocionPackPrecioFijo("Pasta de dientes", cantidad: 5, precioFijo: 7.49m));
        
        _recibo.Total.Should().Be(14.98m);
    }

    [Fact]
    public void Si_Compro2CajasDeTomatesConPromocion2x099_Debe_Pagar099()
    {
        _recibo.AgregarProducto(new ProductoDto("Tomates cherry", 0.50m));
        _recibo.AgregarProducto(new ProductoDto("Tomates cherry", 0.50m));
        
        _recibo.AplicarPromocion(new PromocionPackPrecioFijo("Tomates cherry", cantidad: 2, precioFijo: 0.99m));
        
        _recibo.Total.Should().Be(0.99m);
    }
    
    [Fact]
    public void Si_Compro3CajasDeTomatesConPromocion2x099_Debe_Pagar149()
    {
        for (int i = 0; i < 3; i++)
            _recibo.AgregarProducto(new ProductoDto("Tomates cherry", 0.50m));
    
        _recibo.AplicarPromocion(new PromocionPackPrecioFijo("Tomates cherry", cantidad: 2, precioFijo: 0.99m));
    
        _recibo.Total.Should().Be(1.49m);
    }

    [Fact]
    public void Si_Compro4CajasDeTomatesConPromocion2x099_Debe_Pagar198()
    {
        for (int i = 0; i < 4; i++)
            _recibo.AgregarProducto(new ProductoDto("Tomates cherry", 0.50m));
        
        _recibo.AplicarPromocion(new PromocionPackPrecioFijo("Tomates cherry", cantidad: 2, precioFijo: 0.99m));
    
        _recibo.Total.Should().Be(1.98m);
    }

    [Fact]
    public void Si_IntentaCrearPromocionLLeveXPagueXInvalida_Debe_LanzarExcepcion()
    {
        Action accion = () => new PromocionLLeveXPagueX("Cepillo", compra: 3, lleva: 2);
        
        accion.Should().Throw<ArgumentException>()
            .WithMessage("La cantidad de llevar debe ser mayor a la de comprar"); 
    }

    [Fact]
    public void Si_GeneroReciboSinDescuentosYAgregoProductos_Debe_MostrarLosProductosYElTotal()
    {
        _recibo.AgregarProducto(new ProductoDto("Pasta de dientes", 1.79m));
        _recibo.AgregarProducto(new ProductoDto("Arroz", 2.49m));
       

        string recibo = _recibo.GenerarRecibo();
        
        recibo.Should().Contain("Pasta de dientes");
        recibo.Should().Contain("1.79");
        recibo.Should().Contain("Arroz");
        recibo.Should().Contain("2.49");
        recibo.Should().Contain("TOTAL");
        recibo.Should().Contain("4.28");
    }

    [Fact]
    public void Si_GeneroReciboConDescuentos_Debe_MostrarDetalelDeLosDescuestos()
    {
        _recibo.AgregarProducto(new ProductoDto("Arroz", 2.49m));
        _recibo.AgregarProducto(new ProductoDto("Manzanas", 1.99m));
    
        _recibo.AplicarPromocion(new PromocionDescuentoPorcentual("Arroz", 10m));
        _recibo.AplicarPromocion(new PromocionDescuentoPorcentual("Manzanas", 20m));
    
        string recibo = _recibo.GenerarRecibo();
    
        recibo.Should().Contain("SUBTOTAL");
        recibo.Should().Contain("4.48");
    
        recibo.Should().Contain("DESCUENTOS");
        
        recibo.Should().Contain("TOTAL");
        recibo.Should().Contain("3.83");
    }

    [Fact]
    public void Si_GenerarRecibo_Debe_MostrarUnidadesDeMedida()
    {
        _recibo.AgregarProducto(new ProductoDto("Manzanas", 1.99m, UnidadMedida.Kilo));
        _recibo.AgregarProducto(new ProductoDto("Arroz", 2.49m, UnidadMedida.Saco));
    
        string recibo = _recibo.GenerarRecibo();
    
        recibo.Should().Contain("kg");
        recibo.Should().Contain("saco");
    }
    
}