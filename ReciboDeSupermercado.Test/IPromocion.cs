namespace ReciboDeSupermercado.Test;

public interface IPromocion
{
    string NombreProducto { get; }
    decimal CalcularDescuento(Producto producto);
}