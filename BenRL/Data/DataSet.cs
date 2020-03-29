using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenRL.Data
{
    public class DataSet
    {
        Dictionary<string, List<object>> _data;

        public DataSet()
        {
            _data = new Dictionary<string, List<object>>();
        }

        public void AddValue(string label, object value)
        {
            if (!_data.ContainsKey(label))
            {
                _data.Add(label, new List<object>());
            }
            _data[label].Add(value);
        }

        public void AddValues(string label, object[] values)
        {
            if (!_data.ContainsKey(label))
            {
                _data.Add(label, new List<object>());
            }
            _data[label].AddRange(values);
        }

        public void RemoveValue(string label, int index)
        {
            if (_data.ContainsKey(label))
            {
                _data[label].RemoveAt(index);
            }
        }

        public int GetLabelSize(string label)
        {
            if (_data.ContainsKey(label))
            {
                return _data[label].Count;
            }
            return 0;
        }

        public object[] GetLabelValues(string label)
        {
            if (_data.ContainsKey(label))
            {
                return _data[label].ToArray();
            }
            return new object[] { };
        }

        public void SetLabel(string label, object[] values)
        {
            if (!_data.ContainsKey(label))
            {
                _data.Add(label, values.ToList());
            }
            else
            {
                _data[label] = values.ToList();
            }
        }

        public void RemoveLabel(string label)
        {
            if (_data.ContainsKey(label))
            {
                _data.Remove(label);
            }
        }

        public Tensor ToTensor(params string[] labels)
        {
            int valueLength = labels.Max(s => GetLabelSize(s));
            Tensor result = new Tensor(labels.Length, valueLength);
            for(int l = 0; l < labels.Length; l++)
            {
                int labelSize = GetLabelSize(labels[l]);
                for (int i = 0; i < labelSize; i++)
                {
                    object value = GetLabelValues(labels[l])[i];
                    if (!typeof(double).IsAssignableFrom(value.GetType()))
                        throw new Exception("Invalid type '" + value.GetType().ToString() + "' in label '" + labels[l] + "'.");
                    result[l, i] = (double)value;
                }
            }
            return result;
        }
    }
}
