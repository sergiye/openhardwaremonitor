using System.Collections.Generic;

namespace HidSharp.Reports
{
    public class IndexRange : Indexes
    {
        public IndexRange()
        {

        }

        public IndexRange(uint minimum, uint maximum)
        {
            Minimum = minimum; Maximum = maximum;
        }

        public override bool TryGetIndexFromValue(uint value, out int index)
        {
            if (value >= Minimum && value <= Maximum)
            {
                index = (int)(value - Minimum); return true;
            }

            return base.TryGetIndexFromValue(value, out index);
        }

        public override IEnumerable<uint> GetValuesFromIndex(int index)
        {
            if (index < 0 || index >= Count) { yield break; }
            yield return (uint)(Minimum + index);
        }

        public override int Count
        {
            get { return (int)(Maximum - Minimum + 1); }
        }

        public uint Minimum
        {
            get;
            set;
        }

        public uint Maximum
        {
            get;
            set;
        }
    }
}
