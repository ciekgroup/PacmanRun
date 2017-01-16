using UnityEngine;
using System.Collections;

public class GoogleMap : MonoBehaviour
{
	public enum MapType
	{
		RoadMap,
		Satellite,
		Terrain,
		Hybrid
	}
	public bool loadOnStart = true;
	public bool autoLocateCenter = true;
	public GoogleMapLocation centerLocation;
	public int zoom = 13;
	public MapType mapType;
	public int size = 512;
	public bool doubleResolution = false;
	public GoogleMapMarker[] markers;
	public GoogleMapPath[] paths;
    float lat = 41.213775f;
    float lon = -95.946088f;
    //float lat = 41.2778f;
    //float lon = -96.0431f;
    DetectRoads roads;

    public Camera cam;

	public void Refresh()
    {
        if (autoLocateCenter && (markers.Length == 0 && paths.Length == 0))
        {
            Debug.LogError("Auto Center will only work if paths or markers are used.");
        }
        roads = GetComponent<DetectRoads>();
        Debug.Log("Refresh called");
        //StartCoroutine(_LocationData(lat,lon)); For when mobile is available
        StartCoroutine(_Refresh(lat,lon));
        
        
    }
	 
	IEnumerator _Refresh (float loclat, float loclon)
	{
        Debug.Log("_Refresh called");
		var url = "http://maps.googleapis.com/maps/api/staticmap";
		var qs = "";
		if (!autoLocateCenter)
        {
			if (centerLocation.address != "")
				qs += "center=" + WWW.UnEscapeURL(centerLocation.address);
			else
            {
				qs += "center=" + WWW.UnEscapeURL(string.Format ("{0},{1}", loclat, loclon));
			}
		
			qs += "&zoom=" + zoom.ToString ();
		}
		qs += "&size=" + WWW.UnEscapeURL (string.Format ("{0}x{0}", size));
        qs += "&style=element:labels|visibility:off";

        qs += "&scale=" + (doubleResolution ? "2" : "1");
		qs += "&maptype=" + mapType.ToString ().ToLower ();
		var usingSensor = false;
#if UNITY_IPHONE
		usingSensor = Input.location.isEnabledByUser && Input.location.status == LocationServiceStatus.Running;
#endif
		qs += "&sensor=" + (usingSensor ? "true" : "false");
		
		foreach (var i in markers)
        {
			qs += "&markers=" + string.Format ("size:{0}|color:{1}|label:{2}", i.size.ToString ().ToLower (), i.color, i.label);
			foreach (var loc in i.locations)
            {
				if (loc.address != "")
					qs += "|" + WWW.UnEscapeURL (loc.address);
				else
					qs += "|" + WWW.UnEscapeURL (string.Format ("{0},{1}", loc.latitude, loc.longitude));
			}
		}
		
		foreach (var i in paths)
        {
			qs += "&path=" + string.Format ("weight:{0}|color:{1}", i.weight, i.color);
			if(i.fill) qs += "|fillcolor:" + i.fillColor;
			foreach (var loc in i.locations)
            {
				if (loc.address != "")
					qs += "|" + WWW.UnEscapeURL (loc.address);
				else
					qs += "|" + WWW.UnEscapeURL (string.Format ("{0},{1}", loc.latitude, loc.longitude));
			}
		}
        
        Debug.Log(qs);
        var req = new WWW(url + "?" + qs + "&key=AIzaSyAB4gEkccu3Qw4vWSSlilXvgTutxUXGXRI");
        yield return req;
        GetComponent<Renderer>().material.mainTexture = req.texture;
        roads.PointGen();
    }
    IEnumerator _LocationData(float loclat, float loclon)
    {
        Debug.Log("_LocationData called");
        if (!Input.location.isEnabledByUser)
        {
            yield break;
        }
        //start the service deamon with permission from device
        Input.location.Start();
        //Wait unitl out of init stage
        int maxwait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxwait > 0)
        {
            yield return new WaitForSeconds(1);
            maxwait--;
        }
        if (maxwait < 1)
        {
            Debug.Log("maximum wait time exceded");
            yield break;
        }
        //Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine connection failiure.");
            yield break;
        }
        else
        {
            //access granted and using location data
            Debug.Log("Getting Location Data");
        }
        UpdateLocationData(lat, lon);
    }
    void UpdateLocationData(float loclat, float loclon)
    {
        Debug.Log("UpdateLocationData called");
        loclat = Input.location.lastData.latitude;
        loclon = Input.location.lastData.longitude;
        StartCoroutine(_Refresh(loclat, loclon));
    }
}

public enum GoogleMapColor
{
	black,
	brown,
	green,
	purple,
	yellow,
	blue,
	gray,
	orange,
	red,
	white
}

[System.Serializable]
public class GoogleMapLocation
{
	public string address;
	public float latitude;
	public float longitude;
}

[System.Serializable]
public class GoogleMapMarker
{
	public enum GoogleMapMarkerSize
	{
		Tiny,
		Small,
		Mid
	}
	public GoogleMapMarkerSize size;
	public GoogleMapColor color;
	public string label;
	public GoogleMapLocation[] locations;
	
}

[System.Serializable]
public class GoogleMapPath
{
	public int weight = 5;
	public GoogleMapColor color;
	public bool fill = false;
	public GoogleMapColor fillColor;
	public GoogleMapLocation[] locations;	
}