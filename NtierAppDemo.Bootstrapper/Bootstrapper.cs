using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NtierAppDemo.Domain.Services;
using NtierAppDemo.Core;
using NtierAppDemo.Core.Services.Contracts;

namespace NtierAppDemo.Bootstrapper
{
    public static class Bootstrapper
    {
        private static IUnityContainer container;

        public static void Init()
        {
            InitializeDependencies();
            InitializeSystemSettings();
        }

        public static T Locate<T>() where T : ILocatable
        {
            return container.Resolve<T>();
        }

        static void InitializeSystemSettings()
        {
            NtierAppDemo.Core.Utilities.ConfigurationSettings.NtierAppDemoConnectionString = ConfigurationManager.ConnectionStrings["NtierAppDemoDbConnection"].ConnectionString;
        }



        static void InitializeDependencies()
        {
            container = new UnityContainer();

            container
                .RegisterType<IRatePublisherService, RatePublisherService>();
        }

    }

}
