using K9.Base.DataAccessLayer.Enums;
using K9.WebApplication.Enums;
using K9.WebApplication.Models;
using System.Collections.Generic;

namespace K9.WebApplication.Extensions
{
    public static partial class Extensions
    {
        public static bool IsYin(this EGender gender)
        {
            return new List<EGender>
            {
                EGender.Female,
                EGender.Other,
            }.Contains(gender);
        }

        public static ETransformationType GetTransformationType(this EGetToTheRootKiEnergy energy1, EGetToTheRootKiEnergy energy2)
        {
            switch (energy1)
            {
                case EGetToTheRootKiEnergy.Water:
                    switch (energy2)
                    {
                        case EGetToTheRootKiEnergy.Water:
                            return ETransformationType.Same;

                        case EGetToTheRootKiEnergy.Thunder:
                        case EGetToTheRootKiEnergy.Wind:
                            return ETransformationType.Supports;

                        case EGetToTheRootKiEnergy.Soil:
                        case EGetToTheRootKiEnergy.CoreEarth:
                        case EGetToTheRootKiEnergy.Mountain:
                            return ETransformationType.IsChallenged;

                        case EGetToTheRootKiEnergy.Heaven:
                        case EGetToTheRootKiEnergy.Lake:
                            return ETransformationType.IsSupported;

                        case EGetToTheRootKiEnergy.Fire:
                            return ETransformationType.Challenges;
                    }

                    break;

                case EGetToTheRootKiEnergy.Soil:
                case EGetToTheRootKiEnergy.CoreEarth:
                case EGetToTheRootKiEnergy.Mountain:
                    switch (energy2)
                    {
                        case EGetToTheRootKiEnergy.Water:
                            return ETransformationType.Challenges;

                        case EGetToTheRootKiEnergy.Thunder:
                        case EGetToTheRootKiEnergy.Wind:
                            return ETransformationType.IsChallenged;

                        case EGetToTheRootKiEnergy.Soil:
                        case EGetToTheRootKiEnergy.CoreEarth:
                        case EGetToTheRootKiEnergy.Mountain:
                            return ETransformationType.Same;

                        case EGetToTheRootKiEnergy.Heaven:
                        case EGetToTheRootKiEnergy.Lake:
                            return ETransformationType.Supports;

                        case EGetToTheRootKiEnergy.Fire:
                            return ETransformationType.IsSupported;
                    }

                    break;

                case EGetToTheRootKiEnergy.Thunder:
                case EGetToTheRootKiEnergy.Wind:
                    switch (energy2)
                    {
                        case EGetToTheRootKiEnergy.Water:
                            return ETransformationType.IsSupported;

                        case EGetToTheRootKiEnergy.Thunder:
                        case EGetToTheRootKiEnergy.Wind:
                            return ETransformationType.Same;

                        case EGetToTheRootKiEnergy.Soil:
                        case EGetToTheRootKiEnergy.CoreEarth:
                        case EGetToTheRootKiEnergy.Mountain:
                            return ETransformationType.Challenges;

                        case EGetToTheRootKiEnergy.Heaven:
                        case EGetToTheRootKiEnergy.Lake:
                            return ETransformationType.IsChallenged;

                        case EGetToTheRootKiEnergy.Fire:
                            return ETransformationType.Supports;
                    }

                    break;

                case EGetToTheRootKiEnergy.Heaven:
                case EGetToTheRootKiEnergy.Lake:
                    switch (energy2)
                    {
                        case EGetToTheRootKiEnergy.Water:
                            return ETransformationType.Supports;

                        case EGetToTheRootKiEnergy.Thunder:
                        case EGetToTheRootKiEnergy.Wind:
                            return ETransformationType.Challenges;

                        case EGetToTheRootKiEnergy.Soil:
                        case EGetToTheRootKiEnergy.CoreEarth:
                        case EGetToTheRootKiEnergy.Mountain:
                            return ETransformationType.IsSupported;

                        case EGetToTheRootKiEnergy.Heaven:
                        case EGetToTheRootKiEnergy.Lake:
                            return ETransformationType.Same;

                        case EGetToTheRootKiEnergy.Fire:
                            return ETransformationType.IsChallenged;
                    }

                    break;

                case EGetToTheRootKiEnergy.Fire:
                    switch (energy2)
                    {
                        case EGetToTheRootKiEnergy.Water:
                            return ETransformationType.IsChallenged;

                        case EGetToTheRootKiEnergy.Thunder:
                        case EGetToTheRootKiEnergy.Wind:
                            return ETransformationType.IsSupported;

                        case EGetToTheRootKiEnergy.Soil:
                        case EGetToTheRootKiEnergy.CoreEarth:
                        case EGetToTheRootKiEnergy.Mountain:
                            return ETransformationType.Supports;

                        case EGetToTheRootKiEnergy.Heaven:
                        case EGetToTheRootKiEnergy.Lake:
                            return ETransformationType.Challenges;

                        case EGetToTheRootKiEnergy.Fire:
                            return ETransformationType.Same;
                    }

                    break;
            }

            return ETransformationType.Unspecified;
        }
    }
}
