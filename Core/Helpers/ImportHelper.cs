using System;
using System.Collections.Generic;
using System.Linq;
using HandRecognition.Core.NetworkModels;
using Newtonsoft.Json;

namespace HandRecognition.Core.Helpers
{
	public static class ImportHelper
	{
        public static Network ImportNetwork(string json, ref double progress, ref ProgressState state)
        {
            var dn = JsonConvert.DeserializeObject<HelperNetwork>(json);
            if (dn == null) return null;

            progress = 0;

            double singleStep = 1.0 / (dn.InputLayer.Count()
                + dn.HiddenLayers.Count()
                + dn.OutputLayer.Count()
                + dn.Synapses.Count());
            
            var network = new Network();
            var allNeurons = new List<Neuron>();

            network.LearnRate = dn.LearnRate;
            network.Momentum = dn.Momentum;

            //Input Layer
            state = ProgressState.InputLayer;
            foreach (var n in dn.InputLayer)
            {
                var neuron = new Neuron
                {
                    Id = n.Id,
                    Bias = n.Bias,
                    BiasDelta = n.BiasDelta,
                    Gradient = n.Gradient,
                    Value = n.Value
                };

                network.InputLayer.Add(neuron);
                allNeurons.Add(neuron);

                progress += singleStep;
            }

            //Hidden Layers
            state = ProgressState.HiddenLayers;
            foreach (var layer in dn.HiddenLayers)
            {
                var neurons = new List<Neuron>();
                foreach (var n in layer)
                {
                    var neuron = new Neuron
                    {
                        Id = n.Id,
                        Bias = n.Bias,
                        BiasDelta = n.BiasDelta,
                        Gradient = n.Gradient,
                        Value = n.Value
                    };

                    neurons.Add(neuron);
                    allNeurons.Add(neuron);
                }
                network.HiddenLayers.Add(neurons);

                progress += singleStep;
            }

            //Output Layer
            state = ProgressState.OutputLayer;
            foreach (var n in dn.OutputLayer)
            {
                var neuron = new Neuron
                {
                    Id = n.Id,
                    Bias = n.Bias,
                    BiasDelta = n.BiasDelta,
                    Gradient = n.Gradient,
                    Value = n.Value
                };

                network.OutputLayer.Add(neuron);
                allNeurons.Add(neuron);

                progress += singleStep;
            }

            //Synapses
            state = ProgressState.Synapses;
            foreach (var syn in dn.Synapses)
            {
                var synapse = new Synapse { Id = syn.Id };
                var inputNeuron = allNeurons.First(x => x.Id == syn.InputNeuronId);
                var outputNeuron = allNeurons.First(x => x.Id == syn.OutputNeuronId);
                synapse.InputNeuron = inputNeuron;
                synapse.OutputNeuron = outputNeuron;
                synapse.Weight = syn.Weight;
                synapse.WeightDelta = syn.WeightDelta;

                inputNeuron.OutputSynapses.Add(synapse);
                outputNeuron.InputSynapses.Add(synapse);

                progress += singleStep;
            }

            state = ProgressState.Done;

            return network;
        }

    }

    public enum ProgressState
    {
        Decompressing,
        InputLayer,
        HiddenLayers,
        OutputLayer,
        Synapses,
        Done
    }

}
