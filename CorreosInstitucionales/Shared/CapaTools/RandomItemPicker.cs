using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static CorreosInstitucionales.Shared.Utils.WebUtils;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CorreosInstitucionales.Shared.CapaTools
{
    public class RandomItemPicker<T> where T : IConvertible
    {
        Random _rnd;
        long _total;
        List<RandomItem<T>> _items;

        public RandomItemPicker(string filename)
        {
            _rnd = new Random();
            _total = 0;
            _items= new ();

            string[] lines = File.ReadAllLines(filename);
            string[] data;
            int chance;

            foreach (string line in lines)
            {
                data = line.Split('\t');
                chance = int.Parse(data[0]);

                _total += chance;

                _items.Add(new RandomItem<T>(chance, data[1]));
            }
        }

        public RandomItemPicker(IEnumerable<RandomItem<T>> items)
        {
            _rnd = new Random();
            _total = items.Sum(i=>i.Chance);
            _items = items.ToList();
        }

        public RandomItemPicker(IEnumerable<T> items)
        {
            _rnd = new Random();
            _total = items.Count();
            _items = new();

            foreach (T item in items)
            {
                _items.Add(new RandomItem<T>(1, item));
            }
        }

        public T GetRandomItem()
        {
            long i = (long)((_rnd.NextDouble() * _total));
            long sum = 0;

            foreach (RandomItem<T> item in _items)
            {
                sum += item.Chance;

                if (sum >= i)
                {
                    return item.Value;
                }
            }

            return _items.Last().Value;
        }
    }
}
