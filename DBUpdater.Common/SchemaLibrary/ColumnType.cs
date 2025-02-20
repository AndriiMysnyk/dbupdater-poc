using System.ComponentModel;

namespace DBUpdater.Common;

public enum ColumnType
{
    [Description("int")] Integer,
    [Description("fx")] Floating,
    [Description("ch")] String,
    [Description("nch")] NString,
    [Description("da")] Date,
    [Description("ts")] Timestamp,
    [Description("bl")] Blob,
    [Description("cl")] CLob,
    [Description("ncl")] NCLob,
    [Description("bit")] Bit,
    [Description("seq")] Seq,
    [Description("?")] Unknown,
}
