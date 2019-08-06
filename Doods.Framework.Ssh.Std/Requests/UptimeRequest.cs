
using System;
using System.Collections.Generic;
using System.Globalization;
using Doods.Framework.Ssh.Std.Converters;
using Doods.Framework.Ssh.Std.Requests.YetRequest;
using Doods.Framework.Ssh.Std.Serializers;

namespace Doods.Framework.Ssh.Std.Requests
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>
    /// last -a | head -3
    /// root pts/0        Sun Sep 24 10:01   still logged in    desktop-fpk3rti
    /// reboot   system boot  Sat Sep 23 20:54 - 10:14  (13:20)     4.9.0-0.bpo.3-amd64
    /// root     pts/0        Tue Sep 12 10:42 - 17:34  (06:52)     desktop-fpk3rti
    /// </example>
    public class LastLoginsRequest : SshRequestBase
    {
        public const string RequestString = "cat /proc/uptime";
        public LastLoginsRequest() : base(RequestString)
        {
        }

        public LastLoginsRequest(SshSerializerSettings settings) : base(RequestString, settings)
        {
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <example>
    ///  cat /proc/uptime
    ///  1106378.63 4430593.31
    /// </example>
    public class UptimeRequest : SshRequestBase
    {

        private class UptimeRequestConverter : ISshConverter
        {
           

          
            public bool CanConvert(Type objectType)
            {
                if (objectType == typeof(string)) return true;
                if (objectType == typeof(double)) return true;
                if (objectType == typeof(TimeSpan)) return true;
                if (objectType == typeof(DateTime)) return true;

                return false;
            }

            public object Read(string reader, Type objectType)
            {
                var result = FormatUptime(reader);
                if (objectType == typeof(double)) return result;

                var timeSpan= TimeSpan.FromSeconds(result);
                if (objectType == typeof(TimeSpan))
                    return timeSpan;

                return timeSpan.ToString(string.Empty);
            }


            private double FormatUptime(string output)
            {
                var lines = output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var split = line.Split(' ');
                    if (split.Length == 2)
                    {
                        try
                        {
                            return double.Parse(split[0], CultureInfo.InvariantCulture);
                        }
                        catch (FormatException)
                        {
                           
                        }
                    }
                    else
                    {
                       
                    }
                }
              
                return 0D;
            }




        }
        public const string RequestString = "cat /proc/uptime";
        public UptimeRequest() : base(RequestString)
        {
            _SshSerializer = new SshSerializer(new SshSerializerSettings() { Converters = new List<ISshConverter>() { new UptimeRequestConverter() } });
        }


    }
}