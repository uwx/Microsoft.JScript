# Microsoft.JScript for .NET 10

A decompiled and re-targeted copy of the classic Microsoft JScript engine and its
command-line compiler (`jsc.exe`), running on **.NET 10**.

The repository contains two projects:

| Project | Output | Purpose |
|---|---|---|
| `Microsoft.JScript` | `Microsoft.JScript.dll` | The script engine: parser/compiler, VSA engine, `eval`, CodeDOM provider |
| `jsc` | `jsc.exe` / `jsc.dll` | The `jsc` command-line compiler built on the engine |

Compiled output is emitted with `PersistedAssemblyBuilder` as real, loadable PE
files, so compiled scripts can be saved to disk, loaded back, and inspected with
normal .NET tooling.

---

## Requirements

- **.NET 10 SDK** to build.
- **.NET 10 runtime** to run `jsc` and any assemblies it produces.
- Windows is recommended for the COM/ActiveX features (`ActiveXObject`, `GetObject`);
  the core language and engine are otherwise platform-neutral.

---

## Repository layout

```
Microsoft.JScript.sln
Microsoft.JScript/            # engine library
  Compat/                     # .NET Core shims (persisted emit, IExpando, call context, ...)
  Vsa/                        # VSA engine implementation
  tests/                      # differential test corpora used during the port
jsc/                          # command-line compiler
```

---

## Build

```bash
dotnet build Microsoft.JScript.sln -c Release
```

Outputs:

```
Microsoft.JScript/bin/Release/net10.0/Microsoft.JScript.dll
jsc/bin/Release/net10.0/jsc.exe           # apphost
jsc/bin/Release/net10.0/jsc.dll           # managed entry point
```

For a folder you can copy and distribute:

```bash
dotnet publish jsc -c Release -o out/jsc
out/jsc/jsc.exe /?
```

`jsc.exe` must stay next to `Microsoft.JScript.dll`, `System.CodeDom.dll` and
`System.Security.Permissions.dll`; `dotnet publish` puts them all in one folder.
You can also run the managed entry point directly:

```bash
dotnet jsc/bin/Release/net10.0/jsc.dll /?
dotnet run --project jsc -- /?
```

---

## Using the compiler (`jsc`)

### Quick start

```bash
jsc /out:hello.exe hello.js
```

```js
// hello.js
var msg = "hello";
print("OK " + msg);
```

`jsc` prints its banner unless `/nologo` is given, and writes no diagnostics on
success. The process returns `0` on success and `10` on compiler errors or bad
options. The default target is a console application and the default output name
is the first source file's base name with `.exe` in the current directory.

Multiple source files are compiled together:

```bash
jsc /out:app.exe part1.js part2.js
```

### Options

| Option | Meaning | Status in this port |
|---|---|---|
| `/out:<file>` | Output file name | Supported |
| `/t[arget]:exe` | Console application (default) | Supported |
| `/t[arget]:winexe` | Windows application (GUI subsystem) | Supported with a Windows Forms reference (see notes) |
| `/t[arget]:library` | Library assembly (`.dll`) | Supported |
| `/platform:<platform>` | `x86`, `Itanium`, `x64`, `anycpu` | Accepted; output stays IL-only "AnyCPU" (I386) |
| `/autoref[+|-]` | Auto-reference assemblies from used namespaces | Supported (on by default) |
| `/lib:<path>` | Additional reference search paths | Supported |
| `/r[eference]:<list>` | Reference assemblies (`;`-separated names or paths) | Supported |
| `/win32res:<file>` | Embed a Win32 `.res` file | Accepted, **not embedded** (no managed equivalent) |
| `/res[ource]:<file>[,<name>[,public\|private]]` | Embed a managed `.resources` file | Supported |
| `/linkres[ource]:<file>[,...]` | Link a resource file | Accepted, **not emitted** |
| `/debug[+|-]` | Emit a portable `.pdb` next to the output | Supported |
| `/fast[+|-]` | Disable language features for better codegen | Supported (on by default) |
| `/warnaserror[+|-]` | Treat warnings as errors | Supported |
| `/w[arn]:<0-4>` | Warning level | Supported |
| `/d[efine]:<symbols>` | Conditional compilation symbols (`NAME` or `NAME=VALUE`, `,`/`;` separated) | Supported |
| `/codepage:<id>` | Source file code page | Supported |
| `/lcid:<id>` | Compiler message locale | Supported |
| `/nostdlib[+|-]` | Do not import the standard library | Supported |
| `/utf8output[+|-]` | UTF-8 compiler output | Supported |
| `/versionsafe[+|-]` | Version-safe binding | Supported |
| `/print[+|-]` | Print assembly resolution details | Supported |
| `/nologo` | Suppress the banner | Supported |
| `@<file>` | Response file | Supported |
| `/?`, `/help` | Show help | Supported |

