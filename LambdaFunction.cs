using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SjxLogistics
{
    public class LambdaFunction : Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
    {
        [Obsolete]
        protected override void Init(IWebHostBuilder builder)
        {
            _ = builder
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup()
                .UseApiGateway();
        }
    }
}
