using System.Collections.Immutable;
using System.Security;
using System.Text;
using System.Text.Json;
using TourDePologne;
using TourDePologne.Enums;

string _APPLICATION_NAME = "TourDePologne";
string _FILE_NAME = "tour_de_pologne.json";

if (args.Length == 0)
{
    WriteCommands();

    return;
}

switch (args[0])
{
    case "path":
        try
        {
            Console.WriteLine(GetPath());
        }
        catch (Exception e)
        {
            Console.WriteLine("There was an exception while retrieving path");
        }

        return;

    case "list":
        try
        {
            Filter filter;
            OrderBy order;

            if (args.Length >= 2)
            {
                try
                {
                    filter = Enum.Parse<Filter>(args[1]);
                }
                catch
                {
                    Console.WriteLine("Invalid filter, using default filter: Position");

                    filter = Filter.Position;
                }
            }
            else
            {
                filter = Filter.Position;
            }

            if (args.Length >= 3)
            {
                try
                {
                    order = Enum.Parse<OrderBy>(args[2]);
                }
                catch
                {
                    Console.WriteLine("Invalid order, using default order: Asc");

                    order = OrderBy.Asc;
                }
            }
            else
            {
                order = OrderBy.Asc;
            }

            var race = GetRace();
            var cyclists = race.Cyclists;

            switch (filter)
            {
                case Filter.Position:
                    if (order == OrderBy.Desc)
                    {
                        cyclists = cyclists.Reverse().ToImmutableList();
                    }

                    Console.WriteLine(FormatCyclists(cyclists));

                    return;

                case Filter.Number:
                    if (order == OrderBy.Desc)
                    {
                        cyclists = cyclists.OrderByDescending(c => c.Number).ToImmutableList();
                    }
                    else
                    {
                        cyclists = cyclists.OrderBy(c => c.Number).ToImmutableList();
                    }

                    Console.WriteLine(FormatCyclists(cyclists));

                    return;

                case Filter.DateOfBirth:
                    if (order == OrderBy.Desc)
                    {
                        cyclists = cyclists.OrderByDescending(c => c.DateOfBirth).ToImmutableList();
                    }
                    else
                    {
                        cyclists = cyclists.OrderBy(c => c.DateOfBirth).ToImmutableList();
                    }

                    Console.WriteLine(FormatCyclists(cyclists));

                    return;

                case Filter.Sex:
                    Sex sex;

                    if (args.Length >= 4)
                    {
                        try
                        {
                            sex = Enum.Parse<Sex>(args[3]);
                        }
                        catch
                        {
                            Console.WriteLine("Inavlid sex, using default: Male");

                            sex = Sex.Male;
                        }
                    }
                    else
                    {
                        sex = Sex.Male;
                    }

                    if (order == OrderBy.Desc)
                    {
                        cyclists = cyclists.Where(c => c.Sex == sex).OrderByDescending(c => c.Number).ToImmutableList();
                    }
                    else
                    {
                        cyclists = cyclists.Where(c => c.Sex == sex).OrderBy(c => c.Number).ToImmutableList();
                    }

                    Console.WriteLine(FormatCyclists(cyclists));

                    return;

                case Filter.Nationality:
                    Nationality nationality;

                    if (args.Length >= 4)
                    {
                        try
                        {
                            nationality = Enum.Parse<Nationality>(args[3]);
                        }
                        catch
                        {
                            Console.WriteLine("Invalid nationality, using default: Poland");

                            nationality = Nationality.Poland;
                        }
                    }
                    else
                    {
                        nationality = Nationality.Poland;
                    }

                    if (order == OrderBy.Desc)
                    {
                        cyclists = cyclists.Where(c => c.Nationality == nationality).OrderByDescending(c => c.Number).ToImmutableList();
                    }
                    else
                    {
                        cyclists = cyclists.Where(c => c.Nationality == nationality).OrderBy(c => c.Number).ToImmutableList();
                    }

                    Console.WriteLine(FormatCyclists(cyclists));

                    return;

                case Filter.Experience:
                    Experience experience;

                    if (args.Length >= 4)
                    {
                        try
                        {
                            experience = Enum.Parse<Experience>(args[3]);
                        }
                        catch
                        {
                            Console.WriteLine("Invalid experience, using default: Amateur");

                            experience = Experience.Amateur;
                        }
                    }
                    else
                    {
                        experience = Experience.Amateur;
                    }

                    if (order == OrderBy.Desc)
                    {
                        cyclists = cyclists.Where(c => c.Experience == experience).OrderByDescending(c => c.Number).ToImmutableList();
                    }
                    else
                    {
                        cyclists = cyclists.Where(c => c.Experience == experience).OrderBy(c => c.Number).ToImmutableList();
                    }

                    Console.WriteLine(FormatCyclists(cyclists));

                    return;

                default:
                    Console.Write(race.ToString());
                    return;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("There was an exception while reading data");
        }

        return;

    case "add":
        try
        {
            var number = int.Parse(args[1]);
            var firstName = args[2];
            var lastName = args[3];
            var dateOfBirth = DateTime.Parse(args[4]);
            var sex = Enum.Parse<Sex>(args[5]);
            var nationality = Enum.Parse<Nationality>(args[6]);
            var experience = Enum.Parse<Experience>(args[7]);

            var cyclist = new Cyclist(number, firstName, lastName, dateOfBirth, sex, nationality, experience);

            var race = GetRace();

            race.AddCyclist(cyclist);

            SaveRace(race);
        }
        catch (Exception e)
        {
            Console.WriteLine("There was an error while adding new cyclist");
        }

        return;

    case "update":
        try
        {
            var number = int.Parse(args[1]);
            var firstName = args[2];
            var lastName = args[3];
            var dateOfBirth = DateTime.Parse(args[4]);
            var sex = Enum.Parse<Sex>(args[5]);
            var nationality = Enum.Parse<Nationality>(args[6]);
            var experience = Enum.Parse<Experience>(args[7]);

            var race = GetRace();

            var cyclist = race.GetCyclist(number);

            if (cyclist == null)
            {
                Console.WriteLine("Cyclist with this number does not exist");

                return;
            }

            cyclist.Update(firstName, lastName, dateOfBirth, sex, nationality, experience);

            race.UpdateCyclist(cyclist);

            SaveRace(race);
        }
        catch (Exception e)
        {
            Console.WriteLine("There was an error while updating cyclist");
        }

        return;

    case "remove":
        try
        {
            var number = int.Parse(args[1]);

            var race = GetRace();

            race.RemoveCyclist(number);

            SaveRace(race);
        }
        catch (Exception e)
        {
            Console.WriteLine("There was an error while removing cyclist");

        }

        return;

    case "swap":
        try
        {
            var number1 = int.Parse(args[1]);
            var number2 = int.Parse(args[2]);

            var race = GetRace();

            race.SwapCyclists(number1, number2);

            SaveRace(race);
        }
        catch (Exception e)
        {
            Console.WriteLine("There was an error while swapping cyclists");
        }

        return;

    default:
        Console.WriteLine("Invalid command");

        WriteCommands();

        return;
}

void WriteCommands()
{
    Console.WriteLine("path");
    Console.WriteLine("\tReturns path to the file with data\n");
    Console.WriteLine("list [filter = position] [order = asc/desc]");
    Console.WriteLine("\tReturns list of cyclists\n");
    Console.WriteLine("add [number] [firstName] [lastName] [dateOfBirth] [sex] [nationality] [experience]");
    Console.WriteLine("\tAdds new cyclist to the list\n");
    Console.WriteLine("update [number] [firstName] [lastName] [dateOfBirth] [sex] [nationality] [experience]");
    Console.WriteLine("\tUpdates cyclist on the list\n");
    Console.WriteLine("remove [number]");
    Console.WriteLine("\tRemoves cyclist from the list\n");
    Console.WriteLine("swap [position1] [position2]");
    Console.WriteLine("\tSwaps cyclists on given positions - positions must be adjacent\n");
}

string GetPath()
{
    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _APPLICATION_NAME, _FILE_NAME);
}

