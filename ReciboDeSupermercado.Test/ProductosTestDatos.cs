using ReciboDeSupermercado.Core;

namespace ReciboDeSupermercado.Test;

public class ProductosTestDatos
{
    public List<Producto> Productos { get; set; } = new();
    public decimal TotalEsperado { get; set; }
}