using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Recipe.Monolith
{
    public class Global
    {
        public static Global Singleton
        {
            get;
            set;
        }
        public string DataPath
        {
            get;
            private set;
        }

        public Random Random
        {
            get;
            private set;
        }

        public Global(IWebHostEnvironment env)
        {
            DataPath = Path.Combine(env.WebRootPath, "App_Data");
            Random = new Random();
        }
    }
}
