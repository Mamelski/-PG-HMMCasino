namespace HMMCasino
{
    using System.Collections.Generic;

    public class Matrix<TKey1,TKey2>
    {
        private readonly Dictionary<TKey1, Dictionary<TKey2, double>> innerDictionary;

        public Matrix(ICollection<TKey1> key1space, ICollection<TKey2> key2space)
        {
            this.innerDictionary = new Dictionary<TKey1, Dictionary<TKey2, double>>();
            foreach (var k1 in key1space)
            {
                this.innerDictionary[k1] = new Dictionary<TKey2, double>(key2space.Count);
            }
        }

        public double this[TKey1 key1, TKey2 key2]
        {
            get
            {
                return this.innerDictionary[key1][key2];

            }
            set
            {
                this.innerDictionary[key1][key2] = value;

            }
        }
    }
}
