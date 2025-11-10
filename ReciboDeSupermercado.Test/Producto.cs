namespace ReciboDeSupermercado.Test;

public class Producto
{
    public string Nombre { get; set; }
    public decimal Precio { get; set; }
    public int Cantidad { get; set; } = 1;
    public object Subtotal { get; set; }
}