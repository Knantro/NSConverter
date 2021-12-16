using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MoreLinq;

namespace NSSolver {
    public static class NSConverter {
        public static string FromSomethingToSomethingNS(string value, int fromBasis, int toBasis) {
            CheckValue(value, fromBasis);
            CheckBasis(toBasis);
            var res = FromSomethingTo10NS(value, fromBasis);
            return From10ToSomethingNS(res, toBasis);
        }

        private static string From10ToSomethingNS(long value, int toBasis) {
            var dict = new Dictionary<int, NSConstants>();
            
            while (value > 0) {
                var degree = GetMaxDegree(value, toBasis);
                var coeff = GetMaxCoeff(value, (long)Math.Pow(toBasis, degree));

                value -= (long)Math.Pow(toBasis, degree) * coeff;
                
                dict.Add(degree, (NSConstants)coeff);
            }

            var result = new List<string>(Enumerable.Repeat("0", dict.First().Key + 1));
            dict.ForEach(x => result[x.Key] = x.Value.ToString());
            result.Reverse();
            
            return string.Join(string.Empty, result);
        }

        private static long FromSomethingTo10NS(string value, int fromBasis) {
            var reversedValue = value.Reverse().ToList();

            return long.Parse(Enumerable.Range(0, value.Length)
                                        .Select(i => Math.Pow(fromBasis, i) * 
                                                     (char.IsDigit(reversedValue[i])
                                                         ? int.Parse(reversedValue[i].ToString())
                                                         : (int) (NSConstants) reversedValue[i])).Sum()
                                        .ToString(CultureInfo.InvariantCulture));
        }

        private static int GetMaxDegree(long value, int basis) {
            var result = 0;
            
            while (Math.Pow(basis, result + 1) <= value) {
                result++;
            }

            return result;
        }

        private static int GetMaxCoeff(long value, long degreedValue) {
            var coeff = 1;
            
            while ((coeff + 1) * degreedValue <= value) {
                coeff++;
            }

            return coeff;
        }

        private static void CheckValue(string value, int basis) {
            CheckNumber(value);
            CheckBasis(basis);

            if (value.Any(x => Enum.IsDefined(typeof(NSConstants), char.ToUpper(x).ToString())
                ? (int) Enum.Parse<NSConstants>(char.ToUpper(x).ToString()) >= basis
                : int.Parse(x.ToString()) >= basis)) {
                throw new ArgumentException($"Value should contains a digits included in current basis = {basis}. " +
                                            $"It means that all digits can be in interval [0; {(NSConstants) (basis - 1)}].",
                    nameof(value));
            }
        }

        private static void CheckBasis(int basis) {
            if (basis is not (> 1 and < 17)) {
                throw new ArgumentException("Basis can be in interval [2; 16].", nameof(basis));
            }
        }

        private static void CheckNumber(string value) {
            if (!value.All(x => char.IsDigit(x) || Enum.IsDefined(typeof(NSConstants), char.ToUpper(x).ToString()))) {
                throw new ArgumentException("Value should be a number. (All digits in interval [0; F])", nameof(value));
            }
        }
    }
}