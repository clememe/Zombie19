using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Zombie19
{
    public class Mechanics
    {
        private static Random random = new Random();
        private List<Character> characters = new List<Character>();
        private List<Group> groups = new List<Group>();

        public void CreateCharacters(int numberToCreate = 0, int numberGroups = 0)
        {
            if (numberToCreate <= characters.Count)
            {
                return;
            }
            characters.Add(new Character(numberGroups));
            CreateCharacters(numberToCreate, numberGroups);
        }

        public void CreateGroups(int numberToCreate = 0)
        {
            if (numberToCreate <= groups.Count)
            {
                return;
            }
            groups.Add(new Group());
            CreateGroups(numberToCreate);
        }

        public void CheckOneOrmOREInfected(bool oneInfected = false, int index = 0)
        {
            if (index >= characters.Count)
            {
                if (!oneInfected)
                {
                    int infectOne = random.Next(0, characters.Count);
                    characters[infectOne].Infected = true;

                    Console.WriteLine("We did not find any infected characters, so we infected: " + characters[infectOne].Name);
                }
                return;
            }
            if (characters[index].Infected)
            {
                oneInfected = true;
            }
            index++;
            CheckOneOrmOREInfected(oneInfected, index);
        }

        public void SortCharactersInGroups(int index = 0, int groupIndex = 0)
        {
            if (index >= characters.Count)
            {
                if (groupIndex >= groups.Count)
                {
                    return;
                }
                groupIndex++;
                index = 0;
            }

            if (characters[index].GroupLevel == groupIndex)
            {
                groups[groupIndex].Characters.Add(characters[index]);
                characters[index].GroupIndex = groups[groupIndex].Characters.Count - 1;
            }
            index++;
            SortCharactersInGroups(index, groupIndex);
        }

        public void KeepWorking()
        {
            DetectInfected();
            DisplayGroups();
            CheckEveryoneInfected();
        }



        public void CheckEveryoneInfected()
        {
            bool allInfecteds = characters.All(character => character.Infected);

            if (allInfecteds)
            {
                Console.WriteLine("Everyone is infected, it's over.");
                return;
            }

            Console.WriteLine("\n\nPress V to go to the vaccine menu or press any key to continue...");

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.V)
            {
                // Call your method for the vaccine menu here
                VaccinationMenu();
            }
            else
            {
                KeepWorking();
            }
        }

        public void VaccinationMenu()
        {
            Console.WriteLine("Here is all the availlable options :\n" +
                "A) A.1 Vaccine\n" +
                "B) B.1 Vaccine\n" +
                "C) Ultime Vaccine");

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            switch (keyInfo.Key)
            {
                case ConsoleKey.A:

                    ChooseWhoToVaccinate(0, "A.1-Vaccine");
                    break;

                case ConsoleKey.B:

                    ChooseWhoToVaccinate(1, "B.1-Vaccine");
                    break;

                case ConsoleKey.C:

                    ChooseWhoToVaccinate(2, "Ultime-Vaccine");
                    break;

                default:
                    Console.WriteLine("Unknown Key, the game continue!");
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variationVaccine">
        /// 0 = A1
        /// 1 = B2
        /// 2 = Ultime
        /// </param>
        public void ChooseWhoToVaccinate(int variationVaccine, string vaccineName)
        {
            int numberOfGroups = groups.Count -1;
            Console.WriteLine("\nThere is " +  numberOfGroups+ " groups starting by 0, which group do you want to select?");
            int.TryParse(Console.ReadLine(), out int groupToSelect);
            if (groupToSelect > numberOfGroups)
            {
                KeepWorking();
                return;
            }
            Console.WriteLine("Here is the content of the group n°" + groupToSelect + " :");
            DisplayCharactersFromGroup(groupToSelect);

            Console.WriteLine("\n Select the character by his number to apply the : " + vaccineName);
            int.TryParse(Console.ReadLine(), out int characterToSelect);
            if (characterToSelect >= characters.Count)
            {
                KeepWorking();
                return;
            }

            switch (variationVaccine)
            {
                case 0:
                    if (groups[groupToSelect].Characters[characterToSelect].Age <= 30)
                    {
                        groups[groupToSelect].Characters[characterToSelect].Vaccined = "A.1";
                    }                    
                break;
                case 1:
                    bool dead = random.Next(0, 2) == 1;
                    if (dead == true)
                    {
                        groups[groupToSelect].Characters[characterToSelect].Dead = true;
                        Console.WriteLine("You killed your character, you can't use him anymore.");
                    }
                    else
                    {
                        groups[groupToSelect].Characters[characterToSelect].Vaccined = "B.1";
                    }                    
                break;
                case 2:
                    groups[groupToSelect].Characters[characterToSelect].Vaccined = "Ultime";
                break;
            }
            KeepWorking();
        }

        public void DisplayGroups(int groupIndex = 0)
        {
            if (groupIndex >= groups.Count)
            {
                return;
            }
            Console.WriteLine("\nHere is the group n°: " + groupIndex);
            DisplayCharactersFromGroup(groupIndex);
            groupIndex++;
            DisplayGroups(groupIndex);
        }

        public void DisplayCharactersFromGroup(int groupIndex, int index = 0)
        {
            if (index >= groups[groupIndex].Characters.Count)
            {
                return;
            }
            Console.WriteLine($"{index}) {groups[groupIndex].Characters[index].Name} have {groups[groupIndex].Characters[index].Age} years old{(groups[groupIndex].Characters[index].Infected ? $", is infected and the variation is {groups[groupIndex].Characters[index].Variation}" : "")}");
            index++;
            DisplayCharactersFromGroup(groupIndex, index);
        }

        public void DetectInfected(List<Infection>? infection = null, int index = 0)
        {
            if (infection == null)
            {
                infection = new List<Infection>();
            }

            if (index >= characters.Count)
            {
                SpreadInfection(infection);
                return;
            }

            Character character = characters[index];
            if (character.Infected)
            {
                infection.Add(new Infection(character));
            }
            index++;
            DetectInfected(infection, index);
            return;
        }

        public void SpreadInfection(List<Infection> infections, int index = 0)
        {
            if (index >= infections.Count)
            {
                return;
            }

            int groupLevel = infections[index].Character.GroupLevel;
            string characterVariation = infections[index].Character.Variation;

            switch (characterVariation)
            {
                case "Zombie-A":
                    if (groupLevel > 0)
                    {
                        InfectZombieA(groupLevel - 1, groups[groupLevel - 1].Characters.Count -1);
                    }
                    index++;
                    SpreadInfection(infections, index);
                    return;
                case "Zombie-B":
                    if (groupLevel < groups.Count)
                    {
                        InfectZombieB(groupLevel + 1, 0);
                    }
                    index++;
                    SpreadInfection(infections, index);
                    return;

                case "Zombie-32":
                    InfectZombie32();
                    index++;
                    SpreadInfection(infections, index);
                    return;

                case "Zombie-C":
                    InfectZombieC(groupLevel);
                    index++;
                    SpreadInfection(infections, index);
                    return;

                case "Zombie-Ultime":
                    InfectZombieUltime(0);
                    index++;
                    SpreadInfection(infections, index);
                    return;
            }
            index++;
            SpreadInfection(infections, index);
        }

        public void InfectZombieA(int groupLevel, int characterIndex)
        {
            if (characterIndex <= 0)
            {
                groupLevel--;
                if (groupLevel < 0)
                {
                    return;
                }
                characterIndex = groups[groupLevel].Characters.Count - 1;
            }

            if (characterIndex >= 0 && characterIndex < groups[groupLevel].Characters.Count &&
                !groups[groupLevel].Characters[characterIndex].Infected)
            {
                groups[groupLevel].Characters[characterIndex].Infected = true;
                groups[groupLevel].Characters[characterIndex].Variation = "Zombie-A";
                return;
            }

            characterIndex--;
            InfectZombieA(groupLevel, characterIndex);
        }


        public void InfectZombieB(int groupLevel, int characterIndex)
        {
            if (groupLevel >= groups.Count)
            {
                return;
            }
            if (characterIndex >= groups[groupLevel].Characters.Count)
            {
                groupLevel++;
                if (groupLevel >= groups.Count)
                {
                    return;
                }
                characterIndex = 0;
            }

            if (groups[groupLevel].Characters[characterIndex].Infected == false)
            {
                groups[groupLevel].Characters[characterIndex].Infected = true;
                groups[groupLevel].Characters[characterIndex].Variation = "Zombie-B";
                return;
            }

            characterIndex++;
            InfectZombieB(groupLevel, characterIndex);
        }

        public void InfectZombie32(int index = 0)
        {
            if (index >= characters.Count)
            {
                return;
            }

            if (characters[index].Age > 32 && characters[index].Infected == false)
            {
                characters[index].Infected = true;
                characters[index].Variation = "Zombie-32";
                return;
            }

            index++;
            InfectZombie32(index);
        }

        public void InfectZombieC(int groupLevel, int index = 0)
        {
            if (index >= characters.Count)
            {
                return;
            }

            if (characters[index].GroupLevel != groupLevel && characters[index].Infected == false)
            {
                characters[index].Infected = true;
                characters[index].Variation = "Zombie-C";
                return;
            }

            index++;
            InfectZombieC(groupLevel, index);
        }

        public void InfectZombieUltime(int characterIndex)
        {
            int groupLevel = groups.Count - 1;
            if (characterIndex >= groups[groupLevel].Characters.Count) 
            {
                return;
            }

            if (characters[characterIndex].Infected == false)
            {
                characters[characterIndex].Infected = true;
                characters[characterIndex].Variation = "Zombie-Ultime";
                return;
            }

            characterIndex++;
            InfectZombieUltime(characterIndex);
        }
    }
}
