using System.Globalization;

namespace ReciboDeSupermercado.Core;

public abstract class Promocion
{
   
    public abstract string NombreProducto { get; }
    public abstract decimal CalcularDescuento(Producto producto);
    public abstract string ObtenerDescripcion();

    public string ObtenerImpresionParaRecibo(decimal descuentoAplicado)
    {
        return $"  {ObtenerDescripcion(),-28} -${descuentoAplicado.ToString("F2", CultureInfo.InvariantCulture)}";
    }
}