using System;
using System.IO;
using System.Reflection;

namespace Phony.Data
{
    public class AssemblyInfo
    {
        /// <summary>
        /// Get the app info from the Assembly file
        /// </summary>
        /// <param name="assembly">Assembly.GetEntryAssembly()</param>
        public AssemblyInfo(Assembly assembly)
        {
            this.assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        private readonly Assembly assembly;

        /// <summary>
        /// Gets the title property
        /// </summary>
        public string AppTitle => GetAttributeValue<AssemblyTitleAttribute>(a => a.Title, Path.GetFileNameWithoutExtension(assembly.Location));

        /// <summary>
        /// Gets the application's version
        /// </summary>
        public string Version
        {
            get
            {
                Version version = assembly.GetName().Version;
                return version is not null ? version.ToString() : "1.0.0.0";
            }
        }

        /// <summary>
        /// Gets the description about the application.
        /// </summary>
        public string Description => GetAttributeValue<AssemblyDescriptionAttribute>(a => a.Description);

        /// <summary>
        ///  Gets the product's full name.
        /// </summary>
        public string Product => GetAttributeValue<AssemblyProductAttribute>(a => a.Product);

        /// <summary>
        /// Gets the copyright information for the product.
        /// </summary>
        public string Copyright => GetAttributeValue<AssemblyCopyrightAttribute>(a => a.Copyright);

        /// <summary>
        /// Gets the company information for the product.
        /// </summary>
        public string Company => GetAttributeValue<AssemblyCompanyAttribute>(a => a.Company);

        protected string GetAttributeValue<TAttr>(Func<TAttr, string> resolveFunc, string defaultResult = null) where TAttr : Attribute
        {
            object[] attributes = assembly.GetCustomAttributes(typeof(TAttr), false);
            return attributes.Length > 0 ? resolveFunc((TAttr)attributes[0]) : defaultResult;
        }
    }
}