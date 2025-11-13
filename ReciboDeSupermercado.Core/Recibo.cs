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
        {
            productoExistente.IncrementarCantidad();
        }
        else
        {
            _productos.Add(producto);
        }
    }

    public void AplicarPromocion(Promocion promocion)
    {
        _promociones.Add(promocion);
    }

    public string GenerarRecibo()
    {
        ObtenerDetallesProductosYDescuentos();

        _reciboImpreso.Append(_impresionProductos);
        _reciboImpreso.AppendLine(_separador);
        
        _reciboImpreso.AppendLine($"{"SUBTOTAL:",-30} ${_subtotal.ToString("F2", CultureInfo.InvariantCulture)}");

        if (_impresionDescuentos.ToString() != "")
        {
            _reciboImpreso.AppendLine();
            _reciboImpreso.AppendLine("DESCUENTOS APLICADOS:");

            _reciboImpreso.Append(_impresionDescuentos);
            _reciboImpreso.AppendLine(_separador);
        }
        

        _reciboImpreso.AppendLine(_separador);
        _reciboImpreso.AppendLine($"{"TOTAL:",-30} ${Total.ToString("F2", CultureInfo.InvariantCulture)}");
        
        return _reciboImpreso.ToString();
    }

    private void ObtenerDetallesProductosYDescuentos()
    {
        foreach (var producto in _productos)
        {
            
            _impresionProductos.AppendLine(producto.ObtenerImpresionParaRecibo());
            
            _subtotal += producto.Subtotal;
            
            foreach (var promocion in _promociones)
            {
                CalculoDescuentosPorProducto(promocion, producto);
            }
        }
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