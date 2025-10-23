namespace Director.Extensions.ModeC
{
    internal class ObisEntry
    {
        public string Name { get; set; }
        public string Obis { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanExecute { get; set; }
        public bool ThreePhase { get; set; }
        public string Category { get; set; }
    }
}
