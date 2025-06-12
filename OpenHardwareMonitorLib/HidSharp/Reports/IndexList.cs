using System.Collections.Generic;

namespace HidSharp.Reports
{
    public class IndexList : Indexes
    {
        public IndexList()
        {
            Indices = new List<IList<uint>>();
        }

        public override bool TryGetIndexFromValue(uint value, out int index)
        {
            for (int i = 0; i < Indices.Count; i ++)
            {
                foreach (uint thisValue in Indices[i])
                {
                    if (thisValue == value) { index = i; return true; }
                }
            }

            return base.TryGetIndexFromValue(value, out index);
        }

        public override IEnumerable<uint> GetValuesFromIndex(int index)
        {
            if (index < 0 || index >= Count) { yield break; }
            foreach (uint value in Indices[index]) { yield return value; }
        }

        public override int Count
        {
            get { return Indices.Count; }
        }

        public IList<IList<uint>> Indices
        {
            get;
            private set;
        }
    }
}
