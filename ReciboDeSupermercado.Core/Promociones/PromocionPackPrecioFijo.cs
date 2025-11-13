using System.Globalization;

namespace ReciboDeSupermercado.Core;

public class PromocionPackPrecioFijo : Promocion
{
    public override string NombreProducto { get; }
    private readonly int _cantidad;
    private readonly decimal _precioFijo;

    public PromocionPackPrecioFijo(string nombreProducto, int cantidad, decimal precioFijo)
    {
        NombreProducto = nombreProducto;
        _cantidad = cantidad;
        _precioFijo = precioFijo;
    }

    public override decimal CalcularDescuento(Producto producto)
    {
        if (producto.Nombre != NombreProducto)
            return 0m;

        int packsCompletos = producto.Cantidad / _cantidad;
        decimal precioNormalPack = _cantidad * producto.Precio;
        decimal ahorroPorPack = precioNormalPack - _precioFijo;

        return packsCompletos * ahorroPorPack;
    }
    
    public override string ObtenerDescripcion()
    {
        return $"Pack {_cantidad}x${_precioFijo} en {NombreProducto}";
    }

    
}