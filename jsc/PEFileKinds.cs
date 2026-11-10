using System;
using System.Runtime.InteropServices;

/// <summary>Specifies the type of the portable executable (PE) file.</summary>
// Token: 0x02000374 RID: 884
[ComVisible(true)]
[Serializable]
public enum PEFileKinds
{
    /// <summary>The portable executable (PE) file is a DLL.</summary>
    // Token: 0x040015AB RID: 5547
    Dll = 1,

    /// <summary>The application is a console (not a Windows-based) application.</summary>
    // Token: 0x040015AC RID: 5548
    ConsoleApplication,

    /// <summary>The application is a Windows-based application.</summary>
    // Token: 0x040015AD RID: 5549
    WindowApplication
}