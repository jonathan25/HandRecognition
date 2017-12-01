﻿using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using HandRecognition.Core.NetworkModels;

namespace HandRecognition.Core.Helpers
{
	public static class ExportHelper
	{
		public static void ExportNetwork(Network network, string path)
		{
			var dn = GetHelperNetwork(network);

            using (var file = File.CreateText(path))
            {
                var serializer = new JsonSerializer { Formatting = Formatting.Indented };
                serializer.Serialize(file, dn);
            }
        }

		private static HelperNetwork GetHelperNetwork(Network network)
		{
			var hn = new HelperNetwork
			{
				LearnRate = network.LearnRate,
				Momentum = network.Momentum
			};

			//Input Layer
			foreach (var n in network.InputLayer)
			{
				var neuron = new HelperNeuron
				{
					Id = n.Id,
					Bias = n.Bias,
					BiasDelta = n.BiasDelta,
					Gradient = n.Gradient,
					Value = n.Value
				};

				hn.InputLayer.Add(neuron);

				foreach (var synapse in n.OutputSynapses)
				{
					var syn = new HelperSynapse
					{
						Id = synapse.Id,
						OutputNeuronId = synapse.OutputNeuron.Id,
						InputNeuronId = synapse.InputNeuron.Id,
						Weight = synapse.Weight,
						WeightDelta = synapse.WeightDelta
					};

					hn.Synapses.Add(syn);
				}
			}

			//Hidden Layer
			foreach (var l in network.HiddenLayers)
			{
				var layer = new List<HelperNeuron>();

				foreach (var n in l)
				{
					var neuron = new HelperNeuron
					{
						Id = n.Id,
						Bias = n.Bias,
						BiasDelta = n.BiasDelta,
						Gradient = n.Gradient,
						Value = n.Value
					};

					layer.Add(neuron);

					foreach (var synapse in n.OutputSynapses)
					{
						var syn = new HelperSynapse
						{
							Id = synapse.Id,
							OutputNeuronId = synapse.OutputNeuron.Id,
							InputNeuronId = synapse.InputNeuron.Id,
							Weight = synapse.Weight,
							WeightDelta = synapse.WeightDelta
						};

						hn.Synapses.Add(syn);
					}
				}

				hn.HiddenLayers.Add(layer);
			}

			//Output Layer
			foreach (var n in network.OutputLayer)
			{
				var neuron = new HelperNeuron
				{
					Id = n.Id,
					Bias = n.Bias,
					BiasDelta = n.BiasDelta,
					Gradient = n.Gradient,
					Value = n.Value
				};

				hn.OutputLayer.Add(neuron);

				foreach (var synapse in n.OutputSynapses)
				{
					var syn = new HelperSynapse
					{
						Id = synapse.Id,
						OutputNeuronId = synapse.OutputNeuron.Id,
						InputNeuronId = synapse.InputNeuron.Id,
						Weight = synapse.Weight,
						WeightDelta = synapse.WeightDelta
					};

					hn.Synapses.Add(syn);
				}
			}

			return hn;
		}
	}
}