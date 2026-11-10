// Compatibility shims for types that existed in .NET Framework but were removed
// from .NET Core / .NET 5+.

namespace System.Runtime.InteropServices.Expando
{
    /// <summary>Enables modification of objects by adding and removing members.</summary>
    [Guid("afbf15e6-c37c-11d2-b88e-00a0c9b471b8")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    public interface IExpando : System.Reflection.IReflect
    {
        System.Reflection.FieldInfo AddField(string name);

        System.Reflection.MethodInfo AddMethod(string name, Delegate method);

        System.Reflection.PropertyInfo AddProperty(string name);

        void RemoveMember(System.Reflection.MemberInfo m);
    }
}

namespace System.CodeDom.Compiler
{
    /// <summary>Defines the type of the portable executable (PE) file that the compiler produces.</summary>
    public enum PEFileKinds
    {
        Dll = 1,
        ConsoleApplication = 2,
        WindowApplication = 3
    }
}