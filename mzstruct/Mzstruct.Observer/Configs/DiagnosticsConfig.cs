using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace Mzstruct.Observer.Configs
{
    public static class DiagnosticsConfig
    {
        public const string ServiceName = "CoffeeShop";

        public static Meter Meter = new(ServiceName);

        //public static Counter<int> SalesCounter = Meter.CreateCounter<int>("sales.count");
    }
}
