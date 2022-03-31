﻿namespace traffic_light_simulation.classes.enums
{
    public enum ServerEventStates
    {
        CONNECT_CONTROLLER,
        SET_AUTOMOBILE_ROUTE_STATE,
        SET_CYCLIST_ROUTE_STATE,
        SET_PEDESTRIAN_ROUTE_STATE,
        SET_BOAT_ROUTE_STATE,
        SET_BRIDGE_WARNING_STATE,
        REQUEST_BRIDGE_STATE,
        REQUEST_BARRIERS_STATE,
        REQUEST_BRIDGE_ROAD_EMPTY,
        REQUEST_BRIDGE_WATER_EMPTY,
        ACKNOWLEDGE_BRIDGE_STATE,
        ERROR_UNKNOWN_EVENT_TYPE,
        ERROR_MALFORMED_MESSAGE,
        ERROR_INVALID_STATE,
        ERROR_NOT_PARSEABLE,
        SESSION_START 
    }
}