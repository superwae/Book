﻿namespace Lafatkotob.Entities
{
    public class Events
    {
        int id {  get; set; }
        string eventName { get; set; }
        string? description { get; set; }
        DataTime dateScheduled { get; set; }
        string location { get; set; }
        int hostUserID { get; set; }
    }
}
