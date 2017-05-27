using System;
using System.Text;

namespace neuroLab_9
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Stuff.nu = 0.0001;
			Clusterizer clusty = new Clusterizer();

			clusty.LoadData("moscow.json", "dataset.json");
			clusty.Start();
		}
	}
}
