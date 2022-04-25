using System.Collections;
using System.Collections.Generic;


namespace traffic_light_simulation.classes.dataClasses
{
    public class WeightTable: IEnumerable<KeyValuePair<string,int>>
    {
        private Dictionary<string, int> _rows = new Dictionary<string, int>();
        private int _tableSum = 0;
        
        public WeightTable AddRow(int weight, string name)
        {
            _rows.Add(name, weight);
            _tableSum += weight;
            return this;
        }

        public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
        {
            return _rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int GetTableSum()
        {
            return _tableSum;
        }
    }
}