### Examples

**Library + reference**

```bash
jsc /t:library /out:calc.dll calc.js
jsc /r:calc.dll /out:app.exe app.js
```

```js
// calc.js
class Calc {
    function add(a : int, b : int) : int { return a + b; }
}
```

```js
// app.js
var c = new Calc();
if (c.add(20, 22) !== 42) throw new Error("bad");
print("ok");
```

**Conditional compilation**

```js
@if (@DEBUG)
    var mode = "debug";
@else
    var mode = "release";
@end
print(mode + " " + @LEVEL);
```

```bash
jsc /define:DEBUG /define:LEVEL=3 /out:app.exe app.js
```

**Embedded resources**

```bash
jsc /res:strings.resources /out:app.exe app.js
```

```js
import System;
import System.Reflection;
import System.Resources;

var asm = Assembly.GetExecutingAssembly();
var rm = new ResourceManager("strings", asm);   // base name of strings.resources
print(rm.GetString("Greeting"));
```

**Debug build and response files**

```bash
jsc /debug /out:app.exe app.js     # also writes app.pdb
jsc @build.rsp                     # one option or file per line
```

### Running compiled output

The generated files are ordinary .NET assemblies, but `jsc` does **not** write an
apphost or `runtimeconfig.json`, and the output still needs the engine assembly at
runtime. To run a compiled `.exe`:

1. Add `<name>.runtimeconfig.json` next to it:

   ```json
   {
     "runtimeOptions": {
       "tfm": "net10.0",
       "framework": { "name": "Microsoft.NETCore.App", "version": "10.0.0" }
     }
   }
   ```

2. Copy `Microsoft.JScript.dll`, `System.Security.Permissions.dll` and
   `System.CodeDom.dll` next to the output (they are all in the published `jsc`
   folder), or install them where the host can find them.

```bash
dotnet exec hello.exe
```

Compiled **libraries** need no runtime config. They can be referenced by further
`jsc` compilations (`/r:`), loaded with `Assembly.LoadFrom`, or used through
reflection. They cannot currently be added as a normal `<Reference>` in a C#
project, because the emitted metadata references `System.Private.CoreLib` rather
than the `System.Runtime` reference contract (see limitations).

---

## Using the library

Reference the library from a .NET 10 project:

```xml
<ItemGroup>
  <ProjectReference Include="..\Microsoft.JScript\Microsoft.JScript.csproj" />
</ItemGroup>
```

`System.CodeDom` and `System.Security.Permissions` are pulled in transitively.

### Evaluate expressions and scripts

```csharp
using Microsoft.JScript;
using Microsoft.JScript.Vsa;

VsaEngine engine = VsaEngine.CreateEngine();
Console.WriteLine(Eval.JScriptEvaluate("6 * 7", engine));          // 42
Console.WriteLine(Eval.JScriptEvaluate(
    "var counter = 20; function bump(n){return counter+n;} bump(22);", engine)); // 42
```

Each `JScriptEvaluate` call uses its own eval scope, so declarations must be in
the same string as their use.

### Compile JScript classes and call them from C#

```csharp
using System.CodeDom.Compiler;
using Microsoft.JScript;

var provider = new JScriptCodeProvider();
var options = new CompilerParameters { GenerateInMemory = true, GenerateExecutable = false };
CompilerResults r = provider.CompileAssemblyFromSource(options,
    "class Calc { function add(a : int, b : int) : int { return a + b; } }");

if (r.Errors.HasErrors) { /* inspect r.Output / r.Errors */ }

Type calc = r.CompiledAssembly.GetType("Calc");
object instance = Activator.CreateInstance(calc);
int sum = (int)calc.GetMethod("add").Invoke(instance, new object[] { 19, 23 }); // 42
```

