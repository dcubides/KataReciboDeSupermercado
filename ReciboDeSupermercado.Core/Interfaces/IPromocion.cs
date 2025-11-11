namespace ReciboDeSupermercado.Core;

public interface IPromocion
{
    string NombreProducto { get; }
    decimal CalcularDescuento(Producto producto);
    string ObtenerDescripcion();
}