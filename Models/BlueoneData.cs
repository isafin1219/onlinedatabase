using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace TestConsoleApp1
{

    public class BlueoneData
    {
        private string webservice = "http://blueone.swh.az/azqops/webservices/flights/flights.php";

        private XNamespace XmlNs = "http://www.blueonesoftware.com/webservices/Flights";

        private XDocument XmlData(string opsdate)
        {
            WebRequest webRequest = WebRequest.Create(webservice);
            HttpWebRequest httpRequest = (HttpWebRequest)webRequest;
            httpRequest.Method = "POST";
            httpRequest.ContentType = "text/xml; charset=utf-8";
            httpRequest.Headers.Add("SOAPAction: weblink");
            httpRequest.ProtocolVersion = HttpVersion.Version11;
            httpRequest.Credentials = CredentialCache.DefaultCredentials;
            #region XML request
            string request = @"<x:Envelope xmlns:x=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:fli=""http://www.blueonesoftware.com/webservices/Flights"">
                <x:Header/>
                <x:Body>
                    <fli:FlightsRequest>
                        <fli:username>flightsweb</fli:username>
                        <fli:password>Bluo$Flt7</fli:password>
                        <fli:std_start>" + opsdate + @"T00:00:00</fli:std_start>
                        <fli:std_end>" + opsdate + @"T23:59:00</fli:std_end>
                        <fli:flight>
                            <fli:carrier_code>7L</fli:carrier_code>				            
                            <fli:show_crew_compo>Y</fli:show_crew_compo>
                            <fli:show_fuel>Y</fli:show_fuel>
                            <fli:show_delay_codes>Y</fli:show_delay_codes>
                            <fli:show_cancelled_flights>N</fli:show_cancelled_flights>
                            <fli:show_pending_flights>N</fli:show_pending_flights>
                        </fli:flight>
                    </fli:FlightsRequest>
                </x:Body>
            </x:Envelope>";
            #endregion
            Stream requestStream = httpRequest.GetRequestStream();
            //Create Stream and Complete Request             
            StreamWriter streamWriter = new StreamWriter(requestStream/*, Encoding.UTF8*/);
            streamWriter.Write(request);
            streamWriter.Close();
            //Get the Response    
            HttpWebResponse wr = (HttpWebResponse)httpRequest.GetResponse();
            StreamReader srd = new StreamReader(wr.GetResponseStream());
            string resulXmlFromWebService = srd.ReadToEnd();
            return XDocument.Parse(resulXmlFromWebService);
        }

        public FlightsResponse FlightInfo(string opsdate)
        {
            var response = XmlData(opsdate).Descendants(XmlNs + "FlightsResponse").FirstOrDefault();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(FlightsResponse));
            return (FlightsResponse)xmlSerializer.Deserialize(new StringReader(response.ToString()));
        }
    }

    [Serializable, XmlRoot("FlightsResponse", Namespace = "http://www.blueonesoftware.com/webservices/Flights")]
    public class FlightsResponse
    {
        [XmlElement("success")]
        public string Success { get; set; }

        [XmlElement("flight")]
        public List<FlightData> Flights { get; set; }
    }

    [Serializable]
    public class FlightData
    {
        [XmlElement("uid")]
        public string Id { get; set; }

        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("operational_status")]
        public string Opsstatus { get; set; }

        [XmlElement("call_sign_number")]
        public string Call_sign_number { get; set; }

        [XmlElement("call_sign_code")]
        public string Call_sign_code { get; set; }

        [XmlElement("aircraft_reg")]
        public string Aircraft_reg { get; set; }

        [XmlElement("aircraft_type")]
        public string Aircraft_type { get; set; }

        [XmlElement("std")]
        public string Std { get; set; }

        [XmlElement("sta")]
        public string Sta { get; set; }

        [XmlElement("est_db")]
        public string Est_blocktime { get; set; }

        [XmlElement("est_dt")]
        public string Est_takeofftime { get; set; }

        [XmlElement("est_al")]
        public string Est_touchdowntime { get; set; }

        [XmlElement("est_ab")]
        public string Est_blockintime { get; set; }

        [XmlElement("mvt_db")]
        public string Mvt_blocktime { get; set; }      

        [XmlElement("mvt_dt")]
        public string Mvt_takeofftime { get; set; }

        [XmlElement("mvt_al")]
        public string Mvt_touchdowntime { get; set; }

        [XmlElement("mvt_ab")]
        public string Mvt_blockintime { get; set; }

        [XmlElement("acars_est_al")]
        public string Acars_est_blockintime { get; set; }

        [XmlElement("acars_db")]
        public string Acars_blocktime { get; set; }

        [XmlElement("acars_dt")]
        public string Acars_takeofftime { get; set; }

        [XmlElement("acars_al")]
        public string Acars_touchdowntime { get; set; }

        [XmlElement("acars_ab")]
        public string Acars_blockintime { get; set; }

        [XmlElement("revised_departure")]
        public string Revised_departure { get; set; }

        [XmlElement("revised_arrival")]
        public string Revised_arrival { get; set; }

        [XmlElement("remarks")]
        public string Remarks { get; set; }

        [XmlElement("apt_dep")]
        public AirportData Apt_dep { get; set; }

        [XmlElement("apt_arr_planned")]
        public AirportData Apt_arr_planned { get; set; }

        [XmlElement("apt_arr_actual")]
        public AirportData Apt_arr_actual { get; set; }

        [XmlElement("fuel")]
        public FuelInfo Fuel { get; set; }

        [XmlElement("crew_compo")]
        public CrewInfo Crew_compo { get; set; }

        [XmlElement("delays")]
        public DelayList Delays { get; set; }

        [XmlElement("modified_at")]
        public string Modified_at { get; set; }
    }

    [Serializable]
    public class AirportData
    {
        [XmlElement("iata")]
        public string Iata { get; set; }

        [XmlElement("icao")]
        public string Icao { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("time_offset")]
        public string Time_offset { get; set; }        
    }

    [Serializable]
    public class FuelInfo
    {
        [XmlElement("amount")]
        public FuelAmount Amount { get; set; }
    }

    [Serializable]
    public class FuelAmount
    {
        [XmlElement("ofp_required")]
        public string Ofp_required { get; set; }

        [XmlElement("off_block")]
        public string Off_block { get; set; }

        [XmlElement("taxi")]
        public string Taxi { get; set; }

        [XmlElement("takeoff_planned")]
        public string Takeoff_planned { get; set; }

        [XmlElement("takeoff_actual")]
        public string Takeoff_actual { get; set; }

        [XmlElement("trip_planned")]
        public string Trip_planned { get; set; }

        [XmlElement("landing")]
        public string Landing { get; set; }

        [XmlElement("ofp_landing")]
        public string Ofp_landing { get; set; }

        [XmlElement("onblock_actual")]
        public string Onblock_actual { get; set; }
    }

    [Serializable]
    public class CrewInfo
    {
        [XmlElement("crew_member")]
        public List<Crewmember> Crew_member { get; set; }
    }

    [Serializable]
    public class Crewmember
    {       
        [XmlElement("alpha_code")]
        public string Alpha_code { get; set; }

        [XmlElement("first_name")]
        public string First_name { get; set; }

        [XmlElement("last_name")]
        public string Last_name { get; set; }

        [XmlElement("group")]
        public string Group { get; set; }

        [XmlElement("function")]
        public string Function { get; set; }

        [XmlElement("position_short")]
        public string Position_short { get; set; }

        [XmlElement("position_long")]
        public string Position_long { get; set; }

        [XmlElement("gender")]
        public string Gender { get; set; }

        [XmlElement("deadhead")]
        public string Deadhead { get; set; }
    }

    [Serializable]
    public class DelayList
    {
        [XmlElement("delay")]
        public List<DelayItem> Delay { get; set; }
    }

    [Serializable]
    public class DelayItem
    {
        [XmlElement("code")]
        public string Code { get; set; }

        [XmlElement("duration")]
        public string Duration { get; set; }
    }
}
