using System.Collections;
using ReciboDeSupermercado.Core;

namespace ReciboDeSupermercado.Test;

public class DatosProductosTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new ProductosTestDatos
            {
                Productos = new List<Producto>
                {
                    new("Cepillo de dientes", 0.99m)
                },
                TotalEsperado = 0.99m
            }
        };

        yield return new object[]
        {
            new ProductosTestDatos()
            {
                Productos = new List<Producto>
                {
                    new("Cepillo de dientes", 0.99m),
                    new ("Arroz", 2.49m)
                },
                TotalEsperado = 0.99m + 2.49m
            },
        };

        yield return new object[]
        {
            new ProductosTestDatos()
            {
                Productos = new List<Producto>
                {
                    new("Cepillo de dientes", 0.99m),
                    new ("Arroz", 2.49m),
                    new ("Tubo para pasta de dientes", 1.79m)
                },
                TotalEsperado = 0.99m + 2.49m + 1.79m
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}