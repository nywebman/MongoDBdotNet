using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Tab.Assist;

namespace MongoDBdotNet.Plugins
{
    //public class SnapShotTab :ITab
    //{
    //    public object GetData(ITabContext context)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    public string Name { get; set; }
    //    public RuntimeEvent ExecuteOn { get; set; }
    //    public Type RequestContextType { get; set; }
    //}

    public class SnapShotTab : AspNetTab
    {
        //Data for tab
        public override object GetData(ITabContext context)
        {
            var requestContext = context.GetRequestContext<HttpContextBase>();
            if (requestContext.CurrentNotification == RequestNotification.BeginRequest)
            {
                context.TabStore.Set("BeginGCTotal",GC.GetTotalMemory(true));
                context.TabStore.Set("BeginTimeUTC", DateTime.UtcNow);
                return null;
            }
            //anything below will not be BeginRequest->therefore EndRequest since returning null above
            var endGCTotal = GC.GetTotalMemory(true);
            var beginGCTotal = context.TabStore.Get("BeginGCTotal");
            var endTimeUTC = DateTime.UtcNow;
            var beginTimeUTC=(context.TabStore.Get("BeginTimeUTC") as DateTime?) ?? DateTime.UtcNow;


            var memorySection = new TabSection("Begin Request","End Request");
            memorySection.AddRow()
                .Column(beginGCTotal)
                .Column(endGCTotal);

            var timeSection = new TabSection("Start Time UTC", "End Time UTC", "Time Elapsed in MS");
            timeSection.AddRow()
                .Column(beginTimeUTC)
                .Column(endTimeUTC)
                .Column(endTimeUTC.Subtract(beginTimeUTC).Milliseconds);

            var plugin = Plugin.Create("Section","Data");
            plugin.AddRow().Column("Memory Info").Column(memorySection);
            plugin.AddRow().Column("Time Info").Column(timeSection);

            return plugin;

            //return new Dictionary<string,object>
            //{
            //    {"Begin GC Total",beginGCTotal},
            //    {"End GC Total",endGCTotal},
            //    {"Begin Time",beginTimeUTC},
            //    {"End Time",endTimeUTC}
            //};

            //return new List<string>{"test data 1","test data 2","Hello World"};
        }

        //Name of tab
        public override string Name
        {
            get
            {
                return "Snapshot Tab";
            }
        }

        public override RuntimeEvent ExecuteOn
        {
            get
            {
                return RuntimeEvent.BeginRequest | RuntimeEvent.EndRequest;
            }
        }

    }
}