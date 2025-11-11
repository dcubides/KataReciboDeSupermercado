namespace ReciboDeSupermercado.Core;

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

    public string ObtenerDescripcion()
    {
        return $"{_porcentaje}% en {NombreProducto}";
    }
}