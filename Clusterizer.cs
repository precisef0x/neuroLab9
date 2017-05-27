using System;
using Newtonsoft.Json;

namespace neuroLab_9
{

	public class Clusterizer
	{
		public System.Collections.Generic.List<District> clusters;
		public System.Collections.Generic.List<Parking> dataset;

		public void LoadData(string districtsPath, string datasetPath)
		{
			string districtsJson = System.IO.File.ReadAllText(districtsPath);
			string datasetJson = System.IO.File.ReadAllText(datasetPath);
			clusters = JsonConvert.DeserializeObject<System.Collections.Generic.List<District>>(districtsJson);
			dataset = JsonConvert.DeserializeObject<System.Collections.Generic.List<Parking>>(datasetJson);
		}

		public void CalculateAssignment()
		{
			ClearClusterNodes();
			foreach (Parking parking in dataset)
			{
				int targetCluster = 0;
				double minDistance = Stuff.Distance(parking, clusters[0]);

				for (int i = 0; i < clusters.Count; i++)
				{
					double dist = Stuff.Distance(parking, clusters[i]);
					if (dist < minDistance)
					{
						minDistance = dist;
						targetCluster = i;
					}
				}

				clusters[targetCluster].nodes.Add(parking);
				parking.cluster = targetCluster;
			}
		}

		public void ClearClusterNodes()
		{
			foreach (District districs in clusters)
				districs.nodes.Clear();
		}

		public void UpdateOrigClusters()
		{
			foreach (Parking parking in dataset)
				parking.originalCluster = parking.cluster;
		}

		public int CalculateMassCenter()
		{
			int errors = 0;

			foreach (District district in clusters)
			{
				if (district.nodes.Count < 1)
				{
					Console.WriteLine("District {0}: center [{1}; {2}] not changed; has 0 nodes", district.name, Math.Round(district.x, 4), Math.Round(district.y, 4));
					continue;
				}
				double newX = 0;
				double newY = 0;
				for (int i = 0; i < district.nodes.Count; i++)
				{
					newX += district.nodes[i].Latitude_WGS84;
					newY += district.nodes[i].Longitude_WGS84;
				}
				newX /= district.nodes.Count;
				newY /= district.nodes.Count;

				Console.WriteLine("District {4}: center [{0}; {1}] --> [{2}; {3}] has {5} nodes", Math.Round(district.x, 4), Math.Round(district.y, 4), Math.Round(newX, 4), Math.Round(newY, 4), district.name, district.nodes.Count);
				if (Math.Abs(newX - district.x) > Stuff.nu) //X coord changed
				{
					errors++;
					district.x = newX;
				}
				if (Math.Abs(newY - district.y) > Stuff.nu) //Y coord changed
				{
					errors++;
					district.y = newY;
				}
			}
			return errors;
		}

		public void Start()
		{
			int counter = 0;
			do
			{
				CalculateAssignment();
				if (counter == 0)
					UpdateOrigClusters();
				Console.WriteLine("Epoch {0}", counter);
				counter++;
			}
			while (CalculateMassCenter() != 0);

			double errors = 0;
			foreach (Parking parking in dataset)
				if (parking.cluster != parking.originalCluster)
				{
				Console.WriteLine("Parking {0} cluster: {1} --> {2}", parking.global_id, parking.originalCluster, parking.cluster);
					errors++;
				}

			double eeerrors = errors / dataset.Count;

			Console.WriteLine("\nResults: {0}% errors", eeerrors*100.0);
		}
	}
}
