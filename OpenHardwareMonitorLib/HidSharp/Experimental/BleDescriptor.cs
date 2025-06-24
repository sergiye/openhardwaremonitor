namespace HidSharp.Experimental
{
    public abstract class BleDescriptor
    {
        public override string ToString()
        {
            return Uuid.ToString();
        }

        public abstract BleUuid Uuid
        {
            get;
        }
    }
}
