// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.TagData
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using System;
using System.Globalization;

#nullable disable
namespace Microsoft.Surface.Core
{
  public struct TagData
  {
    private readonly int schema;
    private readonly long series;
    private readonly long extendedValue;
    private readonly long value;
    private readonly bool isNone;
    public static readonly TagData None = new TagData(true);

    private TagData(bool none)
    {
      this.schema = 0;
      this.series = 0L;
      this.extendedValue = 0L;
      this.value = 0L;
      this.isNone = none;
    }

    internal TagData(int schema, long series, long extendedValue, long value)
    {
      this.schema = schema;
      this.series = series;
      this.extendedValue = extendedValue;
      this.value = value;
      this.isNone = false;
    }

    internal TagData(long value)
      : this(0, 0L, 0L, value)
    {
    }

    internal TagData(long series, long value)
      : this(0, series, 0L, value)
    {
    }

    public int Schema => this.schema;

    public long Series => this.series;

    public long Value => this.value;

    public long ExtendedValue => this.extendedValue;

    public static bool operator !=(TagData left, TagData right) => !(left == right);

    public static bool operator ==(TagData left, TagData right)
    {
      return left.isNone == right.isNone && left.schema == right.schema && left.series == right.series && left.extendedValue == right.extendedValue && left.value == right.value;
    }

    public override bool Equals(object obj) => obj is TagData tagData && tagData == this;

    public override int GetHashCode()
    {
      return !this.isNone ? this.schema ^ (int) this.series ^ (int) (this.series >> 32) ^ (int) this.extendedValue ^ (int) (this.extendedValue >> 32) ^ (int) this.value ^ (int) (this.value >> 32) : 1;
    }

    public override string ToString()
    {
      if (this == TagData.None)
        return "None";
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Tag (Schema=0x{0:x8}; Series=0x{1:x16}; ExtendedValue=0x{2:x16}; Value=0x{3:x16})", (object) this.schema, (object) this.series, (object) this.extendedValue, (object) this.value);
    }
  }
}
