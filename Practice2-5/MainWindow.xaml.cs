using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Practice2
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Random rand = new Random();

		public MainWindow()
		{
			InitializeComponent();
			Program();
		}

		public void Program()
		{
			int steps = 200000;
			int trial = 200;
			int sample = 18;
			var min = -7.0;
			var max = 2.0;

			var plotX = Enumerable.Range(0, sample)
				.Select(p => min + (max - min) / sample * p)
				.ToArray();
			var values = plotX.Select(p => Math.Pow(2.0, p)).ToArray();

			var greedyRes = new double[sample];
			var epsGreedyRes = new double[sample];
			var ucbRes = new double[sample];
			var gradientRes = new double[sample];
			for (int i = 0; i < sample; i++)
			{
				var v = values[i];

				var greedy = new Greedy(v);
				var (rgreedy, _) = greedy.GetRates(trial, steps, 10);
				greedyRes[i] = rgreedy.TakeLast(steps / 2).Average();

				var eps = new EpsilonGreedy(v);
				var (reps, _) = eps.GetRates(trial, steps, 10);
				epsGreedyRes[i] = reps.TakeLast(steps / 2).Average();

				var ucb = new UCB(v);
				var (rucb, _) = ucb.GetRates(trial, steps, 10);
				ucbRes[i] = rucb.TakeLast(steps / 2).Average(); 

				var grad = new GradientBandit(v);
				var (rgrad, _) = grad.GetRates(trial, steps, 10);
				gradientRes[i] = rgrad.TakeLast(steps / 2).Average();
			}

			Plot01.Plot.AddScatter(plotX, greedyRes, System.Drawing.Color.Blue);
			Plot01.Plot.AddScatter(plotX, epsGreedyRes, System.Drawing.Color.Orange);
			Plot01.Plot.AddScatter(plotX, ucbRes, System.Drawing.Color.Cyan);
			Plot01.Plot.AddScatter(plotX, gradientRes, System.Drawing.Color.YellowGreen);

			// Plot01.Plot.SetAxisLimitsY(0.9, 1.6);

			Plot01.Refresh();
			Plot01.Plot.Resize(1200, 800);
			Plot01.Plot.SaveFig($"result_{DateTime.Now.ToString("yyyyMMdd_hhmmss")}.png");
		}
	}
}
