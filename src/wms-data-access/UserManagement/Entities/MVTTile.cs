using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace wmsDataAccess.UserManagement.Entities
{
    public class MVTTile
    {
        private IMemoryCache cache;
        private const double EPSLN = 1.0e-10;
        private const double D2R = Math.PI / 180;
        private const double R2D = 180 / Math.PI;
        // 900913 properties.
        private const double A = 6378137.0;
        private const double MAXEXTENT = 20037508.342789244;
        private int size;

        public object Bc { get; }
        public object Cc { get; }

        private object zc;

        public object Ac { get; }

        public MVTTile(int? size, IMemoryCache cache)
        {
            this.cache = cache;
            this.size = (size!=null) ? (int)size : 256;
            var c = (cacheEntry)this.cache.Get(this.size);
            if (c == null)
            {
                c = new cacheEntry(); 
                
                for (var d = 0; d < 30; d++)
                {
                    c.Bc.Add(this.size / 360);
                    c.Cc.Add(this.size / (2 * Math.PI));
                    c.zc.Add(this.size / 2);
                    c.Ac.Add(this.size);
                    size *= 2;
                }
                this.cache.Set(this.size, c);
            }
            this.Bc = c.Bc;
            this.Cc = c.Cc;
            this.zc = c.zc;
            this.Ac = c.Ac;
        }

        private coord px(coord ll, int zoom)
        {
            //if (isFloat(zoom))
            //{
                var size = this.size * Math.Pow(2, zoom);
                var d = size / 2;
                var bc = (size / 360);
                var cc = (size / (2 * Math.PI));
                var ac = size;
                var f = Math.Min(Math.Max(Math.Sin(D2R * ll.y), -0.9999), 0.9999);
                var x = d + ll.x * bc;
                var y = d + 0.5 * Math.Log((1 + f) / (1 - f)) * -cc;
                if(x > ac) x = ac;
                if(y > ac) y = ac;
                //(x < 0) && (x = 0);
                //(y < 0) && (y = 0);
                return new coord() { x = x, y = y };
            //}
            //else
            //{
            //    var d = this.zc[zoom];
            //    var f = Math.min(Math.max(Math.sin(D2R * ll[1]), -0.9999), 0.9999);
            //    var x = Math.round(d + ll[0] * this.Bc[zoom]);
            //    var y = Math.round(d + 0.5 * Math.log((1 + f) / (1 - f)) * (-this.Cc[zoom]));
            //    (x > this.Ac[zoom]) && (x = this.Ac[zoom]);
            //    (y > this.Ac[zoom]) && (y = this.Ac[zoom]);
            //    //(x < 0) && (x = 0);
            //    //(y < 0) && (y = 0);
            //    return [x, y];
            //}
        }

        private coord Ll(coord px, int zoom)
        {
            //if (isFloat(zoom))
            //{
            var size = this.size * Math.Pow(2, zoom);
            var bc = (size / 360);
            var cc = (size / (2 * Math.PI));
            var zc = size / 2;
            var g = (px.y - zc) / -cc;
            var _lon = (px.x - zc) / bc;
            var _lat = R2D * (2 * Math.Atan(Math.Exp(g)) - 0.5 * Math.PI);
            return new coord() { x = _lat, y = _lon };
            //}
            //else
            //{
            //    var g = (px[1] - this.zc[zoom]) / (-this.Cc[zoom]);
            //    var lon = (px[0] - this.zc[zoom]) / this.Bc[zoom];
            //    var lat = R2D * (2 * Math.atan(Math.exp(g)) - 0.5 * Math.PI);
            //    return [lon, lat];
            //}
        }

        public bbox bbox(double x, double y, int zoom, bool tms_style, string srs) {
            // Convert xyz into bbox with srs WGS84
            if (tms_style)
            {
                y = (Math.Pow(2, zoom) - 1) - y;
            }
            // Use +y to make sure it's a number to avoid inadvertent concatenation.
            var ll = new coord() { x = x * this.size, y = (+y + 1) * this.size }; // lower left
            // Use +x to make sure it's a number to avoid inadvertent concatenation.
            var ur = new coord() { x = (+x + 1) * this.size, y = y * this.size }; // upper right
            var bbox = new bbox() { ll = this.Ll(ll, zoom), ur = (this.Ll(ur, zoom)) };

            // If web mercator requested reproject to 900913.
            //if (srs == "900913")
            //{
            //    return this.convert(bbox, "900913");
            //}
            //else
            //{
            //    return bbox;
            //}
            return bbox;
        }
    }

    public class coord {
        public double x;
        public double y;
    }

    public class bbox {
        public coord ll;
        public coord ur;
    }

    internal class cacheEntry {
        public List<double> Bc { get; set; }
        public List<double> Cc { get; set; }
        public List<double> zc { get; set; }
        public List<double> Ac { get; set; }

        public cacheEntry()
        {
            Bc = new List<double>();
            Cc = new List<double>();
            zc = new List<double>();
            Ac = new List<double>();
        }
    }
}
