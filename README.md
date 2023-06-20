# Kerberos Test

Test applications/implementations for the following - applications only at this stage, no environment set up

https://github.com/dotnet/Kerberos.NET

https://www.codeproject.com/Articles/1272546/Authenticate-NET-Core-Client-of-SQL-Server-with-In

https://cloud.redhat.com/blog/kerberos-sidecar-container

# SqlTest 1 – Linux VM -> SQL Server

* Create SPNs for SQL Server
* Install Kerberos tools on Linux client
* Create Kerberos configuration file (krb5.conf) – specific to environment (no secrets)
* Generate keytab file (requires AD credentials)
* Get ticket from AD using kinit
* Run application
## Notes
* Expiry time for ticket can be viewed using “klist”
* Integrated auth using Kerberos for SQL Client currently broken in .NET 6.0 ([this](https://github.com/dotnet/SqlClient/issues/1390) bug) – works in .NET 5.0

# SqlTest 2 – Linux Container-> SQL Server

Following this guide - [Authenticate .NET Core Client of SQL Server with Integrated Security from Linux Docker Container - CodeProject](https://www.codeproject.com/Articles/1272546/Authenticate-NET-Core-Client-of-SQL-Server-with-In)

* Install Kerberos tools in container
* Use Kerberos configuration file (krb5.conf) from previous test – specific to environment (no secrets)
* Use keytab file from previous test (requires AD credentials) – this can be generated on any machine
* Get ticket from AD using kinit
* Run application
## Notes
One time ticket request – this test will stop working when ticket expires (restart container to get new ticket)
# SqlTest3 – Linux Container with sidecar (NYS)

Using by the sidecar approach in this article: https://cloud.redhat.com/blog/kerberos-sidecar-container
* keytab stored as secret
* Kerberos configured to use shared memory for token cache
* Sidecar container periodically refreshes the new token,  which is available to the application container
Advantages:
* Kinit-refresh container is generic – can be used by any application
* No kerb specific tools or utilities in application container – should just work if the correct volumes are mounted
* ![image](https://github.com/davidxw/kerbtest/assets/14179976/83fcdaf7-793e-4840-98ba-09781dc489d3)
