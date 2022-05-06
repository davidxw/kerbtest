using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Kerberos.NET;
using Kerberos.NET.Client;
using Kerberos.NET.Credentials;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

var services = new ServiceCollection();

services.AddHttpClient();

var serviceProvider = services.BuildServiceProvider();

var client = serviceProvider.GetService<HttpClient>();

var factory = LoggerFactory.Create(builder =>
{
    builder.AddConsole(opt => opt.IncludeScopes = true);
    builder.AddFilter<ConsoleLoggerProvider>(level => level >= LogLevel.Trace);
});

var kclient = new KerberosClient(logger: factory);

kclient.Configuration.Realms["KERBTEST.COM"].Kdc.Add("kerbtestvm.kerbtest.com");

kclient.Configuration.Defaults.UdpPreferenceLimit = 10000;

var kerbCred = new KerberosPasswordCredential("dwatson@kerbtest.com", "#######");
await kclient.Authenticate(kerbCred);

var ticket = await kclient.GetServiceTicket("HOST/kerbtestvm");
var header = "Negotiate " + Convert.ToBase64String(ticket.EncodeGssApi().ToArray());

var httpRequestMessage = new HttpRequestMessage
{
    Method = HttpMethod.Get,
    RequestUri = new Uri("http://kerbtestvm.kerbtest.com"),
    Headers = {
          { "Authorization", header },
      }
};

var response = client.SendAsync(httpRequestMessage).Result;

Console.WriteLine($"{response}");