﻿namespace traffic_light_simulation.classes.dataClasses
{
    public class connectData
    {
        public string sessionName { get; set; }
        public int sessionVersion { get; set; }
        public bool discardParseErrors {get; set;}
        public bool discardEventTypeErrors {get; set;}
        public bool discardMalformedDataErrors {get; set;}
        public bool discardInvalidStateErrors {get; set;}

    }
}