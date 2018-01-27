using Api.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Fabric;
using ServiceFabric.Mocks;
using Moq;
using static ServiceFabric.Mocks.MockConfigurationPackage;
using System.Fabric.Description;
using System;
using System.Threading.Tasks;

namespace Api.Test
{
    [TestClass]
    public class CountriesControllerShould
    {
        private readonly CountriesController _CountriesController;
        public CountriesControllerShould()
        {
            //build ConfigurationSectionCollection
            var configSections = new ConfigurationSectionCollection();

            //Build ConfigurationSettings
            var configSettings = CreateConfigurationSettings(configSections);

            //add one ConfigurationSection
            ConfigurationSection configSection = CreateConfigurationSection("MyConfigSection");
            configSections.Add(configSection);

            //add one Parameters entry
            ConfigurationProperty parameter1 = 
                CreateConfigurationSectionParameters("GeographyServiceName", "GeographyService");
            configSection.Parameters.Add(parameter1);

            ConfigurationProperty parameter2 =
                CreateConfigurationSectionParameters("ReverseProxyPort", "19081");
            configSection.Parameters.Add(parameter2);

            //Build ConfigurationPackage
            ConfigurationPackage configPackage = CreateConfigurationPackage(configSettings, nameof(configPackage.Path));


            MockCodePackageActivationContext codePackageActivationContext =
                (MockCodePackageActivationContext)MockCodePackageActivationContext.Default;

            codePackageActivationContext.ConfigurationPackage = configPackage;
            codePackageActivationContext.ApplicationName = "TravelConn";
            ////ICodePackageActivationContext ctx = new Mock<ICodePackageActivationContext>();

            var context = new StatelessServiceContext
                (new NodeContext("MockNode",
                new NodeId(Convert.ToInt64(0), Convert.ToInt64(0)),
                Convert.ToInt64(0), "MockNodeType", "http://localhost"),
                codePackageActivationContext,
                "MockServiceType", 
                new Uri("fabirc:/MockApp/MockService"), 
                new byte[] { }, Guid.NewGuid(), 0902930232);


            _CountriesController = new CountriesController(context,
                new System.Net.Http.HttpClient(),
                new FabricClient(), new ConfigSettings(context));
            
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            var a = await _CountriesController.GetAsync();
            //StatefulServiceContext statelessContext =
            //    new StatefulServiceContext(
            //        new NodeContext("Test", new NodeId(new BigInteger(0), new BigInteger(0)), new BigInteger(0), "TestType", "localhost"),

            //        ); 
        }
    }
}