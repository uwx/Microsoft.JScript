// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.COMCharStream
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a stream that sends its data to an <see cref="T:Microsoft.JScript.IMessageReceiver" />. The data is stored in a buffer until the data is flushed or the stream is closed. </summary>
public class COMCharStream : Stream
{
  private IMessageReceiver messageReceiver;
  private StringBuilder buffer;

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.COMCharStream" /> class.</summary>
  /// <param name="messageReceiver">The receiver of the data of the stream.</param>
  public COMCharStream(IMessageReceiver messageReceiver)
  {
    this.messageReceiver = messageReceiver;
    this.buffer = new StringBuilder(128 /*0x80*/);
  }

  /// <summary>Gets a value that indicates whether the current stream supports writing.</summary>
  /// <returns>true in all cases.</returns>
  public override bool CanWrite => true;

  /// <summary>Gets a value that indicates whether the current stream supports reading.</summary>
  /// <returns>false in all cases.</returns>
  public override bool CanRead => false;

  /// <summary>Gets a value that indicates whether the current stream supports seeking.</summary>
  /// <returns>false in all cases.</returns>
  public override bool CanSeek => false;

  /// <summary>Gets the length of the stream.</summary>
  /// <returns>The length of the stream.</returns>
  public override long Length => (long) this.buffer.Length;

  /// <summary>Gets the current position in the stream. The set method does not do anything.</summary>
  /// <returns>The current position in the stream.</returns>
  public override long Position
  {
    get => (long) this.buffer.Length;
    set
    {
    }
  }

  /// <summary>Sends this stream's data to the <see cref="T:Microsoft.JScript.IMessageReceiver" /> and clears its buffer.</summary>
  public override void Close() => this.Flush();

  /// <summary>Sends this stream's data to the <see cref="T:Microsoft.JScript.IMessageReceiver" /> and clears its buffer.</summary>
  public override void Flush()
  {
    this.messageReceiver.Message(this.buffer.ToString());
    this.buffer = new StringBuilder(128 /*0x80*/);
  }

  /// <summary>Throws a <see cref="T:System.NotSupportedException" /> exception in all cases.</summary>
  /// <returns>Throws a <see cref="T:System.NotSupportedException" /> exception in all cases.</returns>
  /// <param name="buffer">An array of bytes in which to store the data that is read from the stream.</param>
  /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data that is read from the stream.</param>
  /// <param name="count">The maximum number of bytes to read from the stream.</param>
  public override int Read(byte[] buffer, int offset, int count)
  {
    throw new NotSupportedException();
  }

  /// <summary>Sets the position in the current stream. This method does not do anything. It always returns 0.</summary>
  /// <returns>0 in all cases.</returns>
  /// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
  /// <param name="origin">A value that indicates the reference point that is used to obtain the new position.</param>
  public override long Seek(long offset, SeekOrigin origin) => 0;

  /// <summary>Sets the length of the stream.</summary>
  /// <param name="value">The new length of the stream.</param>
  public override void SetLength(long value) => this.buffer.Length = (int) value;

  /// <summary>Writes the specified byte data to the stream.</summary>
  /// <param name="buffer">An array of byte data to copy to the current stream.</param>
  /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
  /// <param name="count">The number of bytes to copy from <paramref name="buffer" /> to the current stream.</param>
  public override void Write(byte[] buffer, int offset, int count)
  {
    for (int index = count; index > 0; --index)
      this.buffer.Append((char) buffer[offset++]);
  }
}
