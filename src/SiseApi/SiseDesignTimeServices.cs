using Humanizer;
//using Humanizer.Core.es; // Necesario para usar .Pluralize() y .Singularize()
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding; // <-- Agrega esta directiva using
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace SiseApi
{
    // Esta clase intercepta el proceso de Scaffolding
    public class SiseDesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices(IServiceCollection services)
        {
            // Reemplazamos el servicio de pluralización por defecto por el nuestro
            services.AddSingleton<IPluralizer, SpanishPluralizer>();
        }
    }

    // Nuestra implementación personalizada que fuerza el español
    public class SpanishPluralizer : IPluralizer
    {
        public string Pluralize(string name)
        {
            // Usa Humanizer con cultura ES
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es");
            return name.Pluralize(inputIsKnownToBeSingular: false) ?? name;
        }

        public string Singularize(string name)
        {
            // Usa Humanizer con cultura ES
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es");
            return name.Singularize(inputIsKnownToBePlural: false) ?? name;
        }

    }

    // Opción: MANTENER NOMBRES ORIGINALES (Sin cambios)
    public class KeepOriginalNamesPluralizer : IPluralizer
    {
        public string Pluralize(string name)
        {
            // Devuelve el nombre tal cual, sin tocarlo
            return name;
        }

        public string Singularize(string name)
        {
            // Devuelve el nombre tal cual
            return name;
        }
    }
}