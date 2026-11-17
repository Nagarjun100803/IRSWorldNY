using System;
using System.Collections.Generic;
using System.Text;

namespace IRSClassLib
{
    public class irsMethaphone
    {
        private string MyMid(string InputString, int Start, int Length)
        {
            if (Start < 1)
            {
                return null;
            }
            else
            {
                return InputString.Substring(Start, Length);
            }
        }

        private Boolean ContainsAnyOf(string BaseStringToSearch, params  String[] StringsToSearchFor)
        {
            for (int i = 0; i < StringsToSearchFor.Length; i++)
            {
                if (StringsToSearchFor[i] == BaseStringToSearch)
                {
                    return true;
                }
            }
            return false;
        }

        private Boolean IsSlavoGermanic(string InputString)
        {
            if (InputString.IndexOf("W") != -1 || InputString.IndexOf("K") != -1 ||
                InputString.IndexOf("CZ") != -1 || InputString.IndexOf("WITZ") != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string DoubleMethaphone(string InputString)
        {
            int iCurrentPosition;
            string CurrentChar;
            int Last;
            string tmpPrimary;
            string tmpSecondary;

            iCurrentPosition = 0;
            Last = InputString.Length - 1;
            tmpPrimary = "";
            tmpSecondary = "";

            InputString = InputString.ToUpper();
            InputString = InputString + "       ";

            //skip these when at start of word
            if (ContainsAnyOf(InputString.Substring(iCurrentPosition, 2), "GN", "KN", "PN", "WR", "PS"))
            {
                iCurrentPosition += 1;
            }

            // Initial 'X' is pronounced 'Z' e.g. 'Xavier'
            if (InputString.Substring(0, 1) == "X")
            {
                tmpPrimary = tmpPrimary + "S";
                tmpSecondary = tmpSecondary + "S";
                iCurrentPosition += 1;
            }

            do
            {
                CurrentChar = InputString.Substring(iCurrentPosition, 1);
                switch (CurrentChar)
                {
                    case "A":
                    case "E":
                    case "I":
                    case "O":
                    case "U":
                    case "Y":
                        if (iCurrentPosition == 0)
                        {
                            tmpPrimary += "A";
                            tmpSecondary += "A";
                        }
                        iCurrentPosition += 1;
                        break;
                    case "B": // "-mb", e.g", "dumb", already skipped over...
                        tmpPrimary += "P";
                        tmpSecondary += "P";

                        if (InputString.Substring(iCurrentPosition + 1, 1) == "B")
                        {
                            iCurrentPosition += 2;
                        }
                        else
                        {
                            iCurrentPosition += 1;
                        }
                        break;
                    case "Ç":
                        tmpPrimary += "S";
                        tmpSecondary += "S";
                        iCurrentPosition += 1;
                        break;

                    case "C":
                        //various germanic
                        if ((iCurrentPosition > 1) &&
                            ContainsAnyOf(InputString.Substring(iCurrentPosition - 1), "A", "E", "I", "O", "U", "Y") &&
                            InputString.Substring(iCurrentPosition - 1, 3) == "ACH" &&
                            (InputString.Substring(iCurrentPosition + 2, 1) != "I" &&
                            (InputString.Substring(iCurrentPosition + 2, 1) != "E" ||
                            ContainsAnyOf(InputString.Substring(iCurrentPosition - 2, 6), "BACHER", "MACHER"))))
                        {
                            tmpPrimary += "K";
                            tmpSecondary += "K";
                            iCurrentPosition += 2;
                            break;
                        }

                        //special case 'caesar'
                        if (iCurrentPosition == 0 && InputString.Substring(0, 6) == "CAESAR")
                        {
                            tmpPrimary += "S";
                            tmpSecondary += "S";
                            iCurrentPosition += 2;
                            break;
                        }

                        //italian "chianti"
                        if (InputString.Substring(iCurrentPosition, 4) == "CHIA")
                        {
                            tmpPrimary += "K";
                            tmpSecondary += "K";
                            iCurrentPosition += 2;
                            break;
                        }

                        if (InputString.Substring(iCurrentPosition, 2) == "CH")
                        {
                            //find michael
                            if (iCurrentPosition > 0 && InputString.Substring(iCurrentPosition, 4) == "CHAE")
                            {
                                tmpPrimary += "K";
                                tmpSecondary += "X";
                                iCurrentPosition += 2;
                                break;
                            }

                            //Greek roots e.g. "chemistry" and "chorus"
                            if (iCurrentPosition == 0 &&
                               ((ContainsAnyOf(InputString.Substring(iCurrentPosition + 1, 5), "HARAC", "HARIS") ||
                                ContainsAnyOf(InputString.Substring(iCurrentPosition + 1, 3), "HOR", "HYM", "HIA", "HEM")) &&
                                InputString.Substring(0, 5) != "CHORE"
                               ))
                            {
                                tmpPrimary += "K";
                                tmpSecondary += "K";
                                iCurrentPosition += 2;
                                break;
                            }

                            //Germanic, Greek or otherwise "ch" for "kh" sound
                            if ((ContainsAnyOf(InputString.Substring(0, 4), "VAN ", "VON ") ||
                                InputString.Substring(0, 3) == "SCH") ||
                                (iCurrentPosition > 1 && ContainsAnyOf(InputString.Substring(iCurrentPosition - 2, 6), "ORCHES", "ARCHIT", "ORCHID")) ||
                                ContainsAnyOf(InputString.Substring(iCurrentPosition + 2, 1), "T", "S") ||
                                (iCurrentPosition > 0 && (ContainsAnyOf(InputString.Substring(iCurrentPosition - 1, 1), "A", "O", "U", "E") ||
                                  iCurrentPosition == 1) &&
                                  ContainsAnyOf(InputString.Substring(iCurrentPosition + 2, 1), "L", "R", "N", "M", "B", "H", "F", "V", "W")
                                ))
                            {
                                tmpPrimary += "K";
                                tmpSecondary += "K";
                            }
                            else
                            {
                                if (iCurrentPosition > 0)
                                {
                                    if (InputString.Substring(0, 2) == "MC")
                                    {
                                        //e.g "McHugh"
                                        tmpPrimary += "K";
                                        tmpSecondary += "K";
                                    }
                                    else
                                    {
                                        tmpPrimary += "X";
                                        tmpSecondary += "K";
                                    }
                                }
                                else
                                {
                                    tmpPrimary += "X";
                                    tmpSecondary += "X";
                                }
                            }

                            iCurrentPosition += 2;
                            break;
                        }

                        //e.g, 'czerny'
                        if (InputString.Substring(iCurrentPosition, 2) == "CZ" &&
                            (iCurrentPosition > 1 && InputString.Substring(iCurrentPosition - 2, 4) != "WICZ"))
                        {
                            tmpPrimary += "S";
                            tmpSecondary += "X";
                            iCurrentPosition += 2;
                            break;
                        }

                        //e.g., 'focaccia'
                        if (InputString.Substring(iCurrentPosition + 1, 3) == "CIA")
                        {
                            tmpPrimary += "X";
                            tmpSecondary += "X";

                            iCurrentPosition += 3;
                            break;
                        }

                        //double 'C', but not if e.g. 'McClellan'
                        if (InputString.Substring(iCurrentPosition, 2) == "CC" &&
                            !(iCurrentPosition == 1 && InputString.Substring(0, 1) == "M"))
                        {
                            //bellocchio' but not 'bacchus'
                            if (ContainsAnyOf(InputString.Substring(iCurrentPosition + 2, 1), "I", "E", "H") &&
                                !(InputString.Substring(iCurrentPosition + 2, 2) == "HU"))
                            {
                                // 'accident', 'accede' 'succeed'
                                if ((iCurrentPosition == 1 && InputString.Substring(iCurrentPosition - 1, 1) == "A") ||
                                    (iCurrentPosition > 0 && ContainsAnyOf(InputString.Substring(iCurrentPosition - 1, 5), "UCCEE", "UCCES")))
                                {
                                    tmpPrimary += "KS";
                                    tmpSecondary += "KS";
                                }
                                else
                                {
                                    tmpPrimary += "X";
                                    tmpSecondary += "X";
                                }

                                iCurrentPosition += 3;
                                break;
                            }
                            else //Pierce's rule
                            {
                                tmpPrimary += "K";
                                tmpSecondary += "K";
                                iCurrentPosition += 2;
                                break;
                            }
                        }

                        if (ContainsAnyOf(InputString.Substring(iCurrentPosition, 2), "CK", "CG", "CQ"))
                        {
                            tmpPrimary += "K";
                            tmpSecondary += "K";
                            iCurrentPosition += 2;
                            break;
                        }

                        if (ContainsAnyOf(InputString.Substring(iCurrentPosition, 2), "CI", "CE", "CY"))
                        {
                            //italian vs. english
                            if (ContainsAnyOf(InputString.Substring(iCurrentPosition, 3), "CIO", "CIE", "CIA"))
                            {
                                tmpPrimary += "S";
                                tmpSecondary += "X";
                            }
                            else
                            {
                                tmpPrimary += "S";
                                tmpSecondary += "S";
                            }
                            iCurrentPosition += 2;
                            break;
                        }

                        tmpPrimary += "K";
                        tmpSecondary += "K";

                        //name sent in 'mac caffrey', 'mac gregor
                        if (ContainsAnyOf(InputString.Substring(iCurrentPosition + 1, 2), " C", " Q", " G"))
                        {
                            iCurrentPosition += 3;
                        }
                        else
                        {
                            if (ContainsAnyOf(InputString.Substring(iCurrentPosition + 1, 1), "C", "K", "Q") &&
                                (!ContainsAnyOf(InputString.Substring(iCurrentPosition + 1, 2), "CE", "CI")))
                            {
                                iCurrentPosition += 2;
                            }
                            else
                            {
                                iCurrentPosition += 1;
                            }
                        }
                        break;

                    case "D":
                        if (InputString.Substring(iCurrentPosition, 2) == "DG")
                        {
                            if (ContainsAnyOf(InputString.Substring(iCurrentPosition + 2, 1), "E", "I", "Y"))
                            {
                                //e.g. 'edge'
                                tmpPrimary += "J";
                                tmpSecondary += "J";
                                iCurrentPosition += 3;
                                break;
                            }
                            else
                            {
                                //e.g. 'edgar'
                                tmpPrimary += "TK";
                                tmpSecondary += "TK";
                                iCurrentPosition += 2;
                                break;
                            }
                        }

                        if (ContainsAnyOf(InputString.Substring(iCurrentPosition, 2), "DT", "DD"))
                        {
                            tmpPrimary += "T";
                            tmpSecondary += "T";
                            iCurrentPosition += 2;
                            break;
                        }

                        tmpPrimary += "T";
                        tmpSecondary += "T";

                        iCurrentPosition += 1;
                        break;

                    case "F":
                        if (InputString.Substring(iCurrentPosition + 1, 1) == "F")
                        {
                            iCurrentPosition += 2;
                        }
                        else
                        {
                            iCurrentPosition += 1;
                        }

                        tmpPrimary += "F";
                        tmpSecondary += "F";
                        break;
                    case "G":
                        if (InputString.Substring(iCurrentPosition + 1, 1) == "H")
                        {
                            if (iCurrentPosition > 0 &&
                                !(ContainsAnyOf(InputString.Substring(iCurrentPosition - 1, 1), "A", "E", "I", "O", "U", "Y")))
                            {
                                tmpPrimary += "K";
                                tmpSecondary += "K";

                                iCurrentPosition += 2;
                                break;
                            }

                            if (iCurrentPosition < 3)
                            {
                                //'ghislane', ghiradelli
                                if (iCurrentPosition == 0)
                                {
                                    if (InputString.Substring(iCurrentPosition + 2, 1) == "I")
                                    {
                                        tmpPrimary += "J";
                                        tmpSecondary += "J";
                                    }
                                    else
                                    {
                                        tmpPrimary += "K";
                                        tmpSecondary += "K";
                                    }
                                    iCurrentPosition += 2;
                                    break;
                                }
                            }

                            //Parker's rule (with some further refinements) - e.g., 'hugh'
                            if ((iCurrentPosition > 1 && ContainsAnyOf(InputString.Substring(iCurrentPosition - 2, 1), "B", "H", "D")) ||
                                (iCurrentPosition > 2 && ContainsAnyOf(InputString.Substring(iCurrentPosition - 3, 1), "B", "H", "D")) ||
                                (iCurrentPosition > 3 && ContainsAnyOf(InputString.Substring(iCurrentPosition - 4, 1), "B", "H")))
                            {
                                iCurrentPosition += 2;
                                break;
                            }
                            else
                            {
                                if (iCurrentPosition > 2 &&
                                    InputString.Substring(iCurrentPosition - 1, 1) == "U" &&
                                    ContainsAnyOf(InputString.Substring(iCurrentPosition - 3, 1), "C", "G", "L", "R", "T"))
                                {
                                    tmpPrimary += "F";
                                    tmpSecondary += "F";
                                }
                                else
                                {
                                    if (iCurrentPosition > 0 && InputString.Substring(iCurrentPosition - 1, 1) != "I")
                                    {
                                        tmpPrimary += "F";
                                        tmpSecondary += "F";
                                    }
                                }
                                iCurrentPosition += 2;
                                break;
                            }
                        }

                        if (InputString.Substring(iCurrentPosition + 1, 1) == "N")
                        {
                            if (iCurrentPosition == 1 &&
                                ContainsAnyOf(InputString.Substring(0, 1), "A", "E", "I", "O", "U", "Y") &&
                                !IsSlavoGermanic(InputString))
                            {
                                tmpPrimary += "KN";
                                tmpSecondary += "N";
                            }
                            //not e.g. 'cagney'
                            else if (!(InputString.Substring(iCurrentPosition + 2, 2) == "EY") &&
                                    InputString.Substring(iCurrentPosition + 1, 1) != "Y" &&
                                    !IsSlavoGermanic(InputString))
                            {
                                tmpPrimary += "N";
                                tmpSecondary += "KN";
                            }
                            else
                            {
                                tmpPrimary += "KN";
                                tmpSecondary += "KN";
                            }
                            iCurrentPosition += 2;
                            break;
                        }

                        //tagliaro
                        if (InputString.Substring(iCurrentPosition + 1, 2) == "LI" &&
                            !IsSlavoGermanic(InputString))
                        {
                            tmpPrimary += "KL";
                            tmpSecondary += "L";
                            iCurrentPosition += 2;
                            break;
                        }

                        //-ges-,-gep-,-gel-, -gie- at beginning
                        if (iCurrentPosition == 0 &&
                            (InputString.Substring(iCurrentPosition + 1, 1) == "Y" ||
                            ContainsAnyOf(InputString.Substring(iCurrentPosition + 1, 2), "ES", "EP", "EB", "EL", "EY", "IB", "IL", "IN", "IE", "EI", "ER")))
                        {
                            tmpPrimary += "K";
                            tmpSecondary += "J";
                            iCurrentPosition += 2;
                            break;
                        }

                        //-ger-,  -gy-
                        if ((InputString.Substring(iCurrentPosition + 1, 2) == "ER" ||
                            InputString.Substring(iCurrentPosition + 1, 1) == "Y") &&
                            !ContainsAnyOf(InputString.Substring(1, 6), "DANGER", "RANGER", "MANGER") &&
                            (iCurrentPosition > 0 && !ContainsAnyOf(InputString.Substring(iCurrentPosition - 1, 1), "E", "I")) &&
                            (iCurrentPosition > 0 && !ContainsAnyOf(InputString.Substring(iCurrentPosition - 1, 3), "RGY", "OGY")))
                        {
                            tmpPrimary += "K";
                            tmpSecondary += "J";
                            iCurrentPosition += 2;
                            break;
                        }

                        // italian e.g, 'biaggi'
                        if (ContainsAnyOf(InputString.Substring(iCurrentPosition + 1, 1), "E", "I", "Y") ||
                            (iCurrentPosition > 0 && ContainsAnyOf(InputString.Substring(iCurrentPosition - 1, 4), "AGGI", "OGGI")))
                        {
                            //obvious germanic
                            if (ContainsAnyOf(InputString.Substring(1, 4), "VAN ", "VON ") ||
                                InputString.Substring(1, 3) == "SCH" ||
                                InputString.Substring(iCurrentPosition + 1, 2) == "ET"
                                )
                            {
                                tmpPrimary += "K";
                                tmpSecondary += "K";
                            }
                            // always soft if french ending
                            else if (InputString.Substring(iCurrentPosition + 1, 4) == "IER ")
                            {
                                tmpPrimary += "J";
                                tmpSecondary += "J";
                            }
                            else
                            {
                                tmpPrimary += "J";
                                tmpSecondary += "K";
                            }
                            iCurrentPosition += 2;
                            break;
                        }

                        if (InputString.Substring(iCurrentPosition + 1, 1) == "G")
                        {
                            iCurrentPosition += 2;
                        }
                        else
                        {
                            iCurrentPosition += 1;
                        }

                        tmpPrimary += "K";
                        tmpSecondary += "K";
                        break;
                    case "H":
                        if ((iCurrentPosition == 0 ||
                            (iCurrentPosition > 0 && ContainsAnyOf(InputString.Substring(iCurrentPosition - 1, 1), "A", "E", "I", "O", "U", "Y"))) &&
                            ContainsAnyOf(InputString.Substring(iCurrentPosition + 1, 1), "A", "E", "I", "O", "U", "Y"))
                        {
                            tmpPrimary += "H";
                            tmpSecondary += "H";
                            iCurrentPosition += 2;
                        }
                        else
                        {
                            iCurrentPosition += 1;
                        }
                        break;

                    case "J":
                        // obvious spanish, 'jose', 'san jacinto'
                        if (InputString.Substring(iCurrentPosition, 4) == "JOSE" ||
                            InputString.Substring(0, 4) == "SAN ")
                        {
                            if ((iCurrentPosition == 0 &&
                                InputString.Substring(iCurrentPosition + 4, 1) == " ") ||
                                InputString.Substring(0, 4) == "SAN ")
                            {
                                tmpPrimary += "H";
                                tmpSecondary += "H";
                            }
                            else
                            {
                                tmpPrimary += "J";
                                tmpSecondary += "H";
                            }
                            iCurrentPosition += 1;
                            break;
                        }

                        if (iCurrentPosition == 0 && !(InputString.Substring(iCurrentPosition, 4) == "JOSE"))
                        {
                            tmpPrimary += "J";
                            tmpSecondary += "A";
                        }
                        // spanish pron. of e.g. 'bajador'
                        else if ((iCurrentPosition > 0 && ContainsAnyOf(InputString.Substring(iCurrentPosition - 1, 1), "A", "E", "I", "O", "U", "Y")) &&
                                !IsSlavoGermanic(InputString) &&
                                ContainsAnyOf(InputString.Substring(iCurrentPosition + 1, 1), "A", "O"))
                        {
                            tmpPrimary += "J";
                            tmpSecondary += "H";
                        }
                        else if (iCurrentPosition == Last)
                        {
                            tmpPrimary += "J";
                            //tmpSecondary += " ";
                        }
                        else if (!ContainsAnyOf(InputString.Substring(iCurrentPosition + 1, 1), "L", "T", "K", "S", "N", "M", "B", "Z") &&
                                (iCurrentPosition > 0 && !ContainsAnyOf(InputString.Substring(iCurrentPosition - 1, 1), "S", "K", "L")))
                        {
                            tmpPrimary += "J";
                            tmpSecondary += "J";
                        }

                        if (InputString.Substring(iCurrentPosition + 1, 1) == "J")
                        {
                            iCurrentPosition += 2;
                        }
                        else
                        {
                            iCurrentPosition += 1;
                        }
                        break;

                    case "K":
                        if (InputString.Substring(iCurrentPosition + 1, 1) == "K")
                        {
                            iCurrentPosition += 2;
                        }
                        else
                        {
                            iCurrentPosition += 1;
                        }

                        tmpPrimary += "K";
                        tmpSecondary += "K";
                        break;

                    case "L":
                        if (InputString.Substring(iCurrentPosition + 1, 1) == "L")
                        {
                            //spanish e.g. 'cabrillo', 'gallegos'
                            if ((iCurrentPosition > 0 && iCurrentPosition == Last - 3 &&
                               ContainsAnyOf(InputString.Substring(iCurrentPosition - 1, 4), "ILLO", "ILLA", "ALLE")) ||
                               ((ContainsAnyOf(InputString.Substring(Last - 1, 2), "AS", "OS") ||
                               ContainsAnyOf(InputString.Substring(Last, 1), "A", "O")) &&
                               (iCurrentPosition > 0 && InputString.Substring(iCurrentPosition - 1, 4) == "ALLE")))
                            {
                                tmpPrimary += "L";
                                iCurrentPosition += 2;
                                break;
                            }
                            iCurrentPosition += 2;
                        }
                        else
                        {
                            iCurrentPosition += 1;
                        }

                        tmpPrimary += "L";
                        tmpSecondary += "L";
                        break;

                    case "M":
                        if (InputString.Substring(iCurrentPosition + 1, 1) == "M" ||
                            ((iCurrentPosition > 0 && InputString.Substring(iCurrentPosition - 1, 3) == "UMB") &&
                            (iCurrentPosition + 1 == Last ||
                            InputString.Substring(iCurrentPosition + 2, 2) == "ER"))
                            )
                        {
                            iCurrentPosition += 2;
                        }
                        else
                        {
                            iCurrentPosition += 1;
                        }

                        tmpPrimary += "M";
                        tmpSecondary += "M";
                        break;

                    case "N":
                        if (InputString.Substring(iCurrentPosition + 1, 1) == "N")
                        {
                            iCurrentPosition += 2;
                        }
                        else
                        {
                            iCurrentPosition += 1;
                        }
                        tmpPrimary += "N";
                        tmpSecondary += "N";
                        break;

                    case "Ñ":
                        tmpPrimary += "Ñ";
                        tmpSecondary += "Ñ";
                        iCurrentPosition += 1;
                        break;

                    case "P":
                        if (InputString.Substring(1, 1) == "H")
                        {
                            tmpPrimary += "F";
                            tmpSecondary += "F";
                            iCurrentPosition += 2;
                            break;
                        }
                        //Also account for CAMPBELL and RASPBERRY
                        if (ContainsAnyOf(InputString.Substring(iCurrentPosition + 1, 1), "P", "B"))
                        {
                            iCurrentPosition += 2;
                        }
                        else
                        {
                            iCurrentPosition += 1;
                        }

                        tmpPrimary += "P";
                        tmpSecondary += "P";
                        break;

                    case "Q":
                        if (InputString.Substring(iCurrentPosition + 1, 1) == "Q")
                        {
                            iCurrentPosition += 2;
                        }
                        else
                        {
                            iCurrentPosition += 1;
                        }
                        tmpPrimary += "K";
                        tmpSecondary += "K";
                        break;
                    case "R":
                        //french e.g. 'rogier', but exclude 'hochmeier'
                        if (iCurrentPosition == Last &&
                            !IsSlavoGermanic(InputString) &&
                            (iCurrentPosition > 1 && InputString.Substring(iCurrentPosition - 2, 2) == "IE") &&
                            (iCurrentPosition > 3 && !ContainsAnyOf(InputString.Substring(iCurrentPosition - 4, 2), "ME", "MA")))
                        {
                            tmpSecondary += "R";
                        }
                        else
                        {
                            tmpPrimary += "R";
                            tmpSecondary += "R";
                        }

                        if (InputString.Substring(iCurrentPosition + 1, 1) == "R")
                        {
                            iCurrentPosition += 2;
                        }
                        else
                        {
                            iCurrentPosition += 1;
                        }
                        break;

                    case "S":
                        // special cases 'island', 'isle', 'carlisle', 'carlysle'
                        if (iCurrentPosition > 0 && ContainsAnyOf(InputString.Substring(iCurrentPosition - 1, 3), "ISL", "YSL"))
                        {
                            iCurrentPosition += 1;
                            break;
                        }

                        //special case 'sugar-'
                        if (iCurrentPosition == 0 && InputString.Substring(0, 5) == "SUGAR")
                        {
                            tmpPrimary += "X";
                            tmpSecondary += "S";
                            iCurrentPosition += 1;
                            break;
                        }

                        if (InputString.Substring(iCurrentPosition, 2) == "SH")
                        {
                            //Germanic
                            if (ContainsAnyOf(InputString.Substring(iCurrentPosition + 1, 4), "HEIM", "HOEK", "HOLM", "HOLZ"))
                            {
                                tmpPrimary += "S";
                                tmpSecondary += "S";
                            }
                            else
                            {
                                //MetaphAdd ("X")
                                tmpPrimary += "X";
                                tmpSecondary += "X";
                            }
                            iCurrentPosition += 2;
                            break;
                        }

                        //Italian & Armenian
                        if (ContainsAnyOf(InputString.Substring(iCurrentPosition, 3), "SIO", "SIA") ||
                            InputString.Substring(iCurrentPosition, 4) == "SIAN")
                        {
                            if (!IsSlavoGermanic(InputString))
                            {
                                tmpPrimary += "S";
                                tmpSecondary += "X";
                            }
                            else
                            {
                                tmpPrimary += "S";
                                tmpSecondary += "S";
                            }

                            iCurrentPosition += 3;
                            break;
                        }

                        //german & anglicisations, e.g. 'smith' match 'schmidt', 'snider' match 'schneider'
                        //also, -sz- in slavic language altho in hungarian it is pronounced 's'                        
                        if (iCurrentPosition == 0 &&
                            ContainsAnyOf(InputString.Substring(iCurrentPosition + 1, 1), "M", "N", "L", "W") ||
                            InputString.Substring(iCurrentPosition + 1, 1) == "Z")
                        {
                            tmpPrimary += "S";
                            tmpSecondary += "X";

                            if (InputString.Substring(iCurrentPosition + 1, 1) == "Z")
                            {
                                iCurrentPosition += 2;
                            }
                            else
                            {
                                iCurrentPosition += 1;
                            }
                            break;
                        }

                        if (InputString.Substring(iCurrentPosition, 2) == "SC")
                        {
                            // Schlesinger's rule
                            if (InputString.Substring(iCurrentPosition + 2, 1) == "H")
                            {
                                //dutch origin, e.g. 'school', 'schooner'
                                if (ContainsAnyOf(InputString.Substring(iCurrentPosition + 3, 2), "OO", "ER", "EN", "UY", "ED", "EM"))
                                {
                                    //'schermerhorn', 'schenker'
                                    if (ContainsAnyOf(InputString.Substring(iCurrentPosition + 3, 2), "ER", "EN"))
                                    {
                                        tmpPrimary += "X";
                                        tmpSecondary += "SK";
                                    }
                                    else
                                    {
                                        tmpPrimary += "SK";
                                        tmpSecondary += "SK";
                                    }
                                    iCurrentPosition += 3;
                                    break;
                                }
                                else
                                {
                                    if (iCurrentPosition == 0 &&
                                        !ContainsAnyOf(InputString.Substring(3, 1), "A", "E", "I", "O", "U", "Y", "W"))
                                    {
                                        tmpPrimary += "X";
                                        tmpSecondary += "S";
                                    }
                                    else
                                    {
                                        tmpPrimary += "X";
                                        tmpSecondary += "X";
                                    }
                                    iCurrentPosition += 3;
                                    break;
                                }
                            }

                            if (ContainsAnyOf(InputString.Substring(iCurrentPosition + 2, 1), "I", "E", "Y"))
                            {
                                tmpPrimary += "S";
                                tmpSecondary += "S";
                                iCurrentPosition += 3;
                                break;
                            }

                            //otherwise
                            tmpPrimary += "SK";
                            tmpSecondary += "SK";
                            iCurrentPosition += 3;
                            break;
                        }

                        //french e.g. 'resnais', 'artois'
                        if (iCurrentPosition == Last &&
                            (iCurrentPosition > 1 && ContainsAnyOf(InputString.Substring(iCurrentPosition - 2, 2), "AI", "OI")))
                        {
                            tmpSecondary += "S";
                        }
                        else
                        {
                            tmpPrimary += "S";
                            tmpSecondary += "S";
                        }

                        if (ContainsAnyOf(InputString.Substring(iCurrentPosition + 1, 1), "S", "Z"))
                        {
                            iCurrentPosition += 2;
                        }
                        else
                        {
                            iCurrentPosition += 1;
                        }
                        break;

                    case "T":
                        if (InputString.Substring(iCurrentPosition, 4) == "TION")
                        {
                            tmpPrimary += "X";
                            tmpSecondary += "X";
                            iCurrentPosition += 3;
                            break;
                        }

                        if (ContainsAnyOf(InputString.Substring(iCurrentPosition, 3), "TIA", "TCH"))
                        {
                            tmpPrimary += "X";
                            tmpSecondary += "X";
                            iCurrentPosition += 3;
                            break;
                        }

                        if (InputString.Substring(iCurrentPosition, 2) == "TH" ||
                            InputString.Substring(iCurrentPosition, 3) == "TTH")
                        {
                            //special case 'thomas', 'thames' or germanic
                            if (ContainsAnyOf(InputString.Substring(iCurrentPosition + 2, 2), "OM", "AM") ||
                                ContainsAnyOf(InputString.Substring(0, 4), "VAN ", "VON ") ||
                                InputString.Substring(0, 3) == "SCH")
                            {
                                tmpPrimary += "T";
                                tmpSecondary += "T";
                            }
                            else
                            {
                                tmpPrimary += "0";
                                tmpSecondary += "T";
                            }
                            iCurrentPosition += 2;
                            break;
                        }

                        if (ContainsAnyOf(InputString.Substring(iCurrentPosition + 1, 1), "T", "D"))
                        {
                            iCurrentPosition += 2;
                        }
                        else
                        {
                            iCurrentPosition += 1;
                        }

                        tmpPrimary += "T";
                        tmpSecondary += "T";
                        break;

                    case "V":
                        if (InputString.Substring(iCurrentPosition + 1, 1) == "V")
                        {
                            iCurrentPosition += 2;
                        }
                        else
                        {
                            iCurrentPosition += 1;
                        }

                        tmpPrimary += "F";
                        tmpSecondary += "F";
                        break;

                    case "W":
                        //can also be in middle of word
                        if (InputString.Substring(iCurrentPosition, 2) == "WR")
                        {
                            tmpPrimary += "R";
                            tmpSecondary += "R";
                            iCurrentPosition += 2;
                            break;
                        }

                        if (iCurrentPosition == 0 &&
                            (ContainsAnyOf(InputString.Substring(iCurrentPosition + 1, 1), "A", "E", "I", "O", "U", "Y") ||
                            InputString.Substring(iCurrentPosition, 2) == "WH"))
                        {
                            //Wasserman should match Vasserman
                            if (ContainsAnyOf(InputString.Substring(iCurrentPosition + 1, 1), "A", "E", "I", "O", "U", "Y"))
                            {
                                tmpPrimary += "A";
                                tmpSecondary += "F";
                            }
                            else
                            {
                                //need Uomo to match Womo
                                tmpPrimary += "F";
                                tmpSecondary += "F";
                            }
                        }

                        //Arnow should match Arnoff
                        if ((iCurrentPosition == Last &&
                            (iCurrentPosition > 0 && ContainsAnyOf(InputString.Substring(iCurrentPosition - 1, 1), "A", "E", "I", "O", "U", "Y"))) ||
                            (iCurrentPosition > 0 && ContainsAnyOf(InputString.Substring(iCurrentPosition - 1, 5), "EWSKI", "EWSKY", "OWSKI", "OWSKY")) ||
                            InputString.Substring(0, 3) == "SCH")
                        {
                            tmpSecondary += "F";
                            iCurrentPosition += 1;
                            break;
                        }

                        // polish e.g. 'filipowicz'
                        if (ContainsAnyOf(InputString.Substring(iCurrentPosition, 4), "WICZ", "WITZ"))
                        {
                            tmpPrimary += "TS";
                            tmpSecondary += "FX";
                            iCurrentPosition += 4;
                            break;
                        }

                        iCurrentPosition += 1;
                        break;

                    case "X":
                        // french e.g. breaux
                        if (!(iCurrentPosition == Last &&
                            ((iCurrentPosition > 2 && ContainsAnyOf(InputString.Substring(iCurrentPosition - 3, 3), "IAU", "EAU")) ||
                            (iCurrentPosition > 1 && ContainsAnyOf(InputString.Substring(iCurrentPosition - 2, 2), "AU", "OU")))))
                        {
                            tmpPrimary += "KS";
                            tmpSecondary += "KS";
                        }

                        if (ContainsAnyOf(InputString.Substring(iCurrentPosition + 1, 1), "C", "X"))
                        {
                            iCurrentPosition += 2;
                        }
                        else
                        {
                            iCurrentPosition += 1;
                        }
                        break;

                    case "Z":
                        //Chinese pinyin e.g. 'zhao'
                        if (InputString.Substring(iCurrentPosition + 1, 1) == "H")
                        {
                            tmpPrimary += "J";
                            tmpSecondary += "J";
                            iCurrentPosition += 2;
                            break;
                        }
                        else if (ContainsAnyOf(InputString.Substring(iCurrentPosition + 1, 2), "ZO", "ZI", "ZA") ||
                            (IsSlavoGermanic(InputString) &&
                            (iCurrentPosition > 0 &&
                            InputString.Substring(iCurrentPosition - 1, 1) != "T")))
                        {
                            tmpPrimary += "S";
                            tmpSecondary += "TS";
                        }
                        else
                        {
                            tmpPrimary += "S";
                            tmpSecondary += "S";
                        }

                        if (InputString.Substring(iCurrentPosition + 1, 1) == "Z")
                        {
                            iCurrentPosition += 2;
                        }
                        else
                        {
                            iCurrentPosition += 1;
                        }
                        break;
                    default:
                        iCurrentPosition += 1;
                        break;
                }
            } while (iCurrentPosition <= Last);

            return tmpPrimary;
        }

      

    }
}
