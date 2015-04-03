using System.Collections.Generic;
using System.Linq;

namespace scrypt
{
    public class Item
    {
        public int Index { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Index, Value);
        }
    }

    public class ListOfItems
    {
        private IEnumerable<Item> _commands;
        private bool? _debug;
        private Dictionary<string, string> _values;
        private bool? _verbose;

        public bool Debug
        {
            get { return _debug ?? (_debug = _commands.Any(x => x.Value == "/debug")).Value; }
        }

        public bool Verbose
        {
            get { return _verbose ?? (_verbose = _commands.Any(x => x.Value == "/v" || x.Value == "/verbose")).Value; }
        }

        public ListOfItems(string[] args)
        {
            var items = args.ToItems().ToList();

            _commands = items.Where(x => x.Value.IndexOf("//") == -1);
            //_values = _commands.ToDictionary(x => x.Value, ;
        }
    }
}