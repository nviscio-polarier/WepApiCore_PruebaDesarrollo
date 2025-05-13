using static WebApiCore.Class.externos.a3innuva.A3innuvaHttpUtils;

namespace WebApiCore.Class.externos.a3innuva
{
    /// <summary>
    /// Utils para las llamadas a la API de A3innuva
    /// </summary>
    public static class A3innuvaHttpUtils
    {
        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/doc/a3innuva-n%C3%B3mina/caracteristicas-tecnicas/filtros">Filtros</a>
        /// </summary>
        public class A3innuvaFilter
        {
            public string field { get; set; }
            public string type { get; set; }
            public string value { get; set; }
            public bool isString { get; set; }
        }
    }
}

public static class StringExtensions
{
    /// <summary>
    /// Maneja la adición de parámetros a una URL, teniendo en cuenta si ya tiene parámetros o no
    /// </summary>
    /// <param name="url"></param>
    public static string HandleAddParameters(this string url)
    {
        if (url.Contains('?'))
        {
            url += "&";
        }
        else
        {
            url += "?";
        }

        return url;
    }

    /// <summary>
    /// Añade un filtro a la URL de una llamada a la API de A3innuva
    /// </summary>
    /// <param name="url"></param>
    /// <param name="filter"></param>
    public static string AddA3innuvaFilter(this string url, A3innuvaFilter? filter)
    {
        if (filter == null)
        {
            return url;
        }

        url = url.HandleAddParameters();

        string value = filter.value.Trim();

        string filterValue = filter.isString ? $"filter={filter.field} {filter.type} '{value}'" : $"filter={filter.field} {filter.type} {value}";

        return url + filterValue;
    }

    /// <summary>
    /// Añade un filtro a la URL de una llamada a la API de A3innuva
    /// </summary>
    /// <param name="url"></param>
    /// <param name="filter"></param>
    public static string AddA3innuvaFilters(this string url, string? filter)
    {
        if (filter == null || filter.Length == 0)
        {
            return url;
        }

        url = url.HandleAddParameters();

        return $"{url}filter={filter}";
    }
}