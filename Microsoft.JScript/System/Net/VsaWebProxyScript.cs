// Decompiled with JetBrains decompiler
// Type: System.Net.VsaWebProxyScript
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System.Net.Sockets;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;

#nullable disable
namespace System.Net;

internal class VsaWebProxyScript : MarshalByRefObject, IWebProxyScript
{
    private const string RootNamespace = "__WebProxyScript";
    private static readonly Zone IntranetZone = new Zone(SecurityZone.Intranet);
    private VsaEngine engine;
    private object scriptInstance;
    private BaseVsaStartup startupInstance;

    private const string NetScriptSource_v4 =
        "import System.Security;\r\n[assembly:System.Security.SecurityTransparent()]\r\nfunction isPlainHostName(hostName: String): Boolean { return __om.isPlainHostName(hostName); }\r\nfunction dnsDomainIs(host: String, domain: String): Boolean { return __om.dnsDomainIs(host, domain); }\r\nfunction localHostOrDomainIs(host: String, hostdom: String): Boolean { return __om.localHostOrDomainIs(host, hostdom); }\r\nfunction isResolvable(host: String): Boolean { return __om.isResolvable(host); }\r\nfunction isInNet(host: String, pattern: String, mask: String): Boolean { return __om.isInNet(host, pattern, mask); }\r\nfunction dnsResolve(host: String): String { return __om.dnsResolve(host); }\r\nfunction myIpAddress(): String { return __om.myIpAddress(); }\r\nfunction dnsDomainLevels(host: String): int { return __om.dnsDomainLevels(host); }\r\nfunction shExpMatch(str: String, pattern: String): Boolean { return __om.shExpMatch(str, pattern); }\r\nfunction weekdayRange(wd1: String, wd2: String, gmt: String): Boolean { return __om.weekdayRange(wd1, wd2, gmt); }\r\nfunction dateRange(day1, month1, year1, day2, month2, year2, gmt): Boolean { return true; }\r\nfunction timeRange(hour1, min1, sec1, hour2, min2, sec2, gmt): Boolean { return true; }\r\n";

    private const string NetScriptSource_v6ExtClass =
        "function getClientVersion(): String { return __om.getClientVersion(); }\r\nfunction sortIpAddressList(IPAddressList:String): String { return __om.sortIpAddressList(IPAddressList); }\r\nfunction isInNetEx(ipAddress:String, ipPrefix:String): Boolean { return __om.isInNetEx(ipAddress, ipPrefix); }\r\nfunction myIpAddressEx(): String { return __om.myIpAddressEx(); }\r\nfunction dnsResolveEx(host:String): String { return __om.dnsResolveEx(host); }\r\nfunction isResolvableEx(host:String): Boolean { return __om.isResolvableEx(host); }\r\nvar __RefereceOfFindProxyForURLEx = this[\"FindProxyForURLEx\"];\r\nvar bFindProxyForURLExFound : Boolean = __RefereceOfFindProxyForURLEx != null && typeof(__RefereceOfFindProxyForURLEx) == \"function\";\r\nclass __WebProxyScript { \t\r\n\t\t\t     \tfunction ExecuteFindProxyForURL(url, host): String { \r\n\t\t         \t\tif(bFindProxyForURLExFound) {\r\n\t\t         \t\t\treturn String(FindProxyForURLEx(url, host)); \r\n\t\t         \t\t}\r\n\t\t         \t\telse {\r\n\t\t\t         \t\treturn String(FindProxyForURL(url, host)); \r\n\t\t         \t\t}\r\n\t\t\t       \t} \r\n\t\t\t       }\r\n\t\t\t         ";

    private const string NetScriptSource_v4Class =
        "class __WebProxyScript { function ExecuteFindProxyForURL(url, host): String { return String(FindProxyForURL(url, host)); } }\r\n";

    private const string NetScriptSource_bindings = "var ProxyConfig = { bindings:{} };\r\n";

