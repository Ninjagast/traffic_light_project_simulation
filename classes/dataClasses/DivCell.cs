﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace traffic_light_simulation.classes.dataClasses
{
    public class DivCell
    {
        private int _tr = -1;
        private int _tl = -1;
        private int _br = -1;
        private int _bl = -1;


        public void ClaimCell(int id, string direction)
        {
            SetDivCell(direction, id);
        }

        public bool IsCellFree(string direction)
        {
            switch (direction)
            {
                case "RIGHT":
                    return _br == -1;
                case "LEFT":
                    return _tl == -1;
                case "DOWN":
                    return _bl == -1;
                case "UP":
                    return _tr == -1;
                
                default:
                    throw new Exception($"{direction} does not exist");
            }
        }

        public void UnClaimCell(string direction)
        {
            SetDivCell(direction, -1);
        }

        public int GetCellId(string direction)
        {
            switch (direction)
            {
                case "RIGHT":
                    return _br;
                case "LEFT":
                    return _tl;
                case "DOWN":
                    return _bl;
                case "UP":
                    return _tr;
                
                default:
                    return -1;
            }
        }

        public List<Vector2> GetClaimedCells()
        {
            List<Vector2> returnList = new List<Vector2>();

            if (_br > -1)
            {
                returnList.Add(new Vector2(50,0));
            }            
            if (_bl > -1)
            {
                returnList.Add(new Vector2(23,13));
            }            
            if (_tr > -1)
            {
                returnList.Add(new Vector2(22,-10));
            }            
            if (_tl > -1)
            {
                returnList.Add(Vector2.Zero);
            }

            return returnList;
        }
        
        private void SetDivCell(string direction, int value)
        {
            switch (direction)
            {
                case "RIGHT":
                    _br = value;
                    break;
                case "LEFT":
                    _tl = value;
                    break;
                case "DOWN":
                    _bl = value;
                    break;
                case "UP":
                    _tr = value;
                    break;
            }
        }
    }
}