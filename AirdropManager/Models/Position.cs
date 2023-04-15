using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;

namespace RestoreMonarchy.AirdropManager.Models
{
    public class Position
    {
        public Position() { }
        public Position(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3 ToVector()
        {
            return new Vector3(X, Y, Z);
        }

        [XmlAttribute("X")]
        public float X { get; set; }
        [XmlAttribute("Y")]
        public float Y { get; set; }
        [XmlAttribute("Z")]
        public float Z { get; set; }
    }
}
