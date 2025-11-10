namespace ReciboDeSupermercado.Test;

public class Producto
{
    public const string LA_DESCRIPCION_DEL_PRODUCTO_NO_PUEDE_ESTAR_VACIA =
        "El nombre del producto no puede estar vacía";

    public const string? EL_PRECIO_DEL_PRODUCTO_DEBE_SER_MAYOR_A_CERO = 
        "El precio del producto debe ser mayor a cero.";
    public string Nombre { get; }
    public decimal Precio { get;  }
    public int Cantidad { get; private set; }
    public decimal Subtotal => Cantidad * Precio;

    public Producto(string nombre, decimal precio)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException(LA_DESCRIPCION_DEL_PRODUCTO_NO_PUEDE_ESTAR_VACIA);
        if (precio <= 0)
            throw new ArgumentException(EL_PRECIO_DEL_PRODUCTO_DEBE_SER_MAYOR_A_CERO);

        Nombre = nombre;
        Precio = precio;
        Cantidad = 1;
    }
    
    public void IncrementarCantidad() => Cantidad++;
}