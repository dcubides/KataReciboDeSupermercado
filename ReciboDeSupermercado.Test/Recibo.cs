namespace ReciboDeSupermercado.Test;

public class Recibo
{
    private readonly List<Producto> _productos = new();
    private readonly List<IPromocion> _promociones = new();
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

    public void PromocionLLeveXPagueX(string nombreProducto, int compra, int lleva)
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
            foreach (var promocion in _promociones)
            {
                total += promocion.CalcularDescuento(producto);
            }
        }

        return total;
    }
}