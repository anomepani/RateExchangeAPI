using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zebpay.Domain.Services;
using ZebPay.Core;
using ZebPay.Core.Services.Contracts;

namespace Zebpay.Bootstrapper
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
            ZebPay.Core.Utilities.ConfigurationSettings.ZebpayConnectionString = ConfigurationManager.ConnectionStrings["ZebPayDbConnection"].ConnectionString;
        }



        static void InitializeDependencies()
        {
            container = new UnityContainer();

            container
                .RegisterType<IRatePublisherService, RatePublisherService>();
        }

    }

}
