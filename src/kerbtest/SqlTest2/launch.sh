#!/bin/sh
kinit dwatson@KERBTEST.COM -k -t dwatson.keytab
dotnet SqlTest2.dll