namespace SchedBus.Model
{
    internal class Plan
    {
        // String variable to store location's destination of a Plan
        public string Destination { get; set; } = string.Empty;
        // String variable to store Plan's name
        public string Label { get; set; } = string.Empty;
        // Integer variable to store the maximum number of requested trip
        public int MaxRequestedTrip { get; set; }
        // Bool variable to store notification switch
        public bool Notification {  get; set; }
        // Bool variable to store viberation switch
        public bool Viberation { get; set; }
    }
}
