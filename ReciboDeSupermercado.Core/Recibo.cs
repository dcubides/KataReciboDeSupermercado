using System.Globalization;
using System.Text;

namespace ReciboDeSupermercado.Core;

public record ProductoDto(string ProductoDescripcion, decimal Precio, UnidadMedida Unidad = UnidadMedida.Unidad);

public class Recibo
{
    private readonly List<Producto> _productos = new();
    private readonly List<Promocion> _promociones = new();
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
    
    
    public void AgregarProducto(ProductoDto productoDto)
    {
        var productoDescripcion = productoDto.ProductoDescripcion;
        var precio = productoDto.Precio;
        var unidad = productoDto.Unidad;
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

    public void AplicarPromocion(Promocion promocion)
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
            reciboImpreso.AppendLine(producto.ObtenerImpresionParaRecibo());
            
            subtotal += producto.Subtotal;
            
            foreach (var promocion in _promociones)
            {
                var descuentoAplicado = promocion.CalcularDescuento(producto);
                if(descuentoAplicado>0)
                {
                    impresionDescuentos.AppendLine(promocion.ObtenerImpresionParaRecibo(descuentoAplicado));
                }

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