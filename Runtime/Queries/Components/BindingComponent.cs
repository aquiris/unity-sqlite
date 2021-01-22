namespace Aquiris.SQLite.Queries.Components
{
    internal readonly struct BindingComponent : IQueryComponent
    {
        private static int _bindingIndex = default;
        
        public string value { get; }

        private BindingComponent(string value)
        {
            this.value = value;
        }
        
        public static BindingComponent Binding()
        {
            BindingComponent binding = new BindingComponent($"@binding{_bindingIndex}");
            if (_bindingIndex + 1 == int.MaxValue) _bindingIndex = -1;
            _bindingIndex += 1;
            return binding;
        }
    }
}