    public void Close()
    {
        if (this.startupInstance != null)
        {
            try
            {
                this.startupInstance.Shutdown();
                this.startupInstance = (BaseVsaStartup)null;
            }
            catch (Exception ex)
            {
                throw new JSVsaException(JSVsaError.EngineCannotReset, ex.ToString(), ex);
            }
        }

        this.engine.Close();
    }

    public string Run(string url, string host)
    {
        return VsaWebProxyScript.CallMethod(this.scriptInstance, "ExecuteFindProxyForURL", new object[2]
        {
            (object)url,
            (object)host
        }) as string;
    }

    public override object InitializeLifetimeService() => (object)null;

    [ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.MemberAccess)]
    private static object CallMethod(object targetObject, string name, object[] args)
    {
        if (targetObject == null || name == null)
            return (object)null;
        Type type = targetObject.GetType();
        Type[] types = new Type[args.Length];
        for (int index = 0; index < args.Length; ++index)
            types[index] = args[index].GetType();
        return type.GetMethod(name,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod |
                BindingFlags.CreateInstance, Type.DefaultBinder, types, (ParameterModifier[])null)
            .Invoke(targetObject, args);
    }

    public bool Load(Uri engineScriptLocation, string scriptBody, Type helperType)
    {
        byte[] pe;
        byte[] pdb;
        return this.CompileScript(engineScriptLocation, scriptBody, helperType, out pe, out pdb) &&
               this.LoadAssembly(pe, pdb);
    }

    [PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
    private bool CompileScript(
        Uri engineScriptLocation,
        string scriptBody,
        Type helperType,
        out byte[] pe,
        out byte[] pdb)
    {
        pe = (byte[])null;
        pdb = (byte[])null;
        try
        {
            this.engine = new VsaEngine();
            this.engine.RootMoniker = "pac-" + engineScriptLocation.ToString();
            this.engine.Site = (IJSVsaSite)new VsaWebProxyScript.VsaEngineSite(helperType);
            this.engine.InitNew();
            this.engine.RootNamespace = "__WebProxyScript";
            this.engine.SetOption("print", (object)false);
            this.engine.SetOption("fast", (object)false);
            this.engine.SetOption("autoref", (object)false);
            string name = typeof(SecurityTransparentAttribute).Assembly.GetName().Name + ".dll";
            (this.engine.Items.CreateItem(name, JSVsaItemType.Reference, JSVsaItemFlag.None) as IJSVsaReferenceItem)
                .AssemblyName = name;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(
                "import System.Security;\r\n[assembly:System.Security.SecurityTransparent()]\r\nfunction isPlainHostName(hostName: String): Boolean { return __om.isPlainHostName(hostName); }\r\nfunction dnsDomainIs(host: String, domain: String): Boolean { return __om.dnsDomainIs(host, domain); }\r\nfunction localHostOrDomainIs(host: String, hostdom: String): Boolean { return __om.localHostOrDomainIs(host, hostdom); }\r\nfunction isResolvable(host: String): Boolean { return __om.isResolvable(host); }\r\nfunction isInNet(host: String, pattern: String, mask: String): Boolean { return __om.isInNet(host, pattern, mask); }\r\nfunction dnsResolve(host: String): String { return __om.dnsResolve(host); }\r\nfunction myIpAddress(): String { return __om.myIpAddress(); }\r\nfunction dnsDomainLevels(host: String): int { return __om.dnsDomainLevels(host); }\r\nfunction shExpMatch(str: String, pattern: String): Boolean { return __om.shExpMatch(str, pattern); }\r\nfunction weekdayRange(wd1: String, wd2: String, gmt: String): Boolean { return __om.weekdayRange(wd1, wd2, gmt); }\r\nfunction dateRange(day1, month1, year1, day2, month2, year2, gmt): Boolean { return true; }\r\nfunction timeRange(hour1, min1, sec1, hour2, min2, sec2, gmt): Boolean { return true; }\r\n");
            if (Socket.OSSupportsIPv6)
                stringBuilder.Append(
                    "function getClientVersion(): String { return __om.getClientVersion(); }\r\nfunction sortIpAddressList(IPAddressList:String): String { return __om.sortIpAddressList(IPAddressList); }\r\nfunction isInNetEx(ipAddress:String, ipPrefix:String): Boolean { return __om.isInNetEx(ipAddress, ipPrefix); }\r\nfunction myIpAddressEx(): String { return __om.myIpAddressEx(); }\r\nfunction dnsResolveEx(host:String): String { return __om.dnsResolveEx(host); }\r\nfunction isResolvableEx(host:String): Boolean { return __om.isResolvableEx(host); }\r\nvar __RefereceOfFindProxyForURLEx = this[\"FindProxyForURLEx\"];\r\nvar bFindProxyForURLExFound : Boolean = __RefereceOfFindProxyForURLEx != null && typeof(__RefereceOfFindProxyForURLEx) == \"function\";\r\nclass __WebProxyScript { \t\r\n\t\t\t     \tfunction ExecuteFindProxyForURL(url, host): String { \r\n\t\t         \t\tif(bFindProxyForURLExFound) {\r\n\t\t         \t\t\treturn String(FindProxyForURLEx(url, host)); \r\n\t\t         \t\t}\r\n\t\t         \t\telse {\r\n\t\t\t         \t\treturn String(FindProxyForURL(url, host)); \r\n\t\t         \t\t}\r\n\t\t\t       \t} \r\n\t\t\t       }\r\n\t\t\t         ");
            else
                stringBuilder.Append(
                    "class __WebProxyScript { function ExecuteFindProxyForURL(url, host): String { return String(FindProxyForURL(url, host)); } }\r\n");
            stringBuilder.Append("var ProxyConfig = { bindings:{} };\r\n");
            stringBuilder.Append($"//@position(file=\"{engineScriptLocation.ToString()}\",line = 1)\n");
            stringBuilder.Append(scriptBody);
            (this.engine.Items.CreateItem("SourceText", JSVsaItemType.Code, JSVsaItemFlag.None) as IJSVsaCodeItem)
                .SourceText = stringBuilder.ToString();
            this.engine.Items.CreateItem("__om", JSVsaItemType.AppGlobal, JSVsaItemFlag.None);
            if (this.engine.Compile())
            {
                this.engine.SaveCompiledState(out pe, out pdb);
                return true;
            }
        }
        catch
        {
        }

        return false;
    }

    private bool LoadAssembly(byte[] pe, byte[] pdb)
    {
        try
        {
            Assembly assembly = Assembly.Load(pe, pdb);
            this.startupInstance =
                (BaseVsaStartup)Activator.CreateInstance(assembly.GetType(this.engine.RootNamespace + "._Startup"));
            this.startupInstance.SetSite(this.engine.Site);
            this.startupInstance.Startup();
            this.scriptInstance =
                Activator.CreateInstance(assembly.GetType(this.engine.RootNamespace + ".__WebProxyScript"));
            VsaWebProxyScript.CallMethod(this.scriptInstance, "SetEngine", new object[1]
            {
                (object)this.engine
            });
        }
        catch
        {
            return false;
        }

        return true;
    }

    private class VsaEngineSite : IJSVsaSite
    {
        private readonly Type m_GlobalType;

        internal VsaEngineSite(Type globalType) => this.m_GlobalType = globalType;

        public void GetCompiledState(out byte[] pe, out byte[] debugInfo)
        {
            pe = (byte[])null;
            debugInfo = (byte[])null;
        }

        public object GetEventSourceInstance(string itemName, string eventSourceName) => (object)null;

        [ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.MemberAccess)]
        public object GetGlobalInstance(string name)
        {
            if (name == "__om")
                return Activator.CreateInstance(this.m_GlobalType, true);
            throw new JSVsaException(JSVsaError.GlobalInstanceInvalid);
        }

        public void Notify(string notify, object info)
        {
        }

        public bool OnCompilerError(IJSVsaError error) => error.Severity != 0;
    }
}