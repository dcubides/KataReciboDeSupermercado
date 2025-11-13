using System.Globalization;
using System.Text;

namespace ReciboDeSupermercado.Core;

public class Recibo
{
    private readonly List<Producto> _productos = new();
    private readonly List<IPromocion> _promociones = new();
    public IReadOnlyCollection<Producto> Productos => _productos.AsReadOnly();

    public decimal Total
    {
        get
        {
            decimal subtotal = Productos.Sum(p => p.Subtotal);
            decimal descuentos = CalcularDescuentoTotal();
            return subtotal - descuentos;
        }
    }


    public void AgregarProducto(string productoDescripcion, decimal precio, UnidadMedida unidad = UnidadMedida.Unidad)
    {
        var productoExistente = _productos.Find(p => p.Nombre == productoDescripcion);

        if (productoExistente != null)
        {
            productoExistente.IncrementarCantidad();
        }
        else
        {
            _productos.Add(new Producto(productoDescripcion, precio, unidad));
        }
    }

    public void AplicarPromocion(IPromocion promocion)
    {
        _promociones.Add(promocion);
    }

    private decimal CalcularDescuentoTotal()
    {
        decimal total = 0m;

        foreach (var producto in _productos)
        {
            foreach (var promocion in _promociones)
            {
                total += promocion.CalcularDescuento(producto);
            }
        }

        return total;
    }

    public string GenerarRecibo()
    {
        var reciboImpreso = new StringBuilder();
        var impresionDescuentos = new StringBuilder();
        var subtotal = 0m;
        var descuentoTotal = 0m;

        foreach (var producto in _productos)
        {
            var imprimirProducto = producto.ObtenerImpresionParaRecibo();

            reciboImpreso.AppendLine(imprimirProducto);
            subtotal += producto.Subtotal;
            
            foreach (var promocion in _promociones)
            {
                var descuentoAplicado = promocion.CalcularDescuento(producto);
                if(descuentoAplicado>0)
                    impresionDescuentos.AppendLine($"  {promocion.ObtenerDescripcion(),-28} -${descuentoAplicado.ToString("F2", CultureInfo.InvariantCulture)}");
                descuentoTotal += descuentoAplicado;
            }
        }

        reciboImpreso.AppendLine("".PadRight(40, '-'));
        
        reciboImpreso.AppendLine($"{"SUBTOTAL:",-30} ${subtotal.ToString("F2", CultureInfo.InvariantCulture)}");

        if (impresionDescuentos.ToString() != "")
        {
            reciboImpreso.AppendLine();
            reciboImpreso.AppendLine("DESCUENTOS APLICADOS:");

            reciboImpreso.Append(impresionDescuentos);
            reciboImpreso.AppendLine("".PadRight(40, '-'));
        }
        

        reciboImpreso.AppendLine("".PadRight(40, '-'));
        reciboImpreso.AppendLine($"{"TOTAL:",-30} ${Total.ToString("F2", CultureInfo.InvariantCulture)}");
        
        return reciboImpreso.ToString();
    }

    
}