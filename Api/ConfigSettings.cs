using System.Fabric;
using System.Fabric.Description;

namespace Api
{
    public class ConfigSettings
    {
        public ConfigSettings(StatelessServiceContext context)
        {
            context.CodePackageActivationContext.ConfigurationPackageModifiedEvent += this.CodePackageActivationContext_ConfigurationPackageModifiedEvent;
            this.UpdateConfigSettings(context.CodePackageActivationContext.GetConfigurationPackageObject("Config").Settings);
        }

        public string GeographyServiceName { get; private set; }

        //public string StatefulBackendServiceName { get; private set; }

        //public string StatelessBackendServiceName { get; private set; }

        //public string ActorBackendServiceName { get; private set; }

        public int ReverseProxyPort { get; private set; }

        private void CodePackageActivationContext_ConfigurationPackageModifiedEvent(object sender, PackageModifiedEventArgs<ConfigurationPackage> e)
        {
            this.UpdateConfigSettings(e.NewPackage.Settings);
        }

        private void UpdateConfigSettings(ConfigurationSettings settings)
        {
            ConfigurationSection section = settings.Sections["MyConfigSection"];
            //this.GuestExeBackendServiceName = section.Parameters["GuestExeBackendServiceName"].Value;
            //this.StatefulBackendServiceName = section.Parameters["StatefulBackendServiceName"].Value;
            //this.StatelessBackendServiceName = section.Parameters["StatelessBackendServiceName"].Value;
            this.GeographyServiceName = section.Parameters["GeographyServiceName"].Value;
            this.ReverseProxyPort = int.Parse(section.Parameters["ReverseProxyPort"].Value);
        }
    }
}