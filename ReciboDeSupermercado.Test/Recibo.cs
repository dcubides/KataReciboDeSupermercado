namespace ReciboDeSupermercado.Test;

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

    public void AplicarPromocion(string nombreProducto, decimal porcentaje)
    {
        var producto = BuscarProducto(nombreProducto);

        if (producto != null)
        {
            _descuentoTotal += producto.Subtotal * (porcentaje / 100m);
        }
    }

    public void AplicarPromocionNxM(string nombreProducto, int compra, int lleva)
    {
        var producto = BuscarProducto(nombreProducto);
        
        if (producto == null) return;

        int promocionesCompletas = producto.Cantidad / lleva;
        
        int unidadesGratis =  promocionesCompletas * (lleva - compra);

        decimal descuento = unidadesGratis * producto.Precio;
        
        _descuentoTotal += descuento;
    }

    public void AplicarPromocionPack(string nombreProducto, int cantidad, decimal precioFijo)
    {
        var producto = BuscarProducto(nombreProducto);

        if (producto == null) return;
        
        int packsCompletos = producto.Cantidad / cantidad;
    
        decimal precioNormalPack = cantidad * producto.Precio;
    
        decimal ahorroPorPack = precioNormalPack - precioFijo;
    
        decimal descuento = packsCompletos * ahorroPorPack;
    
        _descuentoTotal += descuento;
    }
    
    public void AplicarPromocionPackPrecioFijo(string nombreProducto, int cantidad, decimal precioFijo)
    {
        AplicarPromocionPack(nombreProducto, cantidad, precioFijo);
    }
    
    private Producto? BuscarProducto(string nombreProducto)
    {
        return _productos.Find(p => p.Nombre == nombreProducto);
    }
}