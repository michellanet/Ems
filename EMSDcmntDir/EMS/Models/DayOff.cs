namespace EMS.Models
{
    public class DayOff
    {
        public enum DayOffStatus
        {
            Pending, Granted, Refused
        }
        public int id { get; set; }
        public System.DateTime date { get; set; }
        public DayOffStatus status { get; set; }
        public int employeeId { get; set; }

        public virtual Employee Employee { get; set; }
    }
}