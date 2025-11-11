namespace ReciboDeSupermercado.Test;

public class PromocionPackPrecioFijo : IPromocion
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