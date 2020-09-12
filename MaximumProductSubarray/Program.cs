using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Threading;

namespace MaximumProductSubarray
{
    class Program
    {
        //Problem: https://leetcode.com/explore/challenge/card/september-leetcoding-challenge/555/week-2-september-8th-september-14th/3456/
        /*
         * Solution the the 2020-9/11 Daily Problem
         * Included is the original (overthinking) solution.
         * Right idea but too much code. 
         * I've updated with the more elegant simple solution. 
         */

        static void Main(string[] args)
        {
            int ans = MaxProduct(new int[] {-2, 0});
            Console.WriteLine(ans);
        }

        /// <summary>
        /// A more elegant, dynamic solution for the MaxProduct problem. 
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public static int MaxProduct(int[] nums)
        {
            if (nums.Length == 0) return 0;

            int max = nums[0]; //Keep track of the max
            int min = nums[0]; //Keep track of the min
            int result = max;

            int cur, tempMax;
            for (int i = 1; i < nums.Length; i++)
            {
                cur = nums[i];
                tempMax = Math.Max(cur, Math.Max(max * cur, min * cur));
                min = Math.Min(cur, Math.Min(max * cur, min * cur));
                max = tempMax;

                result = Math.Max(max, result);
            }
            return result;
        }

        public static int MaxProductOverthinking(int[] nums)
        {
            List<int> zeroIdx = new List<int>();
            int max = nums[0];
            if (nums.Length == 1) return max;
            for(int i = 0; i < nums.Length; i++)
            {
                if (nums[i] > max)
                    max = nums[i];
                if (nums[i] == 0)
                    zeroIdx.Add(i);
            }
            if (zeroIdx.Count > 0)
            {//Partition by Zero
                int maxPart;
                int lastIdx = 0;
                for(int i = 0; i < zeroIdx.Count; i++)
                {
                    maxPart = MaxProductOverthinking(nums, lastIdx, zeroIdx[i]-1);
                    if (maxPart > max)
                        max = maxPart;
                    lastIdx = zeroIdx[i]+1;
                }
                maxPart = MaxProductOverthinking(nums, lastIdx, nums.Length-1);
                return max > maxPart ? max : maxPart;
            }
            else return MaxProductOverthinking(nums, 0, nums.Length-1);

        }
        public static int MaxProductOverthinking(int[] nums, int start, int limit)
        { //No Zeroes allowed
            if (start < 0) return int.MinValue;
            if (limit >= nums.Length) return int.MinValue;
            if (start > limit) return int.MinValue;
            if (start == limit) return nums[start];
            int max = 1;
            List<int> negIdx = new List<int>();
            for(int i = start; i <= limit; i++)
            {
                max *= nums[i];
                if (nums[i] < 0)
                    negIdx.Add(i);
            }
            if (negIdx.Count % 2 == 1) //Odd number of negative values
            { //Partition Further
                int firstPartition = MaxProductOverthinking(nums, start, negIdx[negIdx.Count - 1]-1);
                int secondPartition = MaxProductOverthinking(nums, negIdx[0]+1, limit);
                int maxPart = firstPartition > secondPartition ? firstPartition : secondPartition;
                if (maxPart > max)
                    max = maxPart;
            }
            return max;

        }

    }
}
