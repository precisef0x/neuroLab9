using System;
namespace neuroLab_9
{
	public class District
	{
		public string name;
		public double x;
		public double y;
		public System.Collections.Generic.List<Parking> nodes;

		public District(string name, double x, double y)
		{
			nodes = new System.Collections.Generic.List<Parking>();
			this.name = name;
			this.x = x;
			this.y = y;
		}
	}

	public class Parking
	{
		public string Name;
		public double Latitude_WGS84;
		public double Longitude_WGS84;
		public string AdmArea;
		public int global_id;
		public int cluster = 0;
		public int originalCluster;

		public Parking(string Name, double Latitude_WGS84, double Longitude_WGS84)
		{
			this.Name = Name;
			this.Latitude_WGS84 = Latitude_WGS84;
			this.Longitude_WGS84 = Longitude_WGS84;
		}
	}

	public static class Stuff
	{
		public static double nu;

		public static double Distance(Parking parking, District district)
		{
			return Math.Sqrt(Math.Pow(parking.Latitude_WGS84 - district.x, 2) + Math.Pow(parking.Longitude_WGS84 - district.y, 2));
		}
	}
}
