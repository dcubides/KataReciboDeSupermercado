namespace ReciboDeSupermercado.Test;

public class Recibo
{
    private readonly List<Producto> _productos = new();
    private readonly List<IPromocion> _promociones = new();
    private decimal _descuentoTotal = 0m;
    public IReadOnlyCollection<Producto> Productos => _productos.AsReadOnly();
    public decimal Total
    {
        get
        {
            decimal subtotal = Productos.Sum(p => p.Subtotal);
            decimal descuentos = CalcularDescuentoTotal();
            return subtotal - descuentos;
        }
    }
    

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

    public void AplicarPromocion(IPromocion promocion)
    {
        _promociones.Add(promocion);
    }
    
    public void AplicarDescuentoPorcentual(string nombreProducto, decimal porcentaje)
    {
        AplicarPromocion(new PromocionDescuentoPorcentual(nombreProducto, porcentaje));
    }

    public void AplicarPromocionNxM(string nombreProducto, int compra, int lleva)
    {
        AplicarPromocion(new PromocionLLeveXPagueX(nombreProducto, compra, lleva));
    }

    public void AplicarPromocionPackPrecioFijo(string nombreProducto, int cantidad, decimal precioFijo)
    {
        AplicarPromocion(new PromocionPackPrecioFijo(nombreProducto, cantidad, precioFijo));
    }

    private decimal CalcularDescuentoTotal()
    {
        decimal total = 0m;

        foreach (var producto in _productos)
        {
            foreach (var promocion  in _promociones)
            {
                total += promocion.CalcularDescuento(producto);
            }
        }
        
        return total;
    }

    // public void AplicarDescuentoPorcentual(string nombreProducto, decimal porcentaje)
    // {
    //     var producto = BuscarProducto(nombreProducto);
    //
    //     if (producto != null)
    //     {
    //         _descuentoTotal += producto.Subtotal * (porcentaje / 100m);
    //     }
    // }
    //
    // public void AplicarPromocionNxM(string nombreProducto, int compra, int lleva)
    // {
    //     var producto = BuscarProducto(nombreProducto);
    //     
    //     if (producto == null) return;
    //
    //     int promocionesCompletas = producto.Cantidad / lleva;
    //     
    //     int unidadesGratis =  promocionesCompletas * (lleva - compra);
    //
    //     decimal descuento = unidadesGratis * producto.Precio;
    //     
    //     _descuentoTotal += descuento;
    // }
    //
    // public void AplicarPromocionPackPrecioFijo(string nombreProducto, int cantidad, decimal precioFijo)
    // {
    //     var producto = BuscarProducto(nombreProducto);
    //
    //     if (producto == null) return;
    //     
    //     int packsCompletos = producto.Cantidad / cantidad;
    //
    //     decimal precioNormalPack = cantidad * producto.Precio;
    //
    //     decimal ahorroPorPack = precioNormalPack - precioFijo;
    //
    //     decimal descuento = packsCompletos * ahorroPorPack;
    //
    //     _descuentoTotal += descuento;
    // }
    
    private Producto? BuscarProducto(string nombreProducto)
    {
        return _productos.Find(p => p.Nombre == nombreProducto);
    }
}

public interface IPromocion
{
    string NombreProducto { get; }
    decimal CalcularDescuento(Producto producto);
}

public class PromocionDescuentoPorcentual : IPromocion
{
    public string NombreProducto { get; }
    private readonly decimal _porcentaje;

    public PromocionDescuentoPorcentual(string nombreProducto, decimal porcentaje)
    {
        NombreProducto = nombreProducto;
        _porcentaje = porcentaje;
    }

    public decimal CalcularDescuento(Producto producto)
    {
        if (producto.Nombre != NombreProducto)
            return 0m;

        return producto.Subtotal * (_porcentaje / 100m);
    }
}

public class PromocionLLeveXPagueX: IPromocion
{
    public string NombreProducto { get; }
    private readonly int _compra;
    private readonly int _lleva;
    
    public PromocionLLeveXPagueX(string nombreProducto, int compra, int lleva)
    {
        NombreProducto = nombreProducto;
        _compra = compra;
        _lleva = lleva;
    }
    
    public decimal CalcularDescuento(Producto producto)
    {
        if (producto.Nombre != NombreProducto)
            return 0m;

        int promocionesCompletas = producto.Cantidad / _lleva;
        int unidadesGratis = promocionesCompletas * (_lleva - _compra);
        
        return unidadesGratis * producto.Precio;
    }
}

public class PromocionPackPrecioFijo: IPromocion
{
    public string NombreProducto { get; }
    private readonly int _cantidad;
    private readonly decimal _precioFijo;
    
    public PromocionPackPrecioFijo(string nombreProducto, int cantidad, decimal precioFijo)
    {
        NombreProducto = nombreProducto;
        _cantidad = cantidad;
        _precioFijo = precioFijo;
    }
    public decimal CalcularDescuento(Producto producto)
    {
        if (producto.Nombre != NombreProducto)
            return 0m;

        int packsCompletos = producto.Cantidad / _cantidad;
        decimal precioNormalPack = _cantidad * producto.Precio;
        decimal ahorroPorPack = precioNormalPack - _precioFijo;
        
        return packsCompletos * ahorroPorPack;
    }
}