### Compile to disk

```csharp
var provider = new JScriptCodeProvider();
var options = new CompilerParameters
{
    GenerateInMemory = false,
    GenerateExecutable = false,      // true for an .exe
    OutputAssembly = @"C:\out\Calc.dll",
    IncludeDebugInformation = true   // also writes Calc.pdb
};
provider.CompileAssemblyFromSource(options,
    "class Calc { function add(a : int, b : int) : int { return a + b; } }");
```

### Embed the engine directly

Use `VsaEngine` with code items when you need persistent globals, host callbacks,
or control over the output file:

```csharp
var engine = new VsaEngine(true);
engine.RootMoniker = "JScript.Vsa.VsaEngine://Microsoft.JScript.VsaEngine.Vsa";
engine.Site = new MySite();                       // IJSVsaSite implementation
engine.InitNew();                                 // or InitVsaEngine(rootMoniker, site)
engine.SetOption("output", @"C:\out\script.dll"); // optional: persist to disk
engine.GenerateDebugInfo = true;                  // optional: portable PDB

var item = (IJSVsaCodeItem)engine.Items.CreateItem(
    "code", JSVsaItemType.Code, JSVsaItemFlag.None);
item.SourceText = File.ReadAllText("script.js");

if (engine.Compile())
    engine.Run();
```

`IJSVsaSite` lets the host receive compiler errors (`OnCompilerError`), supply
globals (`GetGlobalInstance`) and receive compiled state (`GetCompiledState`).

### Raw PE/PDB bytes

```csharp
byte[] pe, pdb;
engine.SaveCompiledState(out pe, out pdb);   // pdb is null unless debug info is on
Assembly asm = Assembly.Load(pe, pdb);
```

`VsaEngine.GetAssembly()` returns the compiled assembly either way; for
in-memory compilation it serializes and loads the assembly for you.

---

## How it fits together

- `jsc` and `JScriptCodeProvider` both drive the same `VsaEngine` compiler.
- When the engine must produce a saved assembly (`/out`, CodeDOM with
  `GenerateInMemory = false`, or a VSA startup class), it emits through
  `PersistedAssemblyBuilder`, including optional portable PDBs and embedded
  managed resources.
- When it only needs to run code (`Eval`, in-memory CodeDOM), it uses the
  runnable in-memory emitter.

---

## Known limitations

- **Win32 resources** (`/win32res`) and **linked resources** (`/linkresource`) are
  accepted but not written into the output.
- **`/t:winexe`** makes the compiler reference `System.Windows.Forms.dll`, which is
  not part of the default .NET 10 runtime; the compile fails with `JS2001` unless
  those reference assemblies are available. Add `<FrameworkReference
  Include="Microsoft.WindowsDesktop.App" />` to `jsc/jsc.csproj`, or pass
  `/r:<path to System.Windows.Forms.dll>`. The output then uses the Windows GUI
  subsystem, and `print` is unavailable (as with the original `jsc`).
- **`/platform`** is accepted but the output is always IL-only with the `I386`
  machine (AnyCPU-style); it still runs on all supported architectures.
- Assemblies emitted by the engine reference **`System.Private.CoreLib`**, so C#
  projects cannot consume them via a plain `<Reference>`. Consume them through
  reflection, through another `jsc` compilation, or from a host that loads them at
  runtime.
- `VsaEngine.Interrupt()` throws `PlatformNotSupportedException`
  (`Thread.Abort` no longer exists).
- Reflection-only assembly loading is approximated with a normal load.
- COM/ActiveX support is Windows-only.
- Debug information is emitted as a **portable PDB**, not a Windows PDB.

---

## Tests

`Microsoft.JScript/tests/` contains the differential corpora used while porting
(expression suites, directive/conditional-compilation cases, and a generated fuzz
corpus). They were compared against the original .NET Framework
`Microsoft.JScript.dll`; differences are limited to .NET Core number formatting
and exception-type wrapping.
