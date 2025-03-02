using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    //"fact":{"daytime":"n","obs_time":1740773349,"season":"winter","source":"station","uptime":1740773349,"cloudness":0,"condition":"clear","feels_like":-20,"humidity":82,"icon":"skc_n","is_thunder":false,"polar":false,"prec_prob":0,"prec_strength":0,"prec_type":0,"pressure_mm":774,"pressure_pa":1031,"temp":-13,"uv_index":0,"temp_water":0,"wind_angle":309,"wind_dir":"nw","wind_gust":8.7,"wind_speed":4.5}
    public class WeatherResponse
    {
        public Fact? fact { get; set; }
    }
    public class Fact 
    {
        public string daytime { get; set; } = string.Empty;
        public double obs_time { get; set; }
        public string season { get; set; } = string.Empty;
        public float cloudness { get; set; }
        public string condition { get; set; } = string.Empty;
        public float feels_like { get; set; }
        public float humidity { get; set; }
        public string icon { get; set; } = string.Empty;
        public bool is_thunder { get; set; }
        public float prec_strength { get; set; }
        public float prec_type { get; set; }
        public string temp { get; set; } = string.Empty;
        public string temp_water { get; set; } = string.Empty;
        public string wind_dir { get; set; } = string.Empty;
        public float wind_speed { get; set; }
        public string imageUrl { get; set; } = string.Empty;
    } 
}
