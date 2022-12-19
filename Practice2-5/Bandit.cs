using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice2
{
	public class Bandit
	{
		internal Random rand = new Random();

		internal double mean0 = 0.0;
		internal double sd0 = 1.0;
		internal double sdTrue = 1.0;
		internal double dMean = 0.0;
		internal double dSd = 0.01;

		internal double eps = 0.1;
		internal double alpha = 0.1;
		internal double c = 5.0;
		internal double q0 = 0.0;

		internal double[] qTrue;
		internal double[] q;
		internal int[] n;

		public Bandit()
		{

		}

		public (double[] rewards, int[] succeeds) Run(int steps, int k)
		{
			qTrue = Enumerable.Repeat(0, k).Select(p => GetNormalDist(mean0, sd0)).ToArray();
			q = Enumerable.Repeat(q0, k).ToArray();
			n = Enumerable.Repeat(0, k).ToArray();

			var rewards = new double[steps];
			var succeeds = new int[steps];

			var lastRewardMean = 0.0;
			var succeed = 0;
			for (int i = 0; i < steps; i++)
			{
				var a = SelectAction(q, i + 1);

				if (a == -1)
				{
					throw new IndexOutOfRangeException("a must not be -1.");
				}
				var reward = GetNormalDist(qTrue[a], sdTrue);
				n[a]++;

				lastRewardMean = lastRewardMean + (reward - lastRewardMean) / (i + 1);
				rewards[i] = reward;

				if (ArgMax(qTrue) == a) succeed++;
				succeeds[i] = succeed;

				q = UpdateInternal(q, a, reward, lastRewardMean);
				(qTrue, sdTrue) = UpdateExternal(qTrue, sdTrue);
			}
			return (rewards, succeeds);
		}

		public virtual int SelectAction(double[] q, int t)
		{
			var r = rand.NextDouble();
			return 0;
		}

		public virtual double[] UpdateInternal(double[] q, int a, double r, double b)
		{
			return q;
		}

		public virtual (double[] qTrue, double sdTrue) UpdateExternal(double[] qTrue, double sdTrue)
		{
			//return (qTrue, sdTrue);
			for (int i = 0; i < qTrue.Length; i++)
			{
				qTrue[i] += GetNormalDist(dMean, dSd);
			}
			return (qTrue, sdTrue);
		}

		public (double[] rewardRates, double[] succeedRates) GetRates(int trial, int steps, int k)
		{
			var rewardRates = new double[steps];
			var succeedRates = new double[steps];
			for (int i = 0; i < trial; i++)
			{
				var (reward, succeed) = Run(steps, k);
				for (int j = 0; j < steps; j++)
				{
					rewardRates[j] += reward[j];
					succeedRates[j] += succeed[j] / (j + 1.0);
				}
			}
			for (int i = 0; i < steps; i++)
			{
				rewardRates[i] /= trial;
				succeedRates[i] /= trial;
			}
			return (rewardRates, succeedRates);
		}

		public double GetNormalDist(double mean, double sd)
		{
			var (x, _) = GetNormalDist2(mean, sd);
			return x;
		}

		public (double x1, double x2) GetNormalDist2(double mean, double sd)
		{
			var (u1, u2) = (rand.NextDouble(), rand.NextDouble());
			var (z1, z2) =
				(Math.Sqrt(-2 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2),
				Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2));
			return (mean + sd * z1, mean + sd * z2);
		}

		public int ArgMax(double[] x)
		{
			var res = new List<int>();
			var max = double.MinValue;
			for (int i = 0; i < x.Length; i++)
			{
				if (max < x[i])
				{
					max = x[i];
					res = new List<int> { i };
				}
				else if (max == x[i])
				{
					res.Add(i);
				}
			}
			return res[rand.Next(0, res.Count)];
		}
	}
}
