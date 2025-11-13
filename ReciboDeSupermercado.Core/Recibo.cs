using System.Globalization;
using System.Text;

namespace ReciboDeSupermercado.Core;

public class Recibo
{
    private readonly List<Producto> _productos = new();
    private readonly List<Promocion> _promociones = new();
    private decimal _subtotal;
    private decimal _descuentoTotal;
    private StringBuilder _reciboImpreso;
    private StringBuilder _impresionDescuentos;
    private StringBuilder _impresionProductos;
    private readonly string _separador = "".PadRight(40, '-');

    public Recibo()
    {
        _subtotal = 0m;
        _descuentoTotal = 0m;
        _reciboImpreso = new StringBuilder();
        _impresionDescuentos = new StringBuilder();
        _impresionProductos = new StringBuilder();
    }

    public IReadOnlyCollection<Producto> Productos => _productos.AsReadOnly();

    public decimal Total => _subtotal - _descuentoTotal;


    public void AgregarProducto(Producto producto)
    {
        var productoExistente = _productos.Find(p => p.Nombre == producto.Nombre);

        if (productoExistente != null)
            productoExistente.IncrementarCantidad();
        else
            _productos.Add(producto);
    }

    public void AplicarPromocion(Promocion promocion)
    {
        _promociones.Add(promocion);
    }

    public string GenerarRecibo()
    {
        ObtenerDetallesProductosYDescuentos();

        ImprimirDetalleProductosYSubtotal();
        ImprimirDescuentos();
        ImprimirNetoAPagar();

        return _reciboImpreso.ToString();
    }

    private void ImprimirNetoAPagar()
    {
        _reciboImpreso.AppendLine(_separador);
        _reciboImpreso.AppendLine($"{"TOTAL:",-30} ${Total.ToString("F2", CultureInfo.InvariantCulture)}");
    }

    private void ImprimirDescuentos()
    {
        if (_impresionDescuentos.ToString() == "") 
            return;
        
        _reciboImpreso.AppendLine();
        _reciboImpreso.AppendLine("DESCUENTOS APLICADOS:");

        _reciboImpreso.Append(_impresionDescuentos);
        _reciboImpreso.AppendLine(_separador);
    }

    private void ImprimirDetalleProductosYSubtotal()
    {
        _reciboImpreso.Append(_impresionProductos);
        _reciboImpreso.AppendLine(_separador);
        
        _reciboImpreso.AppendLine($"{"SUBTOTAL:",-30} ${_subtotal.ToString("F2", CultureInfo.InvariantCulture)}");
    }

    private void ObtenerDetallesProductosYDescuentos()
    {
        _productos.ForEach(producto =>
        {
            ConstruirDetalleProductoYSubtotal(producto);
            
            _promociones.ForEach(promocion => CalculoDescuentosPorProducto(promocion, producto));
        });
    }

    private void ConstruirDetalleProductoYSubtotal(Producto producto)
    {
        _impresionProductos.AppendLine(producto.ObtenerImpresionParaRecibo());

        _subtotal += producto.Subtotal;
    }

    private void CalculoDescuentosPorProducto(Promocion promocion, Producto producto)
    {
        var descuentoAplicado = promocion.CalcularDescuento(producto);
        if(descuentoAplicado>0)
        {
            _impresionDescuentos.AppendLine(promocion.ObtenerImpresionParaRecibo(descuentoAplicado));
        }

        _descuentoTotal += descuentoAplicado;
    }
}