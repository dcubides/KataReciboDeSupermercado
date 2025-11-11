namespace ReciboDeSupermercado.Core;

public static class UnidadMedidaExtensions
{
    public static string ObtenerDescripcion(this UnidadMedida unidad)
    {
        return unidad switch
        {
            UnidadMedida.Kilo => "kg",
            UnidadMedida.Saco => "saco",
            UnidadMedida.Tubo => "tubo",
            UnidadMedida.Caja => "caja",
            UnidadMedida.Unidad => "ud",
            _ => ""
        };
    }
}