Race GetRace()
{
    List<Cyclist> cyclists;

    var path = GetPath();

    try
    {
        var json = File.ReadAllText(path, Encoding.UTF8);

        cyclists = JsonSerializer.Deserialize<List<Cyclist>>(json) ?? new List<Cyclist>();
    }
    catch
    {
        cyclists = new List<Cyclist>();
    }

    return new Race(cyclists);
}

void SaveRace(Race race)
{
    var path = GetPath();
    var directoryPath = Path.GetDirectoryName(path);

    if (directoryPath == null)
    {
        Console.WriteLine("Could not retrieve directory path");

        return;
    }

    try
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var json = JsonSerializer.Serialize(race.Cyclists);

        File.WriteAllText(path, json, Encoding.UTF8);
    }
    catch (SecurityException e)
    {
        Console.WriteLine("Breach of Security");
    }
    catch (UnauthorizedAccessException e)
    {
        Console.WriteLine("Unauthorized Access");
    }
    catch (IOException e)
    {
        Console.WriteLine("There was an error while saving data");
    }
    catch (Exception e)
    {
        Console.WriteLine("There was an error while saving data");
    }
}

string FormatCyclists(ImmutableList<Cyclist> cyclists)
{
    return string.Join("\n", cyclists.Select(c => c.ToString()));
}