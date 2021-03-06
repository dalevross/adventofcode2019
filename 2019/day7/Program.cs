﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace day7
{
    class Program
    {
        static int terminatedCount = 0;
        static List<int> indices;
        static List<int> outputsignal = Enumerable.Repeat(0, 5).ToList();
        static int loop = 0;

        static List<int> lastOutPut;

        static List<int> currentPerm;
        static void Main(string[] args)
        {
            int[] ints;


            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());

            foreach (var file in dir.EnumerateFiles("Out*.csv"))
            {
                file.Delete();
            }

            List<List<int>> perms = GeneratePermutations(Enumerable.Range(5, 5).ToList());
            using (StreamReader sr = new StreamReader("sample.txt"))
            {

                int maxoutput = int.MinValue;
                string line;

                line = sr.ReadLine();
                string[] strs = line.Split(new char[] { ',' });
                ints = strs.Select(x => Int32.Parse(x)).ToArray();

                

                int i = 0;
                List<int> correctPerm = new List<int>();
                
              
                foreach (var list in perms)
                {
                    i = 0;
                    List<int[]> amplifiers = new List<int[]> { (int[])ints.Clone(), (int[])ints.Clone(), (int[])ints.Clone(), (int[])ints.Clone(), (int[])ints.Clone() };
                    outputsignal = Enumerable.Repeat(0, 5).ToList();
                    //var list = new List<int> { 9, 8, 7, 6, 5 };
                    currentPerm = new List<int>(list);
                    indices = Enumerable.Repeat(0, 5).ToList();
                    lastOutPut = Enumerable.Repeat(0, 5).ToList();
                    
                    while (i < 5)
                    {


                        outputsignal[i] = CalculateSignal(amplifiers[i], new int[] { list[i], outputsignal[(i == 0) ? 4 : i - 1] }, i);
                        if ((i + 1 == 5) && terminatedCount < 5)
                        {
                            i = 0;
                            loop++;


                        }
                        else
                        {
                            i++;
                        }

                    }
                    if (outputsignal[4] > maxoutput)
                    {
                        correctPerm = new List<int>(list);
                        maxoutput = outputsignal[4];
                    }

                }
                Console.WriteLine(maxoutput);
                foreach (var setting in correctPerm)
                {
                    Console.Write(setting);
                }
                //step2 = Step1((int[])ints.Clone(),5);


            }


        }

        static void PrintInstrunctions(int[] instructions)
        {

            Console.WriteLine(String.Join(",", instructions.Select(i => i.ToString())));

        }

        static void PrintInstrunctions(int[] instructions, StreamWriter sw)
        {

            sw.WriteLine(String.Join(",", instructions.Select(i => i.ToString())));

        }

        static int CalculateSignal(int[] ints, int[] inputs, int currentAmplifier)
        {
            int currentinput = 0;
            //Console.WriteLine("\n\n");
            //PrintInstrunctions(ints);
            //Console.WriteLine($"Processing Ampifier: {currentAmplifier} Index: {indices[currentAmplifier] } Terminated Count: {terminatedCount}");
            for (int i = indices[currentAmplifier]; i < ints.Length;)
            {


                if (ints[i] == 99)
                {                  
                    terminatedCount++;
                    return lastOutPut[currentAmplifier];
                }
                //Console.Write($"{i}: ");
                //PrintInstrunctions(ints);

                int opcode = ints[i];
                int param1 = ints[i + 1];
                int param2 = ints[i + 2];
                int updateindex = 0;
                if(i + 3 < ints.Length)
                    updateindex = ints[i + 3];
                string opcodeFilled = opcode.ToString().PadLeft(5, '0');
                //Console.WriteLine(opcodeFilled);
                int param1mode = int.Parse(opcodeFilled[2].ToString());
                int param2mode = int.Parse(opcodeFilled[1].ToString());
                int param3mode = int.Parse(opcodeFilled[0].ToString());
                opcode = int.Parse(opcodeFilled.Substring(3));


               
                /*using (StreamWriter sw = new StreamWriter($"OutpuFile{currentAmplifier}-{loop}.csv", true))
                {
                    sw.Write($"Loop: {loop}, In Opcode {opcodeFilled}, Index: {i},");
                    PrintInstrunctions(ints, sw);
                }*/
                

                switch (opcode)
                {

                    case 1:
                        ints[updateindex] = ((param1mode == 0) ? ints[param1] : param1) + ((param2mode == 0) ? ints[param2] : param2);
                        i += 4;
                        break;
                    case 2:
                        ints[updateindex] = ((param1mode == 0) ? ints[param1] : param1) * ((param2mode == 0) ? ints[param2] : param2);
                        i += 4;
                        break;
                    case 3:
                        ints[param1] = inputs[currentinput++];
                        i += 2;
                        break;
                    case 4:
                        i += 2;
                        indices[currentAmplifier] = i;
                        lastOutPut[currentAmplifier] = ints[param1];
                        if(ints[i]==99)
                            terminatedCount++;
                        return ints[param1];
                    case 5:
                        {
                            int param1Val = ((param1mode == 0) ? ints[param1] : param1);
                            int param2Val = ((param2mode == 0) ? ints[param2] : param2);
                            if (param1Val != 0)
                                i = param2Val;
                            else
                                i += 3;
                        }
                        break;
                    case 6:
                        {
                            int param1Val = ((param1mode == 0) ? ints[param1] : param1);
                            int param2Val = ((param2mode == 0) ? ints[param2] : param2);
                            if (param1Val == 0)
                                i = param2Val;
                            else
                                i += 3;
                        }
                        break;
                    case 7:
                        //Console.WriteLine($"{opcodeFilled} {param1} {param2} {updateindex} {ints[updateindex]}");

                        {
                            int param1Val = ((param1mode == 0) ? ints[param1] : param1);
                            int param2Val = ((param2mode == 0) ? ints[param2] : param2);

                            ints[updateindex] = (param1Val < param2Val) ? 1 : 0;

                            i += 4;
                        }
                        break;
                    case 8:
                        //Console.WriteLine($"{opcodeFilled} {param1} {param2} {updateindex} {ints[updateindex]}");

                        {
                            int param1Val = ((param1mode == 0) ? ints[param1] : param1);
                            int param2Val = ((param2mode == 0) ? ints[param2] : param2);

                            ints[updateindex] = (param1Val == param2Val) ? 1 : 0;
                        }

                        i += 4;
                        break;

                }


            }

            return ints[0];
        }

        static void SetupInstructions(int[] instructions, int noun, int verb)
        {
            instructions[1] = noun;
            instructions[2] = verb;
        }


        static int Step2(int[] ints, int target)
        {
            int[] localints;
            for (int noun = 0; noun <= 99; noun++)
            {
                for (int verb = 0; verb <= 99; verb++)
                {
                    localints = (int[])ints.Clone();
                    SetupInstructions(localints, noun, verb);
                    if (true)//tep1(localints, new int[2]()) == target)
                    {
                        return 100 * noun + verb;
                    }


                }

            }
            return 0;
        }

        private static List<List<T>> GeneratePermutations<T>(List<T> items)
        {
            // Make an array to hold the
            // permutation we are building.
            T[] current_permutation = new T[items.Count];

            // Make an array to tell whether
            // an item is in the current selection.
            bool[] in_selection = new bool[items.Count];

            // Make a result list.
            List<List<T>> results = new List<List<T>>();

            // Build the combinations recursively.
            PermuteItems<T>(items, in_selection,
                current_permutation, results, 0);

            // Return the results.
            return results;
        }

        private static void PermuteItems<T>(List<T> items, bool[] in_selection,
            T[] current_permutation, List<List<T>> results,
            int next_position)
        {
            // See if all of the positions are filled.
            if (next_position == items.Count)
            {
                // All of the positioned are filled.
                // Save this permutation.
                results.Add(current_permutation.ToList());
            }
            else
            {
                // Try options for the next position.
                for (int i = 0; i < items.Count; i++)
                {
                    if (!in_selection[i])
                    {
                        // Add this item to the current permutation.
                        in_selection[i] = true;
                        current_permutation[next_position] = items[i];

                        // Recursively fill the remaining positions.
                        PermuteItems<T>(items, in_selection,
                            current_permutation, results,
                            next_position + 1);

                        // Remove the item from the current permutation.
                        in_selection[i] = false;
                    }
                }
            }
        }
    }
}
