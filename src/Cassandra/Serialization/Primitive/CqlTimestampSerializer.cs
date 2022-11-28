namespace Cassandra.Serialization.Primitive
{
    internal class CqlTimestampSerializer : TypeSerializer<CqlTimestamp>
    {
        public override ColumnTypeCode CqlType => ColumnTypeCode.Timestamp;

        internal static CqlTimestamp Deserialize(byte[] buffer, int offset)
        {
            var milliseconds = BeConverter.ToInt64(buffer, offset);

            return new CqlTimestamp(milliseconds);
        }

        internal static byte[] Serialize(CqlTimestamp value)
        {
            var miliseconds = value.Miliseconds;
            return BeConverter.GetBytes(miliseconds);
        }

        public override CqlTimestamp Deserialize(ushort protocolVersion, byte[] buffer, int offset, int length, IColumnInfo typeInfo)
        {
            return Deserialize(buffer, offset);
        }

        public override byte[] Serialize(ushort protocolVersion, CqlTimestamp value)
        {
            return Serialize(value);
        }
    }
}
