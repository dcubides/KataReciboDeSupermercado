using System.Text;

namespace ReciboDeSupermercado.Test;

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


    public void AgregarProducto(string productoDescripcion, decimal precio, UnidadMedida? unidad)
    {
        var productoExistente = _productos.Find(p => p.Nombre == productoDescripcion);

        if (productoExistente != null)
        {
            productoExistente.IncrementarCantidad();
        }
        else
        {
            _productos.Add(new Producto(productoDescripcion, precio));
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

        foreach (var producto in _productos)
        {
            reciboImpreso.AppendLine($"{producto.Nombre,-20} x{producto.Cantidad,-5} ${producto.Subtotal}");
        }

        reciboImpreso.AppendLine("".PadRight(40, '-'));
        
        decimal subtotal = Productos.Sum(p => p.Subtotal);
        reciboImpreso.AppendLine($"{"SUBTOTAL:",-30} ${subtotal:F2}");
        
        decimal descuentoTotal = CalcularDescuentoTotal();
        if (descuentoTotal > 0)
        {
            reciboImpreso.AppendLine();
            reciboImpreso.AppendLine("DESCUENTOS APLICADOS:");
        
            foreach (var promocion in _promociones)
            {
                foreach (var producto in _productos)
                {
                    decimal descuento = promocion.CalcularDescuento(producto);
                    if (descuento > 0)
                    {
                        reciboImpreso.AppendLine($"  {promocion.ObtenerDescripcion(),-28} -${descuento:F2}");
                    }
                }
            }
        
            reciboImpreso.AppendLine("".PadRight(40, '-'));
        }


        reciboImpreso.AppendLine("".PadRight(40, '-'));
        reciboImpreso.AppendLine($"{"TOTAL:",-30} ${Total:F2}");
        
        return reciboImpreso.ToString();
    }

    
}