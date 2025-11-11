namespace ReciboDeSupermercado.Core;

public class PromocionLLeveXPagueX : IPromocion
{
    public string NombreProducto { get; }
    private readonly int _compra;
    private readonly int _lleva;

    public PromocionLLeveXPagueX(string nombreProducto, int compra, int lleva)
    {
        if (lleva <= compra)
            throw new ArgumentException("La cantidad de llevar debe ser mayor a la de comprar");
        
        NombreProducto = nombreProducto;
        _compra = compra;
        _lleva = lleva;
    }

    public decimal CalcularDescuento(Producto producto)
    {
        if (producto.Nombre != NombreProducto)
            return 0m;

        int promocionesCompletas = producto.Cantidad / _lleva;
        int unidadesGratis = promocionesCompletas * (_lleva - _compra);

        return unidadesGratis * producto.Precio;
    }
    
    public string ObtenerDescripcion()
    {
        return $"Lleve {_lleva} Pague {_compra} en {NombreProducto}";
    }
}