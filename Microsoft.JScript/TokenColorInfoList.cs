// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.TokenColorInfoList
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
internal class TokenColorInfoList : ITokenEnumerator
{
    private TokenColorInfo _head;
    private TokenColorInfo _current;
    private bool _atEnd;

    internal TokenColorInfoList()
    {
        this._head = (TokenColorInfo)null;
        this._current = (TokenColorInfo)null;
        this._atEnd = true;
    }

    internal void Add(Context token)
    {
        if (this._head == null)
        {
            this._head = new TokenColorInfo(token);
        }
        else
        {
            TokenColorInfo tokenColorInfo = this._head.Clone();
            this._head._next = tokenColorInfo;
            this._head.UpdateToken(token);
            this._head = tokenColorInfo;
        }

        this._current = this._head;
        this._atEnd = false;
    }

    public virtual ITokenColorInfo GetNext()
    {
        if (this._atEnd)
            return (ITokenColorInfo)null;
        ITokenColorInfo current = (ITokenColorInfo)this._current;
        this._current = this._current._next;
        this._atEnd = this._current == this._head;
        return current;
    }

    public virtual void Reset()
    {
        this._current = this._head;
        if (this._current == null)
            return;
        this._atEnd = false;
    